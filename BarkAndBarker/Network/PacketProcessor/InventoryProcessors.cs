using Azure;
using Azure.Core;
using DC.Packet;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            Console.WriteLine(request.ToString());

            response.Result = 1;
            response.OldItem.Add(request.OldItem);
            response.NewItem.Add(request.NewItem);

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
