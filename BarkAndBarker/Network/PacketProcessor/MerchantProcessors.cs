using DC.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Network.PacketProcessor
{
    internal class MerchantProcessors
    {
        public static object HandleMerchantListReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_MERCHANT_LIST_REQ>();
            return new SS2C_MERCHANT_LIST_RES();
        }

        public static MemoryStream HandleMerchantListRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_MERCHANT_LIST_RES)inputClass;

            // TODO: Merchants list, store these in a DB but their inventory on Redis?
            response.MerchantList.Add(new SMERCHANT_INFO()
            {
                Faction = 1,
                IsUnidentified = 0,
                MerchantId = Guid.NewGuid().ToString(),
                RemainTime = 0,
            });

            response.MerchantList.Add(new SMERCHANT_INFO()
            {
                Faction = 2,
                IsUnidentified = 1,
                MerchantId = Guid.NewGuid().ToString(),
                RemainTime = 1092831,
            });

            var serial = new WrapperSerializer<SS2C_MERCHANT_LIST_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CAutoMatchRegRes);
            return serial.Serialize();
        }
    }
}
