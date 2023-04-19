using System.Net.Sockets;
using System.Net;
using System.Text;

namespace BarkAndBarker.Proxy
{
    internal class LobbyProxy
    {
        private readonly string localAddress;
        private readonly int localPort;
        private readonly string remoteAddress;
        private readonly int remotePort;

        public LobbyProxy(string localAddress, int localPort, string remoteAddress, int remotePort)
        {
            this.localAddress = localAddress;
            this.localPort = localPort;
            this.remoteAddress = remoteAddress;
            this.remotePort = remotePort;
        }

        public void Start()
        {
            var listener = new TcpListener(IPAddress.Parse(localAddress), localPort);
            listener.Start();

            Console.WriteLine($"Listening on {localAddress}:{localPort}");
            while (true)
            {
                var client = listener.AcceptTcpClient();
                var clientThread = new Thread(() => HandleClient(client));
                clientThread.Start();
            }
        }

        private void HandleClient(TcpClient client)
        {
            using (client)
            {
                try
                {
                    using var remoteClient = new TcpClient();
                    remoteClient.Connect(remoteAddress, remotePort);

                    var clientToRemoteThread =
                        new Thread(() => ForwardTraffic(client.GetStream(), remoteClient.GetStream()));
                    var remoteToClientThread =
                        new Thread(() => ForwardTraffic(remoteClient.GetStream(), client.GetStream()));

                    clientToRemoteThread.Start();
                    remoteToClientThread.Start();

                    clientToRemoteThread.Join();
                    remoteToClientThread.Join();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error handling client: {ex.Message}");
                }
            }
        }

        private void ForwardTraffic(NetworkStream inputStream, NetworkStream outputStream)
        {
            var buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string packetData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Packet data: {packetData}");

                outputStream.Write(buffer, 0, bytesRead);
            }
        }
    }
}
