using DC.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarkAndBarker.GatheringHall;
using Google.Protobuf.Collections;

namespace BarkAndBarker.Network.PacketProcessor
{
    internal class GatheringHallProcessors
    {
        public static object HandleGatheringHallListReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_GATHERING_HALL_CHANNEL_LIST_REQ>();
            
            var response = new SS2C_GATHERING_HALL_CHANNEL_LIST_RES();

            foreach (var gatheringHall in GatheringHallManager.GatheringHalls)
            {
                response.Channels.Add(new SGATHERING_HALL_CHANNEL()
                {
                    ChannelId = gatheringHall.ChannelId,
                    ChannelIndex = gatheringHall.ChannelIndex,
                    GroupIndex = gatheringHall.GroupIndex,
                    MemberCount = gatheringHall.MemberCount,
                });
            }

            return response;
        }

        public static MemoryStream HandleGatheringHallListRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_GATHERING_HALL_CHANNEL_LIST_RES)inputClass;
            var serial = new WrapperSerializer<SS2C_GATHERING_HALL_CHANNEL_LIST_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CGatheringHallChannelListRes);
            return serial.Serialize();
        }

        public static object HandleGatheringHallChannelSelectReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_GATHERING_HALL_CHANNEL_SELECT_REQ>();

            var joinResult = GatheringHallManager.Join(session, request.ChannelIndex);

            var response = new SS2C_GATHERING_HALL_CHANNEL_SELECT_RES
            {
                Result = joinResult ? (uint)1 : 0
            };

            return response;
        }

        public static MemoryStream HandleGatheringHallChannelSelectRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_GATHERING_HALL_CHANNEL_SELECT_RES)inputClass;
            
            var serial = new WrapperSerializer<SS2C_GATHERING_HALL_CHANNEL_SELECT_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CGatheringHallChannelSelectRes);
            return serial.Serialize();
        }

        public static object HandleGatheringHallChannelExitReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_GATHERING_HALL_CHANNEL_EXIT_REQ>();

            var leaveResult = GatheringHallManager.Leave(session);

            var response = new SS2C_GATHERING_HALL_CHANNEL_EXIT_RES()
            {
                Result = leaveResult ? (uint)1 : 0
            };

            return response;
        }

        public static MemoryStream HandleGatheringHallChannelExitRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_GATHERING_HALL_CHANNEL_EXIT_RES)inputClass;
            
            var serial = new WrapperSerializer<SS2C_GATHERING_HALL_CHANNEL_EXIT_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CGatheringHallChannelExitRes);
            return serial.Serialize();
        }

        public static object HandleGatheringHallChannelChatReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_GATHERING_HALL_CHANNEL_CHAT_REQ>();

            var message = new ChatMessage(request.Chat.ChatData.ChatDataPieceArray[0].ChatStr, ChatType.Normal, session);
            GatheringHallManager.AddMessage(message, session);

            var response = new SS2C_GATHERING_HALL_CHANNEL_CHAT_RES();

            return response;
        }

        public static MemoryStream HandleGatheringHallChannelChatRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_GATHERING_HALL_CHANNEL_CHAT_RES)inputClass;
            
            response.Result = 1;

            var serial = new WrapperSerializer<SS2C_GATHERING_HALL_CHANNEL_CHAT_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CGatheringHallChannelSelectRes);
            return serial.Serialize();
        }

        public static MemoryStream HandleChannelUserListTrigger(ClientSession session)
        {
            var response = new SS2C_GATHERING_HALL_CHANNEL_USER_LIST_RES();

            var userList = GatheringHallManager.GetUserList(session);
            foreach (var user in userList)
            {
                var character = new SCHARACTER_GATHERING_HALL_INFO();
                character.AccountId = user.m_currentPlayer.AccountID.ToString();
                character.NickName = new SACCOUNT_NICKNAME()
                {
                    OriginalNickName = user.m_currentCharacter.Nickname,
                    StreamingModeNickName = user.m_currentCharacter.Nickname,
                    KarmaRating = user.m_currentCharacter.KarmaScore
                };
                character.CharacterClass = user.m_currentCharacter.Class;
                character.CharacterId = user.m_currentCharacter.CharID;
                character.Gender = (uint)user.m_currentCharacter.Gender;
                character.Level = (uint)user.m_currentCharacter.Level;
                response.Characters.Add(character);
            }

            var serial = new WrapperSerializer<SS2C_GATHERING_HALL_CHANNEL_USER_LIST_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CGatheringHallChannelUserListRes);
            return serial.Serialize();
        }
    }
}
