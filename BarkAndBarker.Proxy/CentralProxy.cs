using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace BarkAndBarker.Proxy
{
    internal class CentralProxy
    {
        private readonly string localAddress;
        private readonly int localPort;
        private readonly string remoteAddress;
        private readonly int remotePort;

        public CentralProxy(string localAddress, int localPort, string remoteAddress, int remotePort)
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

                JObject json = null;

                try
                {
                    json = JObject.Parse(packetData);
                }
                catch (JsonReaderException ex)
                {
                    Console.WriteLine($"Error parsing JSON: {ex.Message}");
                }

                byte[] writeBuffer = buffer;

                if (json != null)
                {
                    if (json.ContainsKey("ipAddress") && json.ContainsKey("port"))
                    {
                        string ipAddress = json["ipAddress"].ToString();
                        int port = json["port"].ToObject<int>();

                        Console.WriteLine($"Extracted IP address: {ipAddress}");
                        Console.WriteLine($"Extracted port: {port}");

                        var t = new Thread(() =>
                        {
                            var lobbyProxy = new LobbyProxy("127.0.0.1", port, ipAddress, port);
                            lobbyProxy.Start();
                        });
                        t.Start();

                        Thread.Sleep(50);
                        
                        json["ipAddress"] = "127.0.0.1";
                        string modifiedPacketData = json.ToString(Formatting.None);
                        Console.WriteLine($"Modified packet data: {modifiedPacketData}");

                        int paddingLength = bytesRead - modifiedPacketData.Length;
                        modifiedPacketData = modifiedPacketData.PadRight(modifiedPacketData.Length + paddingLength);

                        writeBuffer = Encoding.UTF8.GetBytes(modifiedPacketData);
                        bytesRead = writeBuffer.Length;
                    }
                }

                outputStream.Write(writeBuffer, 0, bytesRead);
            }
        }
    }
}