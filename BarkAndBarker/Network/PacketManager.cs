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
        private static Dictionary<PacketCommand, Func<ClientSession, dynamic, object>>       m_requests  = new Dictionary<PacketCommand, Func<ClientSession, dynamic, object>>();
        private static Dictionary<PacketCommand, Func<ClientSession, dynamic, MemoryStream>> m_responses = new Dictionary<PacketCommand, Func<ClientSession, dynamic, MemoryStream>>();

        public PacketManager()
        {
            //  Pre-lobby and/or General
            // Heartbeat
            m_requests.Add(PacketCommand.C2SAliveReq, PacketProcessors.HandleAliveReq);
            m_responses.Add(PacketCommand.S2CAliveRes, PacketProcessors.HandleAliveRes);

            // Login
            m_requests.Add(PacketCommand.C2SAccountLoginReq, PacketProcessors.HandleLoginReq);
            m_responses.Add(PacketCommand.S2CAccountLoginRes, PacketProcessors.HandleLoginRes);

            // Character management
            m_requests.Add(PacketCommand.C2SAccountCharacterCreateReq, PacketProcessors.HandleCharacterCreateReq);
            m_responses.Add(PacketCommand.S2CAccountCharacterCreateRes, PacketProcessors.HandleCharacterCreateRes);
            m_requests.Add(PacketCommand.C2SAccountCharacterListReq, PacketProcessors.HandleCharacterListReq);
            m_responses.Add(PacketCommand.S2CAccountCharacterListRes, PacketProcessors.HandleCharacterListRes);

            // Lobby
            m_requests.Add(PacketCommand.C2SLobbyEnterReq, PacketProcessors.HandleLobbyEnterReq);
            m_responses.Add(PacketCommand.S2CLobbyEnterRes, PacketProcessors.HandleLobbyEnterRes);

            //  In-lobby stuff
            // No idea
            m_requests.Add(PacketCommand.C2SCustomizeCharacterInfoReq, PacketProcessors.HandleCustomizeCharacterInfoReq);
            m_responses.Add(PacketCommand.S2CCustomizeCharacterInfoRes, PacketProcessors.HandleCustomizeCharacterInfoRes);

            // No idea yet
            m_requests.Add(PacketCommand.C2SCustomizeActionInfoReq, PacketProcessors.HandleCustomizeActionInfoReq);
            m_responses.Add(PacketCommand.S2CCustomizeActionInfoRes, PacketProcessors.HandleCustomizeActionInfoRes);

            // Opening the lobby, telling the client which region the client is
            m_requests.Add(PacketCommand.C2SOpenLobbyMapReq, PacketProcessors.HandleOpenLobbyMapReq);
            m_responses.Add(PacketCommand.S2COpenLobbyMapRes, PacketProcessors.HandleOpenLobbyMapRes);

            // Matchmaking region
            m_requests.Add(PacketCommand.C2SMetaLocationReq, PacketProcessors.HandleMetaLocationReq);
            m_responses.Add(PacketCommand.S2CMetaLocationRes, PacketProcessors.HandleMetaLocationRes);

            // Current player equipment items
            m_requests.Add(PacketCommand.C2SClassEquipInfoReq, PacketProcessors.HandleClassEquipInfoReq);
            m_responses.Add(PacketCommand.S2CClassEquipInfoRes, PacketProcessors.HandleClassEquipInfoRes);

            // I believe current customization items?
            m_requests.Add(PacketCommand.C2SCustomizeItemInfoReq, PacketProcessors.HandleCustomizeItemInfoReq);
            m_responses.Add(PacketCommand.S2CCustomizeItemInfoRes, PacketProcessors.HandleCustomizeItemInfoRes);

            // Character level, experience and skill points; last playtest they didn't have this feature.
            m_requests.Add(PacketCommand.C2SClassLevelInfoReq, PacketProcessors.HandleClassLevelInfoReq);
            m_responses.Add(PacketCommand.S2CClassLevelInfoRes, PacketProcessors.HandleClassLevelInfoRes);

            // Matchmaking registration/withdrawal requests
            m_requests.Add(PacketCommand.C2SAutoMatchRegReq, PacketProcessors.HandleMatchmakingReq);
            m_responses.Add(PacketCommand.S2CAutoMatchRegRes, PacketProcessors.HandleMatchmakingRes);

            // Merchants list
            m_requests.Add(PacketCommand.C2SMerchantListReq, PacketProcessors.HandleMerchantListReq);
            m_responses.Add(PacketCommand.S2CMerchantListRes, PacketProcessors.HandleMerchantListRes);

            // Leaderboard
            m_requests.Add(PacketCommand.C2SRankingRangeReq, RankingProcessors.HandleRankingReq);
            m_responses.Add(PacketCommand.S2CRankingRangeRes, RankingProcessors.HandleRankingRes);

            m_requests.Add(PacketCommand.C2SGatheringHallChannelListReq, GatheringHallProcessors.HandleGhateringHallListReq);
            m_responses.Add(PacketCommand.S2CGatheringHallChannelListRes, GatheringHallProcessors.HandleGatheringHallListRes);
        }

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
