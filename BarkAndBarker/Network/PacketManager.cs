using BarkAndBarker.Persistence;
using DC.Packet;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarkAndBarker.Network.PacketProcessor;
using static BarkAndBarker.Network.PacketManager;

namespace BarkAndBarker.Network
{
    public class PacketManager
    {
        // Some packets are triggered on top of other requests (*_NOT), this lists what notifications needs to be sent on top of those packets
        private static readonly Dictionary<PacketCommand, List<Func<ClientSession, object>>> m_notificationTriggers = new Dictionary<PacketCommand, List<Func<ClientSession, object>>>()
        {
            /*
                Typical structure: TriggerPacketType -> List(PacketProcessorMethods)
            */

            // For login packet, SS2C_SERVICE_POLICY_NOT is to be sent as well
            { PacketCommand.C2SAccountLoginReq, new List<Func<ClientSession, object>>() 
                {
                    Notifications.ServicePolicyNotification
                    
                } 
            },


            { PacketCommand.C2SLobbyEnterReq, new List<Func<ClientSession, object>>()
                {
                    CharacterProcessors.HandleClassEquipInfoTrigger,
                    CharacterProcessors.HandleLobbyCharacterInfoTrigger,
                    Notifications.HandleLobbyAccountCurrencyListNot
                } 
            },

             { PacketCommand.C2SClassLevelInfoReq, new List<Func<ClientSession, object>>()
                {
                    CharacterProcessors.HandleLobbyCharacterInfoTrigger,
                    Notifications.HandleLobbyAccountCurrencyListNot
                }
            },

        };

        // Handles the request, needs a PacketCommand, a WrapperDeserializer & returns the deserialized object
        public static readonly Dictionary<PacketCommand, Func<ClientSession, dynamic, object>> m_requests = new Dictionary<PacketCommand, Func<ClientSession, dynamic, object>>()
        {
            // Heartbeat
            { PacketCommand.C2SAliveReq, MiscProcessors.HandleAliveReq },
            // Login
            { PacketCommand.C2SAccountLoginReq, AccountProcessors.HandleLoginReq },


            // Character management
            { PacketCommand.C2SAccountCharacterCreateReq, CharacterProcessors.HandleCharacterCreateReq },
            { PacketCommand.C2SAccountCharacterListReq, CharacterProcessors.HandleCharacterListReq },
            { PacketCommand.C2SAccountCharacterDeleteReq, CharacterProcessors.HandleCharacterDeletionReq },
            // Lobby
            { PacketCommand.C2SLobbyEnterReq, LobbyProcessors.HandleLobbyEnterReq },
            // In-Lobby
            { PacketCommand.C2SCustomizeCharacterInfoReq, CustomizationProcessors.HandleCustomizeCharacterInfoReq },
            { PacketCommand.C2SCustomizeActionInfoReq, CustomizationProcessors.HandleCustomizeActionInfoReq },
            // Lobby opening, region selection
            { PacketCommand.C2SOpenLobbyMapReq, LobbyProcessors.HandleOpenLobbyMapReq },
            // Matchmaking region
            { PacketCommand.C2SMetaLocationReq, LobbyProcessors.HandleMetaLocationReq },
            // Current equipped items
            { PacketCommand.C2SClassEquipInfoReq, EquipProcessors.HandleClassEquipInfoReq },
            // Customization items 
            //{ PacketCommand.C2SCustomizeItemInfoReq, EquipProcessors.HandleCustomizeItemInfoReq },
            // Character level, exp and ability points
            { PacketCommand.C2SClassLevelInfoReq, ProgressionProcessors.HandleClassLevelInfoReq },
            // Matchmaking registration
            { PacketCommand.C2SAutoMatchRegReq, MatchmakingProcessors.HandleMatchmakingReq },
            // Merchants operations
            { PacketCommand.C2SMerchantListReq, MerchantProcessors.HandleMerchantListReq },
            // Leaderboards 
            { PacketCommand.C2SRankingRangeReq, RankingProcessors.HandleRankingReq },
            // Gathering hall
            { PacketCommand.C2SGatheringHallChannelListReq, GatheringHallProcessors.HandleGhateringHallListReq },
            // Party packets
            { PacketCommand.C2SPartyInviteReq, PartyProcessors.HandlePartyInviteReq },
            { PacketCommand.C2SPartyExitReq, PartyProcessors.HandlePartyExitReq },
            { PacketCommand.C2SPartyInviteAnswerReq, PartyProcessors.HandlePartyInviteAnswerReq },
            { PacketCommand.C2SPartyMemberKickReq, PartyProcessors.HandlePartyMemberKickReq },
            { PacketCommand.C2SPartyReadyReq, PartyProcessors.HandlePartyReadyReq },
            { PacketCommand.C2SPartyChatReq, PartyProcessors.HandlePartyChatReq },

            // Map Selection
            { PacketCommand.C2SLobbyGameDifficultySelectReq, LobbyProcessors.HandleLobbyGameDifficultySelectReq },


            // Lobby Character Information
            { PacketCommand.C2SLobbyCharacterInfoReq, CharacterProcessors.HandleLobbyCharacterInfoReq },
            { PacketCommand.C2SCharacterSelectEnterReq, CharacterProcessors.HandleCharacterSelectReq },

            // Block Character
            { PacketCommand.C2SBlockCharacterListReq, CharacterProcessors.HandleBlockCharacterListReq },

            // Skill List
            { PacketCommand.C2SClassSpellListReq, CharacterProcessors.HandleClassSpellListReq },
            { PacketCommand.C2SClassSkillListReq, CharacterProcessors.HandleClassSkillListReq },
            { PacketCommand.C2SClassPerkListReq, CharacterProcessors.HandleClassPerkListReq },





        };
        public static readonly Dictionary<PacketCommand, Func<ClientSession, dynamic, MemoryStream>> m_responses = new Dictionary<PacketCommand, Func<ClientSession, dynamic, MemoryStream>>()
        {
            { PacketCommand.S2CAliveRes, MiscProcessors.HandleAliveRes },

            { PacketCommand.S2CAccountLoginRes, AccountProcessors.HandleLoginRes },

            { PacketCommand.S2CAccountCharacterCreateRes, CharacterProcessors.HandleCharacterCreateRes },
            { PacketCommand.S2CAccountCharacterListRes, CharacterProcessors.HandleCharacterListRes },
            { PacketCommand.S2CAccountCharacterDeleteRes, CharacterProcessors.HandleCharacterDeletionRes },

            { PacketCommand.S2CLobbyEnterRes, LobbyProcessors.HandleLobbyEnterRes },
            { PacketCommand.S2CCustomizeCharacterInfoRes, CustomizationProcessors.HandleCustomizeCharacterInfoRes },
            { PacketCommand.S2CCustomizeActionInfoRes, CustomizationProcessors.HandleCustomizeActionInfoRes },
            { PacketCommand.S2COpenLobbyMapRes, LobbyProcessors.HandleOpenLobbyMapRes },
            { PacketCommand.S2CMetaLocationRes, LobbyProcessors.HandleMetaLocationRes },
            { PacketCommand.S2CClassEquipInfoRes, EquipProcessors.HandleClassEquipInfoRes },
            { PacketCommand.S2CCustomizeItemInfoRes, EquipProcessors.HandleCustomizeItemInfoRes },
            { PacketCommand.S2CClassLevelInfoRes, ProgressionProcessors.HandleClassLevelInfoRes },
            { PacketCommand.S2CAutoMatchRegRes, MatchmakingProcessors.HandleMatchmakingRes },
            { PacketCommand.S2CMerchantListRes, MerchantProcessors.HandleMerchantListRes },
            { PacketCommand.S2CRankingRangeRes, RankingProcessors.HandleRankingRes },
            { PacketCommand.S2CGatheringHallChannelListRes, GatheringHallProcessors.HandleGatheringHallListRes },

            { PacketCommand.S2CPartyInviteRes, PartyProcessors.HandlePartyInviteRes },
            { PacketCommand.S2CPartyExitRes, PartyProcessors.HandlePartyExitRes },
            { PacketCommand.S2CPartyInviteAnswerRes, PartyProcessors.HandlePartyInviteAnswerRes },
            { PacketCommand.S2CPartyMemberKickRes, PartyProcessors.HandlePartyMemberKickRes },
            { PacketCommand.S2CPartyReadyRes, PartyProcessors.HandlePartyReadyRes },
            { PacketCommand.S2CPartyChatRes, PartyProcessors.HandlePartyChatRes },


            { PacketCommand.S2CLobbyGameDifficultySelectRes, LobbyProcessors.HandleLobbyGameDifficultySelectRes },


            { PacketCommand.S2CLobbyCharacterInfoRes, CharacterProcessors.HandleLobbyCharacterInfoRes },
            { PacketCommand.S2CCharacterSelectEnterRes, CharacterProcessors.HandleCharacterSelectRes },

            { PacketCommand.S2CBlockCharacterListRes, CharacterProcessors.HandleBlockCharacterListRes },

            { PacketCommand.S2CClassSpellListRes, CharacterProcessors.HandleClassSpellListRes },
            { PacketCommand.S2CClassSkillListRes, CharacterProcessors.HandleClassSkillListRes },
            { PacketCommand.S2CClassPerkListRes, CharacterProcessors.HandleClassPerkListRes},

        };

        public PacketManager() { }

        public bool HasOverlappingNotifications(PacketCommand packetCommand)
        {
            if (m_notificationTriggers.ContainsKey(packetCommand))
                return true;

            return false;
        }

        public List<MemoryStream> Handle(ClientSession session, MemoryStream packet)
        {
            try
            {
                var deser = new WrapperDeserializer(packet);
#if DEBUG
                if (deser.GetPacketClass() != PacketCommand.C2SAliveReq)
                    Console.WriteLine("< " + deser.GetPacketClass());
#endif
                var requestProcessor = m_requests[deser.GetPacketClass()];
                var outputData = requestProcessor.Invoke(session, deser);

                var responsePacket = deser.GetPacketClass() + 1;
#if DEBUG
                if (deser.GetPacketClass() != PacketCommand.C2SAliveReq)
                    Console.WriteLine("> " + responsePacket);
#endif
                var responseProcessor = m_responses[responsePacket];

                var response = responseProcessor.Invoke(session, outputData);

                var overlaps = this.GetOverlappingNotifications(deser.GetPacketClass(), session);
                var responseQueue = new List<MemoryStream> { response };
#if DEBUG
                if (overlaps.Count > 0)
                    Console.WriteLine("\t & " + overlaps.Count + " NOTs");
#endif
                responseQueue.AddRange(overlaps);
                responseQueue.Reverse(); // Response packet last, notifications first

                return responseQueue;

            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<MemoryStream> GetOverlappingNotifications(PacketCommand packetType, ClientSession session)
        {
            var notificationsBuffers = new List<MemoryStream>();
            if (this.HasOverlappingNotifications(packetType))
            {
                var notificationsQueue = m_notificationTriggers[packetType];
                foreach (var notif in notificationsQueue)
                {
                    var buff = notif.Invoke(session) as MemoryStream;
                    notificationsBuffers.Add(buff);
                }
            }

            return notificationsBuffers;
        }
    }
}
