using BarkAndBarker.Shared.Persistence.Models;
using DC.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Network.PacketProcessor
{
    internal class EquipProcessors
    {
        public static object HandleClassEquipInfoReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_CLASS_EQUIP_INFO_REQ>();

            return new SS2C_CLASS_EQUIP_INFO_RES();
        }

        public static MemoryStream HandleClassEquipInfoRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_CLASS_EQUIP_INFO_RES)inputClass;

            var perksAndSkills = session.GetDB().Select<ModelPerks>(ModelPerks.QuerySelectCharacterSkills, new { CID = session.m_currentCharacter.CharID });
            foreach (var ability in perksAndSkills)
            {
                var slotIsAvailable = (uint)ability.IsAvailableSlot;
                if (session.m_currentCharacter.Level < ability.RequiredLevel)
                    slotIsAvailable = 0;

                response.Equips.Add(new SCLASS_EQUIP_INFO()
                {
                    Index = (uint)ability.Index,
                    EquipId = ability.EquipID != null ? ability.EquipID : "",
                    IsAvailableSlot = slotIsAvailable,
                    RequiredLevel = (uint)ability.RequiredLevel,
                    Type = (uint)ability.Type,
                });
            }

            var serial = new WrapperSerializer<SS2C_CLASS_EQUIP_INFO_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CClassEquipInfoRes);
            return serial.Serialize();
        }

        public static object HandleCustomizeItemInfoReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_CUSTOMIZE_ITEM_INFO_REQ>();
            return new SS2C_CUSTOMIZE_ITEM_INFO_RES();
        }

        public static MemoryStream HandleCustomizeItemInfoRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_CUSTOMIZE_ITEM_INFO_RES)inputClass;

            // TODO
            response.LoopFlag = 0;
            response.CustomizeItems.Add(new SCUSTOMIZE_ITEM()
            {

            });

            var serial = new WrapperSerializer<SS2C_CUSTOMIZE_ITEM_INFO_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CCustomizeItemInfoRes);
            return serial.Serialize();
        }
    }
}
