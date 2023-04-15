using DC.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Network.PacketProcessor
{
    internal class CustomizationProcessors
    {
        public static object HandleCustomizeCharacterInfoReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_CUSTOMIZE_CHARACTER_INFO_REQ>();
            return new SS2C_CUSTOMIZE_CHARACTER_INFO_RES();
        }

        public static MemoryStream HandleCustomizeCharacterInfoRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_CUSTOMIZE_CHARACTER_INFO_RES)inputClass;

            // TODO: Wtf is this
            response.LoopFlag = 0;
            response.CustomizeCharacters.Add(new SCUSTOMIZE_CHARACTER
            {
                CustomizeCharacterId = session.m_currentCharacter.CharID,
                IsEquip = 1
            });

            var serial = new WrapperSerializer<SS2C_CUSTOMIZE_CHARACTER_INFO_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CCustomizeCharacterInfoRes);
            return serial.Serialize();
        }

        public static object HandleCustomizeActionInfoReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_CUSTOMIZE_ACTION_INFO_REQ>();
            return new SS2C_CUSTOMIZE_ACTION_INFO_RES();
        }

        public static MemoryStream HandleCustomizeActionInfoRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_CUSTOMIZE_ACTION_INFO_RES)inputClass;

            response.LoopFlag = 0;
            response.CustomizeActionIds.Add(new SCUSTOMIZE_ACTION()
            {
                CustomizeActionId = "",
                IsEquip = 1,
                IsNew = 1,
            });

            var serial = new WrapperSerializer<SS2C_CUSTOMIZE_ACTION_INFO_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CCustomizeActionInfoRes);
            return serial.Serialize();
        }
    }
}
