using DC.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Network.PacketProcessor
{
    internal class ProgressionProcessors
    {
        public static object HandleClassLevelInfoReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_CLASS_LEVEL_INFO_REQ>();
            return new SS2C_CLASS_LEVEL_INFO_RES();
        }

        public static MemoryStream HandleClassLevelInfoRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_CLASS_LEVEL_INFO_RES)inputClass;

            // TODO: Leveling handling
            response.Level = (uint)session.m_currentCharacter.Level;
            response.Exp = 8000;
            response.ExpBegin = 1;
            response.ExpLimit = 10000;
            response.RewardPoint = 5;

            var serial = new WrapperSerializer<SS2C_CLASS_LEVEL_INFO_RES>(response, PacketCommand.S2CClassLevelInfoRes);
            return serial.Serialize();
        }
    }
}
