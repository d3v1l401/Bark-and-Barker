using Azure;
using BarkAndBarker.Persistence;
using BarkAndBarker.Session;
using BarkAndBarker.Steam;
using DC.Packet;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Collections;

namespace BarkAndBarker.Network
{
    internal class Helpers
    {
        public static string GetHWID(string[] hashes)
        {
            SHA512 sha = SHA512.Create();

            var final = "";
            foreach (var hash in hashes)
                final += hash + "-";

            var rawHash = sha.ComputeHash(final.ToByteArray());
            return Convert.ToBase64String(rawHash);
        }
    }
    public class PacketProcessors
    {
        private const uint STEAM_APPID = 211;

        public static object HandleAliveReq(ClientSession session, dynamic deserializer)
        {
            var des = (WrapperDeserializer)deserializer;
            return des.Parse<SC2S_ALIVE_REQ>();
        }

        public static MemoryStream HandleAliveRes(ClientSession session, dynamic inputClass)
        {
            var res = new SS2C_ALIVE_RES();
            var response = new WrapperSerializer<SS2C_ALIVE_RES>(res, PacketCommand.S2CAliveRes);
            return response.Serialize();
        }

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
                        State = (int)LoginResponseResult.FAIL_OVERFLOW_ID_OR_PASSWORD, // Will report to the client an account already exists
                    };
                } else
                    loggedPlayer = session.GetDB().SelectFirst<ModelAccount>(ModelAccount.QuerySelectAccount, new { SID = inputedSteamID });
            }

            // Same Steam ID that is in the client's steam_appid.txt
            if (parsed.AppID != STEAM_APPID)
            {
                loggedPlayer = new ModelAccount()
                {
                    SteamID = inputedSteamID,
                    State = (int)LoginResponseResult.FAIL_OVERFLOW_ID_OR_PASSWORD, // Will report to the client an account already exists
                };
            }

            // Can proceed as creation of account has a unique SteamID.
            if (loggedPlayer?.State != (int)LoginResponseResult.FAIL_OVERFLOW_ID_OR_PASSWORD)
            {
                var loggingHWID = Helpers.GetHWID(loginData.HwIds.ToArray());

                if (loggedPlayer?.HWID != null) // Not first login
                {
                    var referencedAccounts = session.GetDB().Select<ModelAccount>(ModelAccount.QueryFindDuplicateHWID, new { HWID = loggingHWID });
                    if (referencedAccounts.Count() > 1)
                        loggedPlayer.State = (int)LoginResponseResult.FAIL_LOGIN_BAN_HARDWARE;
                } else {
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

            var serializer = new WrapperSerializer<SS2C_ACCOUNT_LOGIN_RES>(responsePacket, PacketCommand.S2CAccountLoginRes);
            return serializer.Serialize();
        }

        public static object HandleCharacterCreateReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_ACCOUNT_CHARACTER_CREATE_REQ>();
            var response = new SS2C_ACCOUNT_CHARACTER_CREATE_RES();

            var characters = session.GetDB().Select<ModelCharacter>(ModelCharacter.QuerySelectAllByUserAccount, new { AID = session.SteamId });

            if (characters.Count() > 7)
            {
                response.Result = (uint)LoginResponseResult.FAIL_PASSWORD;
            }

            if (characters.Any(x => x.Nickname == request.NickName))
            {
                response.Result = (uint)LoginResponseResult.FAIL_PASSWORD;
            }

            var queryRes = session.GetDB().Execute(ModelCharacter.QueryCreateCharacter, new
            {
                AID = session.m_currentPlayer.SteamID,
                CID = Guid.NewGuid().ToString(),
                Nickname = request.NickName,
                Class = request.CharacterClass,
                Level = 666,
                Gender = request.Gender,
            });

            if (queryRes > 0)
                response.Result = (uint)LoginResponseResult.SUCCESS;
            else
                response.Result = (uint)LoginResponseResult.FAIL_PASSWORD;

            return response;
        }

        public static MemoryStream HandleCharacterCreateRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_ACCOUNT_CHARACTER_CREATE_RES)inputClass;

            var serial = new WrapperSerializer<SS2C_ACCOUNT_CHARACTER_CREATE_RES>(response, PacketCommand.S2CAccountCharacterCreateRes);
            return serial.Serialize();
        }

        public static object HandleCharacterListReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_ACCOUNT_CHARACTER_LIST_REQ>();
            var response = new SS2C_ACCOUNT_CHARACTER_LIST_RES();

            var characters = session.GetDB().Select<ModelCharacter>(ModelCharacter.QuerySelectAllByUserAccount, new { AID = session.SteamId });
            if (characters.Count() > 7)
            {
                session.Disconnect(); // Ugly, client won't know why but theoretically we should never hit this.
            }

            foreach (var character in characters)
            {
                var createdAtUnixTS = new DateTimeOffset(character.CreatedAt);
                var lastLotinUnixTS = new DateTimeOffset(character.LastLogin);
                response.CharacterList.Add(new SLOGIN_CHARACTER_INFO()
                {
                    CharacterClass = character.Class,
                    CharacterId = character.CharID,
                    CreateAt = (ulong)createdAtUnixTS.ToUnixTimeSeconds(),
                    Gender = (uint)character.Gender,
                    LastloginDate = (ulong)lastLotinUnixTS.ToUnixTimeSeconds(),
                    Level = (uint)character.Level,
                    NickName = new SACCOUNT_NICKNAME()
                    {
                        OriginalNickName = character.Nickname,
                        StreamingModeNickName = character.Nickname,
                        KarmaRating = character.KarmaScore,
                    }
                });
            }

            return response;
        }

        public static MemoryStream HandleCharacterListRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_ACCOUNT_CHARACTER_LIST_RES)inputClass;

            response.PageIndex = 1; // Only supporting 1 page for now
            response.TotalCharacterCount = (uint)response.CharacterList.Count;

            var serial = new WrapperSerializer<SS2C_ACCOUNT_CHARACTER_LIST_RES>(response, PacketCommand.S2CAccountCharacterListRes);
            return serial.Serialize();
        }

        public static object HandleLobbyEnterReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_LOBBY_ENTER_REQ>();
            var response = new SS2C_LOBBY_ENTER_RES();

            var selectedCharacter = session.GetDB().SelectFirst<ModelCharacter>(ModelCharacter.QuerySelectCharacterByID, new { CID = request.CharacterId });
            if (selectedCharacter != null)
            {
                var charOwnedAccount = session.GetDB().SelectFirst<ModelAccount>(ModelCharacter.QueryOwnerAccountForCharacterID, new { CID = request.CharacterId });
                if (charOwnedAccount.SteamID != null && charOwnedAccount.SteamID == session.m_currentPlayer.SteamID)
                {
                    session.m_currentCharacter = selectedCharacter;
                    response.Result = (uint)LoginResponseResult.SUCCESS;
                }
            } else
                response.Result = (uint)LoginResponseResult.FAIL_PASSWORD;

            return response;
        }

        public static MemoryStream HandleLobbyEnterRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_LOBBY_ENTER_RES)inputClass;
            response.AccountId = session.m_currentPlayer.SteamID;

            var serial = new WrapperSerializer<SS2C_LOBBY_ENTER_RES>(response, PacketCommand.S2CLobbyEnterRes);
            return serial.Serialize();
        }

        public static object HandleCustomizeCharacterInfoReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_CUSTOMIZE_CHARACTER_INFO_REQ>();
            return new SS2C_CUSTOMIZE_CHARACTER_INFO_RES();
        }

        public static MemoryStream HandleCustomizeCharacterInfoRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_CUSTOMIZE_CHARACTER_INFO_RES)inputClass;

            // TODO: Wtf is this
            response.LoopFlag = 0;
            response.CustomizeCharacters.Add(new SCUSTOMIZE_CHARACTER
            {
                CustomizeCharacterId = session.m_currentCharacter.CharID,
                IsEquip = 1
            });

            var serial = new WrapperSerializer<SS2C_CUSTOMIZE_CHARACTER_INFO_RES>(response, PacketCommand.S2CCustomizeCharacterInfoRes);
            return serial.Serialize();
        }

        public static object HandleCustomizeActionInfoReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_CUSTOMIZE_ACTION_INFO_REQ>();
            return new SS2C_CUSTOMIZE_ACTION_INFO_RES();
        }

        public static MemoryStream HandleCustomizeActionInfoRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_CUSTOMIZE_ACTION_INFO_RES)inputClass;

            response.LoopFlag = 0;
            response.CustomizeLobbyActionIds.Add(new SCUSTOMIZE_ACTION()
            {
                CustomizeActionId = "", // Figure out action IDs
                IsEquip = 1,
            });

            var serial = new WrapperSerializer<SS2C_CUSTOMIZE_ACTION_INFO_RES>(response, PacketCommand.S2CCustomizeActionInfoRes);
            return serial.Serialize();
        }

        public static object HandleOpenLobbyMapReq(ClientSession session, dynamic deserializer)
        { 
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_OPEN_LOBBY_MAP_REQ>();
            return new SS2C_OPEN_LOBBY_MAP_RES();
        }

        public static MemoryStream HandleOpenLobbyMapRes(ClientSession session, dynamic inputClass)
        { 
            var response = (SS2C_OPEN_LOBBY_MAP_RES)inputClass;

            var serial = new WrapperSerializer<SS2C_OPEN_LOBBY_MAP_RES>(response, PacketCommand.S2COpenLobbyMapRes);
            return serial.Serialize();
        }

        public static object HandleMetaLocationReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_META_LOCATION_REQ>();
            var response = new SS2C_META_LOCATION_RES();

            response.Location = request.Location;

            return response;
        }

        public static MemoryStream HandleMetaLocationRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_META_LOCATION_RES)inputClass;
            var serial = new WrapperSerializer<SS2C_META_LOCATION_RES>(response, PacketCommand.S2CMetaLocationRes);
            return serial.Serialize();
        }

        public static object HandleClassEquipInfoReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_CLASS_EQUIP_INFO_REQ>();

            return new SS2C_CLASS_EQUIP_INFO_RES();
        }

        public static MemoryStream HandleClassEquipInfoRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_CLASS_EQUIP_INFO_RES)inputClass;
            
            // TODO: Find item IDs, handle all the data
            response.Equips.Add(new SCLASS_EQUIP_INFO
            {
                
            });

            var serial = new WrapperSerializer<SS2C_CLASS_EQUIP_INFO_RES>(response, PacketCommand.S2CClassEquipInfoRes);
            return serial.Serialize();
        }

        public static object HandleCustomizeItemInfoReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_CUSTOMIZE_ITEM_INFO_REQ>();
            return new SS2C_CUSTOMIZE_ITEM_INFO_RES();
        }

        public static MemoryStream HandleCustomizeItemInfoRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_CUSTOMIZE_ITEM_INFO_RES)inputClass;

            // TODO
            response.LoopFlag = 0;
            response.CustomizeItems.Add(new SCUSTOMIZE_ITEM()
            {

            });

            var serial = new WrapperSerializer<SS2C_CUSTOMIZE_ITEM_INFO_RES>(response, PacketCommand.S2CCustomizeItemInfoRes);
            return serial.Serialize();
        }

        public static object HandleClassLevelInfoReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_CLASS_LEVEL_INFO_REQ>();
            return new SS2C_CLASS_LEVEL_INFO_RES();
        }

        public static MemoryStream HandleClassLevelInfoRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_CLASS_LEVEL_INFO_RES)inputClass;

            // TODO: Leveling handling
            response.Level = (uint)session.m_currentCharacter.Level;
            response.Exp = 8000;
            response.ExpBegin = 1;
            response.ExpLimit = 10000;
            response.RewardPoint = 5;

            var serial = new WrapperSerializer<SS2C_CLASS_LEVEL_INFO_RES>(response, PacketCommand.S2CClassLevelInfoRes);
            return serial.Serialize();
        }

        public static object HandleMatchmakingReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_AUTO_MATCH_REG_REQ>();

            // TODO: Matchmaking; Mode 1 => enlist, Mode 2 => unenlist
            Console.WriteLine(request.ToString());
            if (request.Mode == 1)
                _ = Matchmaking.Instance().AddUser(session);
            else if (request.Mode == 2)
                _ = Matchmaking.Instance().RemoveUser(session);

            return new SS2C_AUTO_MATCH_REG_RES();
        }

        public static MemoryStream HandleMatchmakingRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_AUTO_MATCH_REG_RES)inputClass;

            // TODO: Enlist to matchmaking, deal with processing and notify player
            response.Result = (uint)MatchmakingResponseResult.SUCCESS;

            // TODO: This should be another thread
            Matchmaking.Instance().AcceptPlayers();
            _ = Matchmaking.Instance().Matchmake();

            var serial = new WrapperSerializer<SS2C_AUTO_MATCH_REG_RES>(response, PacketCommand.S2CAutoMatchRegRes);
            return serial.Serialize();
        }

        public static object HandleMerchantListReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_MERCHANT_LIST_REQ>();
            return new SS2C_MERCHANT_LIST_RES();
        }

        public static MemoryStream HandleMerchantListRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_MERCHANT_LIST_RES)inputClass;

            // TODO: Merchants list, store these in a DB but their inventory on Redis?
            response.MerchantList.Add(new SMERCHANT_INFO()
            {
                Faction = 1,
                IsUnidentified = 0,
                MerchantId = Guid.NewGuid().ToString(),
                RemainTime = 0,
            });

            response.MerchantList.Add(new SMERCHANT_INFO()
            {
                Faction = 2,
                IsUnidentified = 1,
                MerchantId = Guid.NewGuid().ToString(),
                RemainTime = 1092831,
            });

            var serial = new WrapperSerializer<SS2C_MERCHANT_LIST_RES>(response, PacketCommand.S2CAutoMatchRegRes);
            return serial.Serialize();
        }

        public static object HandleRankingReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_RANKING_RANGE_REQ>();

            var response = new SS2C_RANKING_RANGE_RES();
            response.RankType = request.RankType;
            response.StartIndex = request.StartIndex;
            response.EndIndex = request.EndIndex;
            response.CharacterClass = request.CharacterClass;

            return response;
        }

        public static MemoryStream HandleRankingRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_RANKING_RANGE_RES)inputClass;

            response.Result = (uint)MatchmakingResponseResult.SUCCESS;

            response.AllRowCount = 1;

            //TODO
            var record = new SRankRecord
            {
                PageIndex = 0,
                Rank = 1,
                Score = 6969,
                Percentage = 30,
                AccountId = "421421412",
                NickName = new SACCOUNT_NICKNAME()
                {
                    OriginalNickName = "BestOne",
                    StreamingModeNickName = "BestOne",
                    KarmaRating = 100,
                },
                CharacterClass = response.CharacterClass
            };

            response.Records.Add(record);


            var serial = new WrapperSerializer<SS2C_RANKING_RANGE_RES>(response, PacketCommand.S2CRankingRangeRes);
            return serial.Serialize();
        }
    }
}