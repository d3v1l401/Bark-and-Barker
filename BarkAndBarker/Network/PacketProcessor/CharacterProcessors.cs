using DC.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarkAndBarker.Shared.Persistence.Models;
using BarkAndBarker.Persistence;

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

            if (userChars.Count() > 7)
            {
                response.Result = (uint)LoginResponseResult.FAIL_PASSWORD;
                return response;
            }

            if (charsWithNickname > 0)
            {
                response.Result = (uint)LoginResponseResult.FAIL_LOGIN_BAN_USER_CHEATER;
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
                Level = 1,
                request.Gender,
            });

            if (queryRes > 0)
                response.Result = (uint)LoginResponseResult.SUCCESS;
            else
                response.Result = 0;

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

            // Fill the inventory 
            foreach (var character in response.CharacterList)
            {
                var items = InventoryHelpers.GetAllUserItems(session.GetDB(), character.CharacterId, false);
                foreach (var item in items)
                {
                    try
                    {
                        if (item.Key.InventoryID == (uint)InventoryType.INVENTORY_CHARACTER) // Skip stash items
                            character.EquipItemList.Add(InventoryHelpers.MakeSItemObject(item.Key));
                        if (item.Key.InventoryID == (uint)InventoryType.INVENTORY_STASH)
                            character.EquipItemList.Add(InventoryHelpers.MakeSItemObject(item.Key));

                    } catch (Exception ex) {
#if !DEBUG
                        session.m_scheduledDisconnect = true;
#endif
                        throw new Exception(ex.Message);
                    }
                }
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

        public static object HandleLobbyCharacterInfoReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_LOBBY_CHARACTER_INFO_REQ>();
            var response = new SS2C_LOBBY_CHARACTER_INFO_RES();
            return response;
        }

        public static object HandleCharacterSelectReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_CHARACTER_SELECT_ENTER_REQ>();
            var response = new SS2C_CHARACTER_SELECT_ENTER_RES();
            return response;
        }

        public static MemoryStream HandleCharacterSelectRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_CHARACTER_SELECT_ENTER_RES)inputClass;

            response.Result = 1;

            var serial = new WrapperSerializer<SS2C_CHARACTER_SELECT_ENTER_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CCharacterSelectEnterRes);
            return serial.Serialize();
        }

        public static MemoryStream HandleClassEquipInfoTrigger(ClientSession session)
        {
            return EquipProcessors.HandleClassEquipInfoRes(session, new SS2C_CLASS_EQUIP_INFO_RES());
        }

        public static MemoryStream HandleLobbyCharacterInfoTrigger(ClientSession session)
        {
            return HandleLobbyCharacterInfoRes(session, new SS2C_LOBBY_CHARACTER_INFO_RES());
        }

        public static MemoryStream HandleLobbyAccountCurrencyListNotTrigger(ClientSession session)
        {
            return HandleLobbyAccountCurrencyListNot(session, new SS2C_LOBBY_ACCOUNT_CURRENCY_LIST_NOT());
        }

        public static MemoryStream HandleLobbyAccountCurrencyListNot(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_LOBBY_ACCOUNT_CURRENCY_LIST_NOT)inputClass;


            var serial = new WrapperSerializer<SS2C_LOBBY_ACCOUNT_CURRENCY_LIST_NOT>(response, session.m_currentPacketSequence++, PacketCommand.S2CLobbyAccountCurrencyListNot);
            return serial.Serialize();
        }


        public static MemoryStream HandleLobbyCharacterInfoRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_LOBBY_CHARACTER_INFO_RES)inputClass;

            response.Result = (uint)LoginResponseResult.SUCCESS;
            response.CharacterDataBase = new SCHARACTER_INFO()
            {
                Level = (uint)session.m_currentCharacter.Level,
                AccountId = session.m_currentPlayer.AccountID.ToString(),
                CharacterClass = session.m_currentCharacter.Class,
                CharacterId = session.m_currentCharacter.CharID.ToString(),
                Gender = (uint)session.m_currentCharacter.Gender,
                NickName = new SACCOUNT_NICKNAME()
                {
                    KarmaRating = session.m_currentCharacter.KarmaScore,
                    StreamingModeNickName = session.m_currentCharacter.Nickname,
                    OriginalNickName = session.m_currentCharacter.Nickname,
                }
            };

            try
            {
                var allItems = InventoryHelpers.GetAllUserItems(session.GetDB(), session.m_currentCharacter.CharID, false);
                foreach (var item in allItems)
                {
                    var clientItem = InventoryHelpers.MakeSItemObject(item.Key, true, session.GetDB());

                    switch ((uint)item.Key.InventoryID)
                    {
                        case (uint)InventoryType.INVENTORY_CHARACTER:
                            response.CharacterDataBase.CharacterItemList.Add(clientItem);
                            break;
                        case (uint)InventoryType.INVENTORY_STASH:
                            response.CharacterDataBase.CharacterStorageItemList.Add(clientItem);
                            break;
                    }
                }
            } catch (Exception ex) {
#if !DEBUG
                session.m_scheduledDisconnect = true;
#endif
                throw new Exception(ex.Message);
            }

            Console.WriteLine(response.ToString());

            var serial = new WrapperSerializer<SS2C_LOBBY_CHARACTER_INFO_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CLobbyCharacterInfoRes);
            return serial.Serialize();
        }
    }
}
