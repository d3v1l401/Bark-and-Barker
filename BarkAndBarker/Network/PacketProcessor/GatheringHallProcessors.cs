using DC.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Network.PacketProcessor
{
    internal class GatheringHallProcessors
    {
        public static object HandleGatheringHallListReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_GATHERING_HALL_CHANNEL_LIST_REQ>();

            // TODO
            var response = new SS2C_GATHERING_HALL_CHANNEL_LIST_RES();
            response.Channels.Add(new SGATHERING_HALL_CHANNEL()
            {
                ChannelId = Guid.NewGuid().ToString(),
                ChannelIndex = 1,
                GroupIndex = 1,
                MemberCount = 666,
            });

            response.Channels.Add(new SGATHERING_HALL_CHANNEL()
            {
                ChannelId = Guid.NewGuid().ToString(),
                ChannelIndex = 2,
                GroupIndex = 2,
                MemberCount = 1337,
            });

            response.Channels.Add(new SGATHERING_HALL_CHANNEL()
            {
                ChannelId = Guid.NewGuid().ToString(),
                ChannelIndex = 0,
                GroupIndex = 2,
                MemberCount = 401,
            });

            return response;
        }

        public static MemoryStream HandleGatheringHallListRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_GATHERING_HALL_CHANNEL_LIST_RES)inputClass;
            var serial = new WrapperSerializer<SS2C_GATHERING_HALL_CHANNEL_LIST_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CGatheringHallChannelListRes);
            return serial.Serialize();
        }
    }
}
