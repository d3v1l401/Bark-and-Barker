using System.Collections.Concurrent;
using System.Globalization;

namespace BarkAndBarker.Proxy
{
    internal class Logger
    {
        private BlockingCollection<string> logQueue = new BlockingCollection<string>();
        private string logFilePath;

        private bool printToConsole;

        public Logger(string logFilePath, bool printToConsole)
        {
            this.printToConsole = printToConsole;

            this.logFilePath = logFilePath;

            Task.Run(() => ProcessLogQueueAsync());
            this.printToConsole = printToConsole;
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
                    if (printToConsole)
                    {
                        Console.WriteLine(logEntry);
                    }

                    await fileWriter.WriteLineAsync(logEntry);
                    await fileWriter.FlushAsync();
                }

                Thread.Sleep(3);
            }

            fileWriter.Close();
        }
    }
}