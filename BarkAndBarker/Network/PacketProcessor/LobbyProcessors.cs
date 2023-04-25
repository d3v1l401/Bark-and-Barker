using DC.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarkAndBarker.Shared.Persistence.Models;

namespace BarkAndBarker.Network.PacketProcessor
{
    internal class LobbyProcessors
    {
        public static object HandleLobbyEnterReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_LOBBY_ENTER_REQ>();
            var response = new SS2C_LOBBY_ENTER_RES();

            var selectedCharacter = session.GetDB().SelectFirst<ModelCharacter>(ModelCharacter.QuerySelectCharacterByID, new { CID = request.CharacterId });
            if (selectedCharacter != null)
            {
                var charOwnedAccount = session.GetDB().SelectFirst<ModelAccount>(ModelCharacter.QueryOwnerAccountForCharacterID, new { CID = request.CharacterId });
#if USE_STEAM
                if (charOwnedAccount.SteamID != null && charOwnedAccount.SteamID == session.m_currentPlayer.SteamID)
                {
                    session.m_currentCharacter = selectedCharacter;
                    response.Result = (uint)LoginResponseResult.SUCCESS;
                }
#else
                if (charOwnedAccount.ID != null && charOwnedAccount.ID == session.m_currentPlayer.AccountID)
                {
                    session.m_currentCharacter = selectedCharacter;
                    response.Result = (uint)LoginResponseResult.SUCCESS;
                }
#endif
            }
            else
                response.Result = (uint)LoginResponseResult.FAIL_PASSWORD;

            return response;
        }

        public static MemoryStream HandleLobbyEnterRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_LOBBY_ENTER_RES)inputClass;

            response.AccountId = session.m_currentPlayer.AccountID.ToString();
            response.Result = (uint)LoginResponseResult.SUCCESS;

            var serial = new WrapperSerializer<SS2C_LOBBY_ENTER_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CLobbyEnterRes);
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

            var serial = new WrapperSerializer<SS2C_OPEN_LOBBY_MAP_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2COpenLobbyMapRes);
            return serial.Serialize();
        }


        public static object HandleLobbyGameDifficultySelectReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_LOBBY_GAME_DIFFICULTY_SELECT_REQ>();
            var response = new SS2C_LOBBY_GAME_DIFFICULTY_SELECT_RES();

            // Switch Case / Enum? Might be over kill, depends on what's required in the future.
            // For now, result = 1 if the map is valid.

            if (request.GameDifficultyTypeIndex <= 4 && request.GameDifficultyTypeIndex != 0)
            {
                response.Result = 1;
            }
            else
            {
                response.Result = 0;
            }

            return response;
        }

        public static MemoryStream HandleLobbyGameDifficultySelectRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_LOBBY_GAME_DIFFICULTY_SELECT_RES)inputClass;

            var serial = new WrapperSerializer<SS2C_LOBBY_GAME_DIFFICULTY_SELECT_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CLobbyGameDifficultySelectRes);
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

           

            var serial = new WrapperSerializer<SS2C_META_LOCATION_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CMetaLocationRes);
            return serial.Serialize();
        }

    }
}
