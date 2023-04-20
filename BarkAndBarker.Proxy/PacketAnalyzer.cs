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

        private static Dictionary<PacketCommand, MessageDescriptor> packetMapping = new Dictionary<PacketCommand, MessageDescriptor>();

        private static string GetRelativePacketClass(PacketCommand command)
        {
            var finalClassName = "";
            var directionalPrefix = "SC2S";

            var packetCommandName = Enum.GetName(command);
            if (packetCommandName.StartsWith("C2S"))
                directionalPrefix = "SC2S";
            else if (packetCommandName.StartsWith("S2C"))
                directionalPrefix = "SS2C";
            else
            {
                Console.WriteLine("Unknown PacketCommand prefix '" + packetCommandName + "'");
                return null;
            }

            finalClassName += directionalPrefix;

            packetCommandName = packetCommandName.Substring(3);
            foreach (var character in packetCommandName)
            {
                if (char.IsUpper(character))
                {
                    finalClassName += "_";
                    finalClassName += char.ToUpper(character);
                } else
                    finalClassName += char.ToUpper(character);
            }

            return finalClassName;
        }

        private static MessageDescriptor GetMessageDescriptor(string className)
        {
            // Get all assemblies in the application
            var appDomainAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            // Select all packets find the one we want
            var message = typeof(IMessage).GetImplementors(appDomainAssemblies).Where(x => x.Name == className).FirstOrDefault();

            if (message == null)
            {
                Console.WriteLine("Unable to find '" + className + "' descriptor.");
                return null;
            }

            // Get the descriptor class
            var descriptor = message.GetProperty("Descriptor", BindingFlags.Public | BindingFlags.Static).GetValue(null, null) as MessageDescriptor;
            if (descriptor == null)
            {
                Console.WriteLine("Unable to find '" + className + "' descriptor property");
                return null;
            }

            return descriptor;
        }

        private static void BuildPacketCommandHandlers()
        {
            if (packetMapping.Count <= 0)
            {
                var packetCommandList = typeof(PacketCommand).GetFields(BindingFlags.Public | BindingFlags.Static).ToList();
                foreach (var command in packetCommandList)
                {
                    if (command.Name.StartsWith("Min") || command.Name.StartsWith("Max") || command.Name == "PacketNone")
                        continue;

                    var enumVar = (PacketCommand)command.GetRawConstantValue();

                    var className = GetRelativePacketClass(enumVar);
                    if (className == null)
                        continue;

                    var descriptor = GetMessageDescriptor(className);
                    if (descriptor == null)
                        continue;

                    packetMapping.Add(enumVar, descriptor);
                }
            }
        }

        public PacketAnalyzer(Logger rawLogger, Logger analyzedLogger)
        {
            internalBuffer = new Queue<byte>();

            this.rawLogger = rawLogger;
            this.analyzedLogger = analyzedLogger;

            BuildPacketCommandHandlers();
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
            if (packetMapping.TryGetValue(command, out descrParser))
            {
                var decodedPacket = descrParser.Parser.ParseFrom(deserializer.GetPayloadBuffer());
                analyzedLogger.Log(decodedPacket.ToString());
            } else
                Console.WriteLine($"Unhandeled packet command {command.ToString()}");
        }

    }
}
