using Azure;
using Azure.Core;
using BarkAndBarker.Shared.Persistence.Models;
using DC.Packet;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BarkAndBarker.Network.PacketProcessor
{
    internal class InventoryProcessors
    {

        public static object HandleInventoryMoveReq(ClientSession session , dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_INVENTORY_MOVE_REQ>();

            Console.WriteLine(request.ToString());

            return new SS2C_INVENTORY_MOVE_RES();

        }

        public static MemoryStream HandleInventoryMoveRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_INVENTORY_MOVE_RES)inputClass;

            var serial = new WrapperSerializer<SS2C_INVENTORY_MOVE_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CInventoryMoveRes);
            return serial.Serialize();

        }

        public static object HandleInventorySingleUpdateReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_INVENTORY_SINGLE_UPDATE_REQ>();

            var response = new SS2C_INVENTORY_SINGLE_UPDATE_RES();

            // Checks should be inserted here I believe.
            // Database should also be updated if the result = 1 (success).

            Console.WriteLine(request.ToString());

            if (request.NewItem.Count > 1) {
                response.Result = 0;
                return response;
            }

            

            var newItem = request.NewItem[0];
            var oldItem = request.OldItem[0];


            if (newItem.ItemUniqueId != oldItem.ItemUniqueId && newItem.ItemId == "DesignDataItem:Id_Item_GoldCoins" && (newItem.ItemCount - oldItem.ItemCount == 0))
            {

                var queryGoldCount = session.GetDB().Select<ModelInventoryItem>(ModelInventoryItem.QueryInventoryGold, new { UniqueID = oldItem.ItemUniqueId });
                int goldcount = 0;

                foreach (var item in queryGoldCount)
                {
                   goldcount = item.ItemCount;
                }

                if (goldcount - newItem.ItemCount <= 0)
                {
                    Console.WriteLine("Gold has depleted and will be deleting.");

                    var queryResGold = session.GetDB().Execute(ModelInventoryItem.QueryDeleteInventoryGold, new
                    {
                        UniqueID = oldItem.ItemUniqueId,
                        //ItemCount = -1

                    });

                    var queryUpdateGold = session.GetDB().Execute(ModelInventoryItem.QueryUpdateInventoryGold, new
                    {
                        UniqueID = oldItem.ItemUniqueId,
                        ItemCount = newItem.ItemCount + goldcount,
                        SlotID = newItem.SlotId
                    }); 
                }
                else
                {
                    Console.WriteLine("Taking a gold out of stack.");

                    var queryInsertGold = session.GetDB().Execute(ModelInventoryItem.QueryInsertInventoryGold, new
                    {
                        OwnerID = session.m_currentCharacter.CharID,
                        UniqueID = newItem.ItemUniqueId,
                        ItemCount = newItem.ItemCount,
                        ItemID = newItem.ItemId,
                        InventoryID = newItem.InventoryId,
                        SlotID = newItem.SlotId

                    });

                    var queryResGold = session.GetDB().Execute(ModelInventoryItem.QueryUpdateInventoryGold, new
                    {
                        UniqueID = oldItem.ItemUniqueId,
                        ItemCount = goldcount - newItem.ItemCount,
                        SlotID = oldItem.SlotId

                    });;

                 

                }


                
            }

            //var GoldCount = newItem.ItemCount + oldItem.ItemCount;


            var queryRes = session.GetDB().Execute(ModelInventoryItem.QueryUpdateInventoryGold, new
            {
                UniqueID = newItem.ItemUniqueId,
                ItemCount = newItem.ItemCount,
                SlotID = newItem.SlotId

            });


            Console.WriteLine(queryRes.ToString());


            if (queryRes > 0)
            {
                response.Result = 1;
                response.OldItem.Add(request.OldItem);
                response.NewItem.Add(request.NewItem);
            }
            else
                response.Result = 0;

            return response;
        }

        public static MemoryStream HandleInventorySingleUpdateRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_INVENTORY_SINGLE_UPDATE_RES)inputClass;

            Console.WriteLine(response.ToString());

            var serial = new WrapperSerializer<SS2C_INVENTORY_SINGLE_UPDATE_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CInventorySingleUpdateRes);
            return serial.Serialize();

        }

    }
}
