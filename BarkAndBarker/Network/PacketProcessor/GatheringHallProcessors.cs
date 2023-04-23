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

            // TODO
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

            var channelIndex = request.ChannelIndex;

            var response = new SS2C_GATHERING_HALL_CHANNEL_SELECT_RES();

            return response;
        }

        public static MemoryStream HandleGatheringHallChannelSelectRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_GATHERING_HALL_CHANNEL_SELECT_RES)inputClass;

            response.Result = 1;

            var serial = new WrapperSerializer<SS2C_GATHERING_HALL_CHANNEL_SELECT_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CGatheringHallChannelSelectRes);
            return serial.Serialize();
        }

        public static object HandleGatheringHallChannelExitReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_GATHERING_HALL_CHANNEL_EXIT_REQ>();
            
            var response = new SS2C_GATHERING_HALL_CHANNEL_EXIT_RES();

            return response;
        }

        public static MemoryStream HandleGatheringHallChannelExitRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_GATHERING_HALL_CHANNEL_EXIT_RES)inputClass;

            response.Result = 1;

            var serial = new WrapperSerializer<SS2C_GATHERING_HALL_CHANNEL_EXIT_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CGatheringHallChannelExitRes);
            return serial.Serialize();
        }

        public static object HandleGatheringHallChannelChatReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_GATHERING_HALL_CHANNEL_CHAT_REQ>();

            var chat = request.Chat;
            Console.WriteLine(request.ToString());

            var response = new SS2C_GATHERING_HALL_CHANNEL_CHAT_RES();

            var myChat = new SGATHERING_HALL_CHAT_S2C();
            myChat.ChatIndex = 1;
            myChat.Time = 1681949940468;
            myChat.ChatType = chat.ChatType;
            myChat.ChatData = chat.ChatData;

            response.Chats.Add(myChat);

            return response;
        }

        public static MemoryStream HandleGatheringHallChannelChatRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_GATHERING_HALL_CHANNEL_CHAT_RES)inputClass;

            //var c = new SGATHERING_HALL_CHAT_S2C();
            //response.Chats.Add(c);

            var chatNotC = new SS2C_GATHERING_HALL_CHANNEL_CHAT_NOT();
            chatNotC.Chats.Add(response.Chats[0]);

            var chatNot = new WrapperSerializer<SS2C_GATHERING_HALL_CHANNEL_CHAT_NOT>(chatNotC,
                session.m_currentPacketSequence++, PacketCommand.S2CGatheringHallChannelChatNot);

            session.SendAsync(chatNot.Serialize().ToArray());


            response.Result = 1;

            var serial = new WrapperSerializer<SS2C_GATHERING_HALL_CHANNEL_CHAT_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CGatheringHallChannelSelectRes);
            return serial.Serialize();
        }
    }
}
