using DC.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Network.PacketProcessor
{
    internal class MiscProcessors
    {
        public static object HandleAliveReq(ClientSession session, dynamic deserializer)
        {
            var des = (WrapperDeserializer)deserializer;
            return des.Parse<SC2S_ALIVE_REQ>();
        }

        public static MemoryStream HandleAliveRes(ClientSession session, dynamic inputClass)
        {
            var res = new SS2C_ALIVE_RES();
            var response = new WrapperSerializer<SS2C_ALIVE_RES>(res, session.m_currentPacketSequence++, PacketCommand.S2CAliveRes);
            return response.Serialize();
        }
    }
}
