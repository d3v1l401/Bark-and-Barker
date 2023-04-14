using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace BarkAndBarker
{
    internal static class CSWebExtensions
    {
        public static byte[] ToByteArray(this string s)
        {
            List<byte> buffer = new List<byte>();
            foreach (var character in s)
                buffer.Add((byte)character);

            return buffer.ToArray();
        }
    }

    internal class Endpoints
    {
        public static string m_clientManagerAddress = "127.0.0.1";
        public static UInt16 m_clientManagerPort = 1339;
        
        private class ClientEntrypointACK
        {
            public string ipAddress;
            public UInt16 port;
        }

        public static async Task<MemoryStream> ClientEntrypoint()
        {
            using (var stream = new MemoryStream())
            {
                var response = new ClientEntrypointACK() // TODO: Proper configuration medium
                {
                    ipAddress = m_clientManagerAddress,
                    port = m_clientManagerPort,
                };

                var rawPayload = JsonConvert.SerializeObject(response);

                stream.Write(rawPayload.ToByteArray());

                return stream;
            }
        }
    }
}
