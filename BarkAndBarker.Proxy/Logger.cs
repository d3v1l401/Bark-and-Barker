using System.Collections.Concurrent;
using System.Globalization;

namespace BarkAndBarker.Proxy
{
    internal class Logger
    {
        private BlockingCollection<string> logQueue = new BlockingCollection<string>();
        private string logFilePath;

        public Logger()
        {
            logFilePath = $"packetLog_{DateTime.Now.Ticks}.txt";

            Task.Run(() => ProcessLogQueueAsync());
        }

        public void Log(string logEntry)
        {
            logQueue.Add(logEntry);
        }

        private async Task ProcessLogQueueAsync()
        {
            StreamWriter fileWriter = new StreamWriter(logFilePath, true);
            while (true)
            {
                if (logQueue.TryTake(out string logEntry, TimeSpan.FromSeconds(1)))
                {
                    Console.WriteLine(logEntry);
                    await fileWriter.WriteLineAsync(logEntry);
                    await fileWriter.FlushAsync();
                }

                Thread.Sleep(3);
            }

            fileWriter.Close();
        }
    }
}