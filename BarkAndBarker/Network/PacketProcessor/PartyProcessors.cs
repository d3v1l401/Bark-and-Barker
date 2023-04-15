using DC.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Network.PacketProcessor
{
    internal class PartyProcessors
    {
        public static object HandlePartyInviteReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_PARTY_INVITE_REQ>();

            Console.WriteLine(request.ToString());

            return new SS2C_PARTY_INVITE_RES();
        }

        public static MemoryStream HandlePartyInviteRes(ClientSession session, dynamic inputClass)
        {
            var res = new SS2C_PARTY_INVITE_RES();
            var response = new WrapperSerializer<SS2C_PARTY_INVITE_RES>(res, session.m_currentPacketSequence++, PacketCommand.S2CPartyInviteRes);
            return response.Serialize();
        }

        public static object HandlePartyExitReq(ClientSession session, dynamic deserializer)
        {
            var des = (WrapperDeserializer)deserializer;
            return des.Parse<SC2S_ALIVE_REQ>();
        }

        public static MemoryStream HandlePartyExitRes(ClientSession session, dynamic inputClass)
        {
            var res = new SS2C_ALIVE_RES();
            var response = new WrapperSerializer<SS2C_ALIVE_RES>(res, session.m_currentPacketSequence++, PacketCommand.S2CAliveRes);
            return response.Serialize();
        }

        public static object HandlePartyInviteAnswerReq(ClientSession session, dynamic deserializer)
        {
            var des = (WrapperDeserializer)deserializer;
            return des.Parse<SC2S_ALIVE_REQ>();
        }

        public static MemoryStream HandlePartyInviteAnswerRes(ClientSession session, dynamic inputClass)
        {
            var res = new SS2C_ALIVE_RES();
            var response = new WrapperSerializer<SS2C_ALIVE_RES>(res, session.m_currentPacketSequence++, PacketCommand.S2CAliveRes);
            return response.Serialize();
        }
        public static object HandlePartyMemberKickReq(ClientSession session, dynamic deserializer)
        {
            var des = (WrapperDeserializer)deserializer;
            return des.Parse<SC2S_ALIVE_REQ>();
        }

        public static MemoryStream HandlePartyMemberKickRes(ClientSession session, dynamic inputClass)
        {
            var res = new SS2C_ALIVE_RES();
            var response = new WrapperSerializer<SS2C_ALIVE_RES>(res, session.m_currentPacketSequence++, PacketCommand.S2CAliveRes);
            return response.Serialize();
        }
        public static object HandlePartyStartActionReq(ClientSession session, dynamic deserializer)
        {
            var des = (WrapperDeserializer)deserializer;
            return des.Parse<SC2S_ALIVE_REQ>();
        }

        public static MemoryStream HandlePartyStartActionRes(ClientSession session, dynamic inputClass)
        {
            var res = new SS2C_ALIVE_RES();
            var response = new WrapperSerializer<SS2C_ALIVE_RES>(res, session.m_currentPacketSequence++, PacketCommand.S2CAliveRes);
            return response.Serialize();
        }
        public static object HandlePartyReadyReq(ClientSession session, dynamic deserializer)
        {
            var des = (WrapperDeserializer)deserializer;
            return des.Parse<SC2S_ALIVE_REQ>();
        }

        public static MemoryStream HandlePartyReadyRes(ClientSession session, dynamic inputClass)
        {
            var res = new SS2C_ALIVE_RES();
            var response = new WrapperSerializer<SS2C_ALIVE_RES>(res, session.m_currentPacketSequence++, PacketCommand.S2CAliveRes);
            return response.Serialize();
        }

        public static object HandlePartyChatReq(ClientSession session, dynamic deserializer)
        {
            var des = (WrapperDeserializer)deserializer;
            return des.Parse<SC2S_ALIVE_REQ>();
        }

        public static MemoryStream HandlePartyChatRes(ClientSession session, dynamic inputClass)
        {
            var res = new SS2C_ALIVE_RES();
            var response = new WrapperSerializer<SS2C_ALIVE_RES>(res, session.m_currentPacketSequence++, PacketCommand.S2CAliveRes);
            return response.Serialize();
        }
    }
}
