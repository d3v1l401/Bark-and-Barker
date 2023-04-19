using BarkAndBarker.Network;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Bcpg;

namespace BarkAndBarker.Proxy
{
    internal class PacketAnalyzer
    {
        private Logger rawLogger;
        private Logger analyzedLogger;

        private Queue<byte> internalBuffer;

        public PacketAnalyzer(Logger rawLogger, Logger analyzedLogger)
        {
            internalBuffer = new Queue<byte>();

            this.rawLogger = rawLogger;
            this.analyzedLogger = analyzedLogger;
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
                var memoryStream = new MemoryStream();
                memoryStream.Write(buffer, 0, buffer.Length);

                //var deser = new WrapperDeserializer(buffer);
                //
                //var packetType = deser.GetPacketClass().ToString();
                //
                //analyzedLogger.Log(packetType);

            }
            catch (Exception ex)
            {
                rawLogger.Log(ex.Message);
                analyzedLogger.Log("UNRECOGNIZED!" + rawStringified);
            }
        }
    }
}
