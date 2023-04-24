using Azure;
using BarkAndBarker.Persistence.Models;
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
            var merchants = session.GetDB().Select<ModelMerchants>(ModelMerchants.QueryMerchantList, null);

            // Merchants list TODO: Keep their Inventory on Redist? Calculate RemainTime using UnixTimeStamps?
            // NOTE: Most merchant items must be random for each player.

            foreach (var merchant in merchants)
            {
                response.MerchantList.Add(new SMERCHANT_INFO()
                {
                    MerchantId = merchant.MerchantID,
                    Faction = merchant.Faction,
                    RemainTime = merchant.RemainTime,
                    IsUnidentified = merchant.isUnidentified,

                });

            }

            var serial = new WrapperSerializer<SS2C_MERCHANT_LIST_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CMerchantListRes);
            return serial.Serialize();

        }

        public static object HandleMerchantStockBuyItemListReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_MERCHANT_STOCK_BUY_ITEM_LIST_REQ>();
            return new SS2C_MERCHANT_STOCK_BUY_ITEM_LIST_RES();


        }

        public static MemoryStream HandleMerchantStockBuyItemListRes(ClientSession session, dynamic inputClass)
        {

            var response = (SS2C_MERCHANT_STOCK_BUY_ITEM_LIST_RES)inputClass;

            var serial = new WrapperSerializer<SS2C_MERCHANT_STOCK_BUY_ITEM_LIST_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CMerchantStockBuyItemListRes);

            return serial.Serialize();
        }
    }
}