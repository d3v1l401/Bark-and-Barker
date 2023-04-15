using BarkAndBarker.Session;
using BarkAndBarker.Network;
using DC.Packet;
using NetCoreServer;
using System.Net;
using System.Net.Sockets;
using BarkAndBarker.Persistence;
using System.Collections.Concurrent;
using BarkAndBarker.Persistence.Models;

namespace BarkAndBarker
{
    internal static class CSSocketExtensions
    {
        public static void Dump(this byte[] buffer)
        {
            foreach (var b in buffer)
                Console.Write(b.ToString("X2") + " ");
        }

        public static T[] Span<T>(this T[] buffer, int offset, long length)
        {
            T[] result = new T[length];
            Array.Copy(buffer, offset, result, 0, length);
            return result;
        }
    }
    public class ClientSession : NetCoreServer.TcpSession
    {
        private static PacketManager m_packetManager = new PacketManager();
        public PlayerInfo m_currentPlayer = new PlayerInfo();
        public ModelCharacter m_currentCharacter = null;
        private Database m_databaseSession = new Database();

        public UInt16 m_currentPacketSequence { get; set; } = 0;

        public Database GetDB() { return m_databaseSession; }

        public ClientSession(TcpServer server) : base(server) 
        {
            Console.WriteLine($"Client connection hit.");
        }

        protected override void OnConnected()
        {
            Console.WriteLine($"Client {Id} connected.");
        }

        protected override void OnDisconnected()
        {
            Console.WriteLine($"Client {Id} disconnected.");
        }

        protected override void OnSent(long sent, long pending)
        {
            
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            var inputStream = new MemoryStream(buffer, 0, (int)size, false, true);
            inputStream.Seek(0, SeekOrigin.Begin);

            try
            {
                SendAsync(m_packetManager.Handle(this, inputStream).ToArray());
            } catch (Exception ex) {
                Console.WriteLine("[" + base.Id + "] Could not process packet: " + ex.Message);
#if !DEBUG
                this.Disconnect();
#endif
                return;
            }

            // Multicast message to all connected sessions
            //Server.Multicast(message);
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"[ClientSession] Client caught an error with code {error}");
        }
    }

    public class ClientManager : NetCoreServer.TcpServer
    {
        public ConcurrentDictionary<Guid, TcpSession> GetSessions()
            => base.Sessions;

        public ClientManager(IPAddress address, int port) : base(address, port) 
        {
            
        }

        protected override TcpSession CreateSession() {
            return new ClientSession(this);
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"[ClientManager] Client caught an error with code {error}");
        }
    }
}
