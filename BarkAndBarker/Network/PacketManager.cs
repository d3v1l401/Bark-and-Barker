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
            { PacketCommand.C2SLobbyEnterReq, PacketProcessors.HandleLobbyEnterReq },
            // In-Lobby
            { PacketCommand.C2SCustomizeCharacterInfoReq, PacketProcessors.HandleCustomizeCharacterInfoReq },
            { PacketCommand.C2SCustomizeActionInfoReq, PacketProcessors.HandleCustomizeActionInfoReq },
            // Lobby opening, region selection
            { PacketCommand.C2SOpenLobbyMapReq, PacketProcessors.HandleOpenLobbyMapReq },
            // Matchmaking region
            { PacketCommand.C2SMetaLocationReq, PacketProcessors.HandleMetaLocationReq },
            // Current equipped items
            { PacketCommand.C2SClassEquipInfoReq, PacketProcessors.HandleClassEquipInfoReq },
            // Customization items 
            { PacketCommand.C2SCustomizeItemInfoReq, PacketProcessors.HandleCustomizeItemInfoReq },
            // Character level, exp and ability points
            { PacketCommand.C2SClassLevelInfoReq, PacketProcessors.HandleClassLevelInfoReq },
            // Matchmaking registration
            { PacketCommand.C2SAutoMatchRegReq, PacketProcessors.HandleMatchmakingReq },
            // Merchants operations
            { PacketCommand.C2SMerchantListReq, PacketProcessors.HandleMerchantListReq },
            // Leaderboards 
            { PacketCommand.C2SRankingRangeReq, RankingProcessors.HandleRankingReq },
            // Gathering hall
            { PacketCommand.C2SGatheringHallChannelListReq, GatheringHallProcessors.HandleGhateringHallListReq },

        };
        private static readonly Dictionary<PacketCommand, Func<ClientSession, dynamic, MemoryStream>> m_responses = new Dictionary<PacketCommand, Func<ClientSession, dynamic, MemoryStream>>()
        {
            { PacketCommand.S2CAliveRes, MiscProcessors.HandleAliveRes },
            
            { PacketCommand.S2CAccountLoginRes, AccountProcessors.HandleLoginRes },
            
            { PacketCommand.S2CAccountCharacterCreateRes, CharacterProcessors.HandleCharacterCreateRes },
            { PacketCommand.S2CAccountCharacterListRes, CharacterProcessors.HandleCharacterListRes },
            { PacketCommand.S2CAccountCharacterDeleteRes, CharacterProcessors.HandleCharacterDeletionRes },

            { PacketCommand.S2CLobbyEnterRes, PacketProcessors.HandleLobbyEnterRes },
            { PacketCommand.S2CCustomizeCharacterInfoRes, PacketProcessors.HandleCustomizeCharacterInfoRes },
            { PacketCommand.S2CCustomizeActionInfoRes, PacketProcessors.HandleCustomizeActionInfoRes },
            { PacketCommand.S2COpenLobbyMapRes, PacketProcessors.HandleOpenLobbyMapRes },
            { PacketCommand.S2CMetaLocationRes, PacketProcessors.HandleMetaLocationRes },
            { PacketCommand.S2CClassEquipInfoRes, PacketProcessors.HandleClassEquipInfoRes },
            { PacketCommand.S2CCustomizeItemInfoRes, PacketProcessors.HandleCustomizeItemInfoRes },
            { PacketCommand.S2CClassLevelInfoRes, PacketProcessors.HandleClassLevelInfoRes },
            { PacketCommand.S2CAutoMatchRegRes, PacketProcessors.HandleMatchmakingRes },
            { PacketCommand.S2CMerchantListRes, PacketProcessors.HandleMerchantListRes },
            { PacketCommand.S2CRankingRangeRes, RankingProcessors.HandleRankingRes },
            { PacketCommand.S2CGatheringHallChannelListRes, GatheringHallProcessors.HandleGatheringHallListRes }
        };

        public PacketManager() { }

        public MemoryStream Handle(ClientSession session, MemoryStream packet)
        {
            var deser = new WrapperDeserializer(packet);
            
            try
            {
                var requestProcessor = m_requests[deser.GetPacketClass()];
                var outputData = requestProcessor.Invoke(session, deser);

                var responsePacket = deser.GetPacketClass() + 1;
                var responseProcessor = m_responses[responsePacket];

                return responseProcessor.Invoke(session, outputData);

            } catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
