using DC.Packet;
using Google.Protobuf.Reflection;
using Google.Protobuf;
using System.Reflection;
using BarkAndBarker.Network;

namespace BarkAndBarker.Proxy
{
    internal static class PacketHelpers
    {
        public static Dictionary<PacketCommand, MessageDescriptor> PacketMapping = new Dictionary<PacketCommand, MessageDescriptor>();

        public static string GetRelativePacketClass(PacketCommand command)
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
                }
                else
                    finalClassName += char.ToUpper(character);
            }

            return finalClassName;
        }

        public static MessageDescriptor GetMessageDescriptor(string className)
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

        public static void BuildPacketCommandHandlers()
        {
            if (PacketMapping.Count <= 0)
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

                    PacketMapping.Add(enumVar, descriptor);
                }
            }
        }
    }
}
