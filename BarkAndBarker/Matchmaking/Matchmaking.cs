using BarkAndBarker;
using BarkAndBarker.Network;
using DC.Packet;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Utilities.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker
{
    public class Matchmaking
    {
        public static int MaxPlayersPerSession = 12;
        public static int MinPlayersPerSession = 1;
        public static int MaxSessionsPerRegion = 1;

        public class GameSession
        {
            private List<ClientSession> PlayingClients = new List<ClientSession>();

            public GameSession(List<ClientSession> players)
            {
                this.PlayingClients = players;
            }

            public GameSession() { }

            public void NotifyClients()
            {
                Task.Run(this.notifyClientsInternal);
            }

            public async Task<int> notifyClientsInternal()
            {
                await Task.Delay(TimeSpan.FromSeconds(2));

                var notified = 0;
                foreach (var client in PlayingClients)
                {
                    var packet = new SS2C_ENTER_GAME_SERVER_NOT();

                    packet.SessionId = Guid.NewGuid().ToString();
                    packet.AccountId = client.m_currentPlayer.SteamID;
                    packet.NickName = new SACCOUNT_NICKNAME()
                    {
                        OriginalNickName = client.m_currentCharacter.Nickname,
                        StreamingModeNickName = client.m_currentCharacter.Nickname,
                        KarmaRating = client.m_currentCharacter.KarmaScore,
                    };
                    packet.Ip = "127.0.0.1";
                    packet.Port = 3000;

                    new GameServer(System.Net.IPAddress.Parse(packet.Ip), (int)packet.Port).Start();

                    Console.WriteLine("NotifyClients is sending updates");

                    var serial = new WrapperSerializer<SS2C_ENTER_GAME_SERVER_NOT>(packet, client.m_currentPacketSequence++, PacketCommand.S2CEnterGameServerNot);

                    if (client.SendAsync(serial.Serialize().ToArray()))
                        notified++;
                }

                return notified;
            }
        }

        private static Matchmaking __singleton = new Matchmaking();
        private static bool IsAcceptingPlayers = true;

        public static Matchmaking Instance() 
            => __singleton;

        public enum Regions : int
        {
            EU = 0,
        }

        // [Region, List<Player>]
        private Dictionary<Regions, List<ClientSession>> m_enlistedPlayers = new Dictionary<Regions, List<ClientSession>>();
        private Dictionary<Regions, List<GameSession>>   m_runningGameSessions = new Dictionary<Regions, List<GameSession>>();
        public Matchmaking()
        {
            IsAcceptingPlayers = true;
            this.m_runningGameSessions[Regions.EU] = new List<GameSession>();
            this.m_enlistedPlayers[Regions.EU] = new List<ClientSession>();
        }

        public async Task AddUser(ClientSession session, Regions region = Regions.EU)
            => this.m_enlistedPlayers[region].Add(session);

        public async Task RemoveUser(ClientSession session, Regions region = Regions.EU)
            => this.m_enlistedPlayers[region].Remove(session);

        public async Task<GameSession> PrepareGameSession(Regions region, int playerCount = 0)
        {
            var selectedPlayers = new List<ClientSession>();

            if (playerCount == 0)
                playerCount = MaxPlayersPerSession;

            // TODO: FIFO list
            foreach (var player in this.m_enlistedPlayers[region])
            {
                if (selectedPlayers.Count <= playerCount)
                    selectedPlayers.Add(player);
                else break;
            }

            return new GameSession(selectedPlayers);
        }

        public void AcceptPlayers()
            => IsAcceptingPlayers = true;

        public async Task Matchmake()
        {
            while (IsAcceptingPlayers)
            {
                if (this.m_runningGameSessions[Regions.EU].Count < MaxSessionsPerRegion)
                {
                    var session = await this.PrepareGameSession(Regions.EU, 1);

                    await Task.Delay(1000);

                    session.NotifyClients();
                    return;
                }

                IsAcceptingPlayers = false;
            }

            return;
        }
    }
}
