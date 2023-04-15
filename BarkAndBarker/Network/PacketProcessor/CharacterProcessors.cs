using BarkAndBarker.Persistence.Models;
using DC.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Network.PacketProcessor
{
    internal class CharacterProcessors
    {
        public static object HandleCharacterCreateReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_ACCOUNT_CHARACTER_CREATE_REQ>();
            var response = new SS2C_ACCOUNT_CHARACTER_CREATE_RES();

            var charsWithNickname = session.GetDB().SelectFirst<int>(ModelCharacter.QuerySelectAllNicknames, new { Nickname = request.NickName });
#if USE_STEAM
            var userChars = session.GetDB().Select<ModelCharacter>(ModelCharacter.QuerySelectAllByUserAccount, new { AID = session.m_currentPlayer.SteamID });
#else
            var userChars = session.GetDB().Select<ModelCharacter>(ModelCharacter.QuerySelectAllByUserAccount, new { AID = session.m_currentPlayer.AccountID }); 
#endif

            if (userChars.Count() > 7 || charsWithNickname > 0)
            {
                response.Result = (uint)LoginResponseResult.FAIL_PASSWORD;
                return response;
            }

            var queryRes = session.GetDB().Execute(ModelCharacter.QueryCreateCharacter, new
            {
#if USE_STEAM
                AID = session.m_currentPlayer.SteamID,
#else
                AID = session.m_currentPlayer.AccountID,
#endif
                CID = Guid.NewGuid().ToString(),
                Nickname = request.NickName,
                Class = request.CharacterClass,
                Level = 666,
                request.Gender,
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

            var serial = new WrapperSerializer<SS2C_ACCOUNT_CHARACTER_CREATE_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CAccountCharacterCreateRes);
            return serial.Serialize();
        }

        public static object HandleCharacterListReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_ACCOUNT_CHARACTER_LIST_REQ>();
            var response = new SS2C_ACCOUNT_CHARACTER_LIST_RES();

#if USE_STEAM
            var characters = session.GetDB().Select<ModelCharacter>(ModelCharacter.QuerySelectAllByUserAccount, new { AID = session.m_currentPlayer.SteamID });
#else
            var characters = session.GetDB().Select<ModelCharacter>(ModelCharacter.QuerySelectAllByUserAccount, new { AID = session.m_currentPlayer.AccountID });
#endif
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

            var serial = new WrapperSerializer<SS2C_ACCOUNT_CHARACTER_LIST_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CAccountCharacterListRes);
            return serial.Serialize();
        }


        public static object HandleCharacterDeletionReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_ACCOUNT_CHARACTER_DELETE_REQ>();
            var response = new SS2C_ACCOUNT_CHARACTER_DELETE_RES();

            var selectedCharacter = session.GetDB().SelectFirst<ModelCharacter>(ModelCharacter.QuerySelectCharacterByID, new { CID = request.CharacterId });
            if (selectedCharacter != null)
            {
                var charOwnedAccount = session.GetDB().SelectFirst<ModelAccount>(ModelCharacter.QueryOwnerAccountForCharacterID, new { CID = request.CharacterId });
#if USE_STEAM
                if (charOwnedAccount.SteamID != null && charOwnedAccount.SteamID == session.m_currentPlayer.SteamID)
                {
                    // TODO: Make this a transaction, if deletion will affect more than 1 row we need to rollback as something went wrong.
                    var deletedCharactersCount = session.GetDB().Execute(ModelCharacter.QueryDeleteCharacter, new
                    {
                        AID = charOwnedAccount.SteamID,
                        CID = selectedCharacter.CharID,
                    });

                    if (deletedCharactersCount >= 1)
                        response.Result = (uint)LoginResponseResult.SUCCESS;
                }
#else
                if (charOwnedAccount.ID != null && charOwnedAccount.ID == session.m_currentPlayer.AccountID)
                {
                    // TODO: Make this a transaction, if deletion will affect more than 1 row we need to rollback as something went wrong.
                    var deletedCharactersCount = session.GetDB().Execute(ModelCharacter.QueryDeleteCharacter, new
                    {
                        AID = charOwnedAccount.ID,
                        CID = selectedCharacter.CharID,
                    });

                    if (deletedCharactersCount >= 1)
                        response.Result = (uint)LoginResponseResult.SUCCESS;
                }
#endif
            }
            else
                response.Result = (uint)LoginResponseResult.FAIL_PASSWORD;

            return response;
        }

        public static MemoryStream HandleCharacterDeletionRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_ACCOUNT_CHARACTER_DELETE_RES)inputClass;
            var serial = new WrapperSerializer<SS2C_ACCOUNT_CHARACTER_DELETE_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CAccountCharacterListRes);
            return serial.Serialize();
        }
    }
}
