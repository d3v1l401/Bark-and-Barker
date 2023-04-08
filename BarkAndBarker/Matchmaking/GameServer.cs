using NetCoreServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker
{
    // TODO: Change to UDP?
    class GameServer : UdpServer
    {
        public GameServer(IPAddress address, int port) : base(address, port) { }

        protected override void OnStarted()
        {
            Console.WriteLine("Game server started and listening on " + base.Address + ":" + base.Port);
            ReceiveAsync();
        }

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            Console.WriteLine("Incoming:");
            buffer.Span(0, size).Dump();
            Console.WriteLine("");

            // Echo the message back to the sender
            //SendAsync(endpoint, buffer, 0, size);
        }

        protected override void OnSent(EndPoint endpoint, long sent)
        {
            ReceiveAsync();
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"[GameServer] Caught an error with code {error}");
        }
    }
}
