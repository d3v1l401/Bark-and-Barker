using BarkAndBarker.Steam;
using DC.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarkAndBarker.Shared.Persistence.Models;

namespace BarkAndBarker.Network.PacketProcessor
{
    internal class AccountProcessors
    {
        private const uint STEAM_APPID = 211;

        public static object HandleLoginReq(ClientSession session, dynamic deserializer)
        {
            var des = (WrapperDeserializer)deserializer;
            var loginData = des.Parse<SC2S_ACCOUNT_LOGIN_REQ>();

            var bytes = Enumerable.Range(0, loginData.LoginId.Length / 2)
                        .Select(i => Convert.ToByte(loginData.LoginId.Substring(i * 2, 2), 16))
                        .ToArray();
            var parsed = SteamTicket.ParseAppTicket(bytes);

            var inputedSteamID = parsed.SteamID.AccountID.ToString();

            session.GetDB().Execute(ModelAccount.QueryUpdateLastLogin, null);
            var loggedPlayer = session.GetDB().SelectFirst<ModelAccount>(ModelAccount.QuerySelectAccount, new { SID = inputedSteamID });
            if (loggedPlayer == default(ModelAccount)) // User does not exist, create it
            {
                var created = session.GetDB().Execute(ModelAccount.QueryCreateAccount, new { SID = inputedSteamID });
                if (created != 1)
                {
                    loggedPlayer = new ModelAccount()
                    {
                        SteamID = inputedSteamID,
                        State = (int)LoginResponseResult.FAIL_PASSWORD, // Will report to the client an account already exists
                    };
                }
                else
                    loggedPlayer = session.GetDB().SelectFirst<ModelAccount>(ModelAccount.QuerySelectAccount, new { SID = inputedSteamID });
            }

            // Same Steam ID that is in the client's steam_appid.txt
            if (parsed.AppID != STEAM_APPID)
            {
                loggedPlayer = new ModelAccount()
                {
                    SteamID = inputedSteamID,
                    State = (int)LoginResponseResult.FAIL_STEAM_BUILD_ID, // Will report to the client an account already exists
                };
            }

            // Invalid steam ownership ticket
            if (!parsed.IsValid || !parsed.HasValidSignature)
            {
                loggedPlayer = new ModelAccount()
                {
                    SteamID = inputedSteamID,
                    State = (int)LoginResponseResult.FAIL_PASSWORD, // Will report to the client an account already exists
                };
            }

            // Can proceed as creation of account has a unique SteamID.
            if (loggedPlayer?.State != (int)LoginResponseResult.FAIL_PASSWORD)
            {
                var loggingHWID = Helpers.GetHWID(loginData.HwIds.ToArray());

                if (loggedPlayer?.HWID != null) // Not first login
                {
                    var referencedAccounts = session.GetDB().Select<ModelAccount>(ModelAccount.QueryFindDuplicateHWID, new { HWID = loggingHWID });
                    if (referencedAccounts.Count() > 1)
                        loggedPlayer.State = (int)LoginResponseResult.FAIL_LOGIN_BAN_HARDWARE;
                }
                else
                {
                    session.GetDB().Execute(ModelAccount.QueryUpdateHWID, new { HWID = session.m_currentPlayer.CurrentHWID });
                    session.GetDB().Execute(ModelAccount.QueryUpdateLastLogin, new { IP = parsed.OwnershipTicketExternalIP.ToString() });
                }

                session.m_currentPlayer.CurrentHWID = loggingHWID;
            }

            session.SteamId = inputedSteamID;

            return loggedPlayer;
        }

        public static MemoryStream HandleLoginRes(ClientSession session, dynamic inputClass)
        {
            var loggedPlayer = (ModelAccount)inputClass;

            var responsePacket = new SS2C_ACCOUNT_LOGIN_RES()
            {
                AccountId = loggedPlayer.SteamID.ToString(),
                SessionId = session.Id.ToString(),
                ServerLocation = 3, // TODO
                IsReconnect = 0, // TODO
                Address = "", // TODO
                Result = (uint)loggedPlayer.State,
                AccountInfo = new SLOGIN_ACCOUNT_INFO() { AccountID = loggedPlayer.SteamID.ToString() }
            };

            session.m_currentPlayer.SteamID = loggedPlayer.SteamID.ToString();

            var serializer = new WrapperSerializer<SS2C_ACCOUNT_LOGIN_RES>(responsePacket, session.m_currentPacketSequence++, PacketCommand.S2CAccountLoginRes);
            return serializer.Serialize();
        }
    }
}
