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
                        new Thread(() => ForwardTraffic(client.GetStream(), remoteClient.GetStream(), Direction.C2S));
                    var remoteToClientThread =
                        new Thread(() => ForwardTraffic(remoteClient.GetStream(), client.GetStream(), Direction.S2C));

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

        private void ForwardTraffic(NetworkStream inputStream, NetworkStream outputStream, Direction direction)
        {
            var buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                StringBuilder sb = new StringBuilder();

                switch (direction)
                {
                    case Direction.C2S:
                        sb.Append("C2S\n");
                        break;
                    case Direction.S2C:
                        sb.Append("S2C\n");
                        break;
                }

                for (int i = 0; i < bytesRead; i++)
                {
                    sb.AppendFormat("0x{0:X2} ", buffer[i]);
                }

                Console.WriteLine(sb.ToString());

                outputStream.Write(buffer, 0, bytesRead);
            }
        }
    }

    enum Direction
    {
        C2S,
        S2C
    }
}