using BarkAndBarker.Persistence;
using NetCoreServer;
using System.Collections.Concurrent;
using System.Net;
using BarkAndBarker.Shared.Persistence;

namespace BarkAndBarker
{
    internal class Program
    {
        private static bool     m_keepRunning = false;
        private static DateTime m_lastTickUpdate = DateTime.MinValue;
        private static Settings.SData m_settings = null;

        private static Tuple<long, long> GetSessionsStatistics(ConcurrentDictionary<Guid, TcpSession> sessions)
        {
            long sent = 0, recv = 0;

            foreach (var session in sessions)
            {
                sent += session.Value.BytesSent;
                recv += session.Value.BytesReceived;
            }

            return new Tuple<long, long>(sent, recv);
        }

        static void Main(string[] args)
        {
#if DEBUG
            Console.WriteLine("Importing settings from 'settings.json'...");
            m_settings = Settings.ImportSettings("./settings.json");
#else
            Console.WriteLine("Importing settings from environment...");
            m_settings = Settings.ImportSettings();
#endif
            if (m_settings == null)
            {
                Console.WriteLine("Could not import settings.");
                return;
            }

            Console.WriteLine("Connecting to database...");
            Database.ConnectionString = m_settings.DBConnectionString;
            var database = new Database();
            database.Connect();
            if (!database.IsConnected())
            {
                Console.WriteLine("Could not connect to the database, please fix.");
                return;
            }

            DBGeneral.CheckAndCreateDatabase(database);

            Console.WriteLine("Fireing up ClientManager...");
            ClientManager m_clientManager = new ClientManager(IPAddress.Parse(m_settings.LobbyAddress), m_settings.LobbyPort);
            if (!m_clientManager.Start())
            {
                Console.WriteLine("Failed to start ClientManager.");
                return;
            }

            Console.WriteLine("Fireing up CentralServer...");
            Endpoints.m_clientManagerAddress = m_settings.LobbyAddress;
            Endpoints.m_clientManagerPort = m_settings.LobbyPort;

            CentralServer m_centralServer = new CentralServer(m_settings.CSAddress, m_settings.CSPort);
            m_centralServer.Start();

            m_keepRunning = true;

            Console.WriteLine("Listening...");
            while (m_keepRunning) 
            {
                var lastUpdate = DateTime.Now.Subtract(m_lastTickUpdate);
                if (lastUpdate.TotalSeconds >= 5)
                {
                    var sessions = m_clientManager.GetSessions();
                    var stats = GetSessionsStatistics(sessions);
                    var sessCount = sessions.Count();
                    
                    var statsReport = string.Format("Total bytes sent: {0}, received: {1}; total clients: {2}", stats.Item1, stats.Item2, sessCount);
                    
                    Console.WriteLine(statsReport);
                    
                    sessions = null;
                    stats = null;
                    m_lastTickUpdate = DateTime.Now;
                }
            }

            m_centralServer.Stop();
            m_clientManager.Stop();
        }
    }
}