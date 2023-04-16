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
        // Handles the request, needs a PacketCommand, a WrapperDeserializer & returns the deserialized object
        private static readonly Dictionary<PacketCommand, Func<ClientSession, dynamic, object>> m_requests = new Dictionary<PacketCommand, Func<ClientSession, dynamic, object>>()
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
            { PacketCommand.C2SCustomizeItemInfoReq, EquipProcessors.HandleCustomizeItemInfoReq },
            // Character level, exp and ability points
            { PacketCommand.C2SClassLevelInfoReq, ProgressionProcessors.HandleClassLevelInfoReq },
            // Matchmaking registration
            { PacketCommand.C2SAutoMatchRegReq, MatchmakingProcessors.HandleMatchmakingReq },
            // Merchants operations
            { PacketCommand.C2SMerchantListReq, MerchantProcessors.HandleMerchantListReq },
            // Leaderboards 
            { PacketCommand.C2SRankingRangeReq, RankingProcessors.HandleRankingReq },
            // Gathering hall
            { PacketCommand.C2SGatheringHallChannelListReq, GatheringHallProcessors.HandleGatheringHallListReq },
            // Party packets
            { PacketCommand.C2SPartyInviteReq, PartyProcessors.HandlePartyInviteReq },
            { PacketCommand.C2SPartyExitReq, PartyProcessors.HandlePartyExitReq },
            { PacketCommand.C2SPartyInviteAnswerReq, PartyProcessors.HandlePartyInviteAnswerReq },
            { PacketCommand.C2SPartyMemberKickReq, PartyProcessors.HandlePartyMemberKickReq },
            { PacketCommand.C2SPartyReadyReq, PartyProcessors.HandlePartyReadyReq },
            { PacketCommand.C2SPartyChatReq, PartyProcessors.HandlePartyChatReq },

        };
        private static readonly Dictionary<PacketCommand, Func<ClientSession, dynamic, MemoryStream>> m_responses = new Dictionary<PacketCommand, Func<ClientSession, dynamic, MemoryStream>>()
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
        };

        public PacketManager() { }

        public MemoryStream Handle(ClientSession session, MemoryStream packet)
        {
            var deser = new WrapperDeserializer(packet);
            
            try
            {
                Console.WriteLine("< " + deser.GetPacketClass());
                var requestProcessor = m_requests[deser.GetPacketClass()];
                var outputData = requestProcessor.Invoke(session, deser);

                var responsePacket = deser.GetPacketClass() + 1;

                Console.WriteLine("> " + responsePacket);
                var responseProcessor = m_responses[responsePacket];

                return responseProcessor.Invoke(session, outputData);

            } catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
