using Azure;
using DC.Packet;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Network
{
    public static class ReflectionHelpers
    {
        public static IEnumerable<Type> GetImplementors(this Type abstractType, params Assembly[] assembliesToSearch)
        {
            var typesInAssemblies = assembliesToSearch.SelectMany(assembly => assembly.GetTypes());
            return typesInAssemblies.Where(abstractType.IsAssignableFrom);
        }
    }
    public class WrapperDeserializer
    {
        // Used for dynamic parsing of client to server packets, will get populated every time the client sends a packet of which type descriptor needs to be resolved from current assembly
        private static Dictionary<Type, MessageDescriptor> CachedParsers;

        private static void getMessageDescriptors()
        {
            if (CachedParsers == null)
                CachedParsers = new Dictionary<Type, MessageDescriptor>();

            // Get all assemblies in the application
            var appDomainAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            // Select only client to server packets classes
            var messages = typeof(IMessage).GetImplementors(appDomainAssemblies).Where(x => x.Name.StartsWith("SC2S"));

            foreach (var message in messages)
            {
                var desc = message.GetProperty("Descriptor", BindingFlags.Public | BindingFlags.Static).GetValue(null, null) as MessageDescriptor;
                if (desc != null)
                    CachedParsers.Add(message, desc);
                else
                    Console.WriteLine("WARNING: " + message.Name + " does not contain the Descriptor member!");
            }
        }

        private bool m_headerParsed = false;

        private uint m_packetSize = 0; // 4 bytes
        private uint m_packetType = 0; // +2 bytes
        private uint m_packetUnk = 0;  // +2 bytes

        private BinaryReader m_stream;
        public WrapperDeserializer(MemoryStream inputStream)
        {
            if (inputStream == null)
                throw new ArgumentNullException();

            if (!inputStream.CanSeek && !inputStream.CanRead)
                throw new ArgumentException();

            if (inputStream.Length <= 0)
                throw new ArgumentException();

            inputStream.Seek(0, SeekOrigin.Begin);

            if (CachedParsers == null)
                getMessageDescriptors();

            this.m_stream = new BinaryReader(inputStream);
            this.parseHeader();
        }

        public void Skip(uint skipBytes)
        {
            var curPosition = this.m_stream.BaseStream.Position;
            var newPosition = (curPosition + skipBytes);

            if (newPosition > this.m_stream.BaseStream.Length)
                throw new Exception();

            this.m_stream.BaseStream.Seek(newPosition, SeekOrigin.Begin);
        }

        public ushort GetUShort()
            => this.m_stream.ReadUInt16();

        public uint GetUInt()
            => this.m_stream.ReadUInt32();

        public int GetInt()
            => this.m_stream.ReadInt32();

        public short GetShort()
            => this.m_stream.ReadInt16();

        public long GetLong()
            => this.m_stream.ReadInt64();

        public ulong GetULong()
            => this.m_stream.ReadUInt64();

        public byte GetByte()
            => this.m_stream.ReadByte();

        public PacketCommand GetPacketClass()
            => (PacketCommand)this.m_packetType;

        private void parseHeader()
        {
            this.m_packetSize = this.GetUInt();
            this.m_packetType = this.GetUShort();
            this.m_packetUnk = this.GetUShort();

            if ((this.m_stream.BaseStream.Length == this.m_packetSize) || (this.m_packetSize == 8)) // ALIVE_REQ does not use the wrapping header
            {
                this.m_headerParsed = true;
                return;
            } else
                Console.WriteLine("Not a valid packet size (" + this.m_packetSize + " != " + this.m_stream.BaseStream.Length + ")");

            throw new Exception("invalid packet");
        }

        public T Parse<T>() where T : class
        {

            if (!this.m_headerParsed)
                this.parseHeader();

            var protoBuffer = this.m_stream.ReadBytes((int)this.m_packetSize - 4 - 2 - 2);

            try
            {
                MessageDescriptor? parser = null;
                if (CachedParsers.TryGetValue(typeof(T), out parser) && parser != null)
                    return (T)CachedParsers[typeof(T)].Parser.ParseFrom(protoBuffer);
                else
                    throw new Exception("Unimplemented packet MessageDescriptor: " + typeof(T).Name);

            } catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }

    public class WrapperSerializer<T>
    {
        private T m_packet;
        private PacketCommand m_packetClass = PacketCommand.PacketNone;

        public WrapperSerializer(T packet, PacketCommand packetClass) 
        {
            if (packet == null)
                throw new ArgumentException();

            this.m_packet = packet;
            this.m_packetClass = packetClass;
        }

        public MemoryStream Serialize()
        {
            UInt32 headerSize = 4 + 2 + 2;

            using (var packet = new MemoryStream())
            {
                using (var packetPayload = new MemoryStream())
                {
                    if (this.m_packet == null) // Should never happen but we never know
                        throw new AccessViolationException();

                    (this.m_packet as IMessage).WriteTo(packetPayload);

                    using (var writer = new BinaryWriter(packet))
                    {
                        writer.Write((UInt32)(headerSize + packetPayload.Length));
                        writer.Write((UInt16)this.m_packetClass); 
                        writer.Write((UInt16)2);
                        writer.Write(packetPayload.ToArray());
                    }

                    return packet;
                }
            }
        }

    }
}
