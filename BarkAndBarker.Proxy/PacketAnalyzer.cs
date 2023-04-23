using BarkAndBarker.Network;
using DC.Packet;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Bcpg;
using System.Net.Sockets;
using System.Reflection;

namespace BarkAndBarker.Proxy
{
    internal class PacketAnalyzer
    {
        private Logger rawLogger;
        private Logger analyzedLogger;

        private Queue<byte> internalBuffer;


        public PacketAnalyzer(Logger rawLogger, Logger analyzedLogger)
        {
            internalBuffer = new Queue<byte>();

            this.rawLogger = rawLogger;
            this.analyzedLogger = analyzedLogger;

            PacketHelpers.BuildPacketCommandHandlers();
        }

        public void Analyze(byte[] buffer, string rawStringified)
        {
            rawLogger.Log(rawStringified);

            foreach (var b in buffer)
            {

                internalBuffer.Enqueue(b);
            }

            try
            {
                var packetLengthBytes = internalBuffer.Take(4).ToArray();
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(packetLengthBytes); // Reverse the byte order if running on a little-endian system
                }
                var packetLength = System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt32(packetLengthBytes, 0));
                
                if (internalBuffer.Count >= packetLength)
                {
                    var currPacketBuffer = new byte[packetLength];

                    for (int i = 0; i < packetLength; i++)
                    {
                        currPacketBuffer[i] = internalBuffer.Dequeue();
                    }

                    var memoryStream = new MemoryStream();
                    memoryStream.Write(currPacketBuffer, 0, currPacketBuffer.Length);

                    var deser = new WrapperDeserializer(memoryStream);

                    var packetType = deser.GetPacketClass();
                    
                    analyzedLogger.Log(packetType.ToString());

                    HandleCommand(deser, packetType);
                }
            }
            catch (Exception ex)
            {
                rawLogger.Log(ex.Message);
                analyzedLogger.Log("UNRECOGNIZED!" + rawStringified);
            }
        }

        private void HandleCommand(WrapperDeserializer deserializer, PacketCommand command)
        {
            MessageDescriptor descrParser = null;
            if (PacketHelpers.PacketMapping.TryGetValue(command, out descrParser))
            {
                var decodedPacket = descrParser.Parser.ParseFrom(deserializer.GetPayloadBuffer());
                analyzedLogger.Log(decodedPacket.ToString());
            } else
                Console.WriteLine($"Unhandeled packet command {command.ToString()}");
        }

    }
}
