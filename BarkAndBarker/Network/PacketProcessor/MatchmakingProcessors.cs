using DC.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Network.PacketProcessor
{
    internal class MatchmakingProcessors
    {
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

            var serial = new WrapperSerializer<SS2C_AUTO_MATCH_REG_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CAutoMatchRegRes);
            return serial.Serialize();
        }
    }
}
