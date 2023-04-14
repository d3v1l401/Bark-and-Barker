using BarkAndBarker.Persistence.Models;
using DC.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                if (charOwnedAccount.SteamID != null && charOwnedAccount.SteamID == session.m_currentPlayer.SteamID)
                {
                    session.m_currentCharacter = selectedCharacter;
                    response.Result = (uint)LoginResponseResult.SUCCESS;
                }
            }
            else
                response.Result = (uint)LoginResponseResult.FAIL_PASSWORD;

            return response;
        }

        public static MemoryStream HandleLobbyEnterRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_LOBBY_ENTER_RES)inputClass;
            response.AccountId = session.m_currentPlayer.SteamID;

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
