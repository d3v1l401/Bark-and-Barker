using BarkAndBarker.Network;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Bcpg;

namespace BarkAndBarker.Proxy
{
    internal class PacketAnalyzer
    {
        private Logger rawLogger;
        private Logger analyzedLogger;

        public PacketAnalyzer()
        {
            rawLogger = new Logger($"rawPacketLog_{DateTime.Now.Ticks}.txt", false);
            analyzedLogger = new Logger($"analyPacketLog_{DateTime.Now.Ticks}.txt", true);
        }

        public void Analyze(MemoryStream buffer, Direction dir, string rawStringified)
        {
            rawLogger.Log(rawStringified);

            var deser = new WrapperDeserializer(buffer);
            
            try
            {
                var packetType = deser.GetPacketClass().ToString();

                analyzedLogger.Log(packetType);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
