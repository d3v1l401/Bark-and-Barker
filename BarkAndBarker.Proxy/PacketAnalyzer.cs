using BarkAndBarker.Network;
using DC.Packet;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Bcpg;
using System.Net.Sockets;

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
                var packetLengthBytes = internalBuffer.Take(4).ToArray();
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(packetLengthBytes); // Reverse the byte order if running on a little-endian system
                }
                var packetLength = System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt32(packetLengthBytes, 0));
                
                if (internalBuffer.Count >= packetLength)
                {
                    var currPacketBuffer = new byte[packetLength];

                    for (int i = 0; i < packetLength; i++)
                    {
                        currPacketBuffer[i] = internalBuffer.Dequeue();
                    }

                    var memoryStream = new MemoryStream();
                    memoryStream.Write(currPacketBuffer, 0, currPacketBuffer.Length);

                    var deser = new WrapperDeserializer(memoryStream);

                    var packetType = deser.GetPacketClass();
                    
                    analyzedLogger.Log(packetType.ToString());

                    HandleCommand(deser, packetType);
                }
            }
            catch (Exception ex)
            {
                rawLogger.Log(ex.Message);
                analyzedLogger.Log("UNRECOGNIZED!" + rawStringified);
            }
        }

        private void HandleCommand(WrapperDeserializer deserializer, PacketCommand command)
        {
            switch (command)
            {
                case PacketCommand.C2SAliveReq:
                    var C2SAliveReq = deserializer.Parse<SC2S_ALIVE_REQ>();
                    analyzedLogger.Log(C2SAliveReq.ToString());
                    break;
                case PacketCommand.C2SAccountLoginReq:
                    var C2SAccountLoginReq = deserializer.Parse<IronMace_Login>();
                    analyzedLogger.Log(C2SAccountLoginReq.ToString());
                    break;
                case PacketCommand.C2SAccountCharacterCreateReq:
                    var C2SAccountCharacterCreateReq = deserializer.Parse<SC2S_ACCOUNT_CHARACTER_CREATE_REQ>();
                    analyzedLogger.Log(C2SAccountCharacterCreateReq.ToString());
                    break;
                case PacketCommand.C2SAccountCharacterListReq:
                    var C2SAccountCharacterListReq = deserializer.Parse<SC2S_ACCOUNT_CHARACTER_LIST_REQ>();
                    analyzedLogger.Log(C2SAccountCharacterListReq.ToString());
                    break;
                case PacketCommand.C2SAccountCharacterDeleteReq:
                    var C2SAccountCharacterDeleteReq = deserializer.Parse<SC2S_ACCOUNT_CHARACTER_DELETE_REQ>();
                    analyzedLogger.Log(C2SAccountCharacterDeleteReq.ToString());
                    break;
                case PacketCommand.C2SLobbyEnterReq:
                    var C2SLobbyEnterReq = deserializer.Parse<SC2S_LOBBY_ENTER_REQ>();
                    analyzedLogger.Log(C2SLobbyEnterReq.ToString());
                    break;
                case PacketCommand.C2SCustomizeCharacterInfoReq:
                    var C2SCustomizeCharacterInfoReq = deserializer.Parse<IronMace_Login>();
                    analyzedLogger.Log(C2SCustomizeCharacterInfoReq.ToString());
                    break;
                case PacketCommand.C2SCustomizeActionInfoReq:
                    var C2SCustomizeActionInfoReq = deserializer.Parse<SC2S_CUSTOMIZE_ACTION_INFO_REQ>();
                    analyzedLogger.Log(C2SCustomizeActionInfoReq.ToString());
                    break;
                case PacketCommand.C2SOpenLobbyMapReq:
                    var C2SOpenLobbyMapReq = deserializer.Parse<SC2S_OPEN_LOBBY_MAP_REQ>();
                    analyzedLogger.Log(C2SOpenLobbyMapReq.ToString());
                    break;
                case PacketCommand.C2SMetaLocationReq:
                    var C2SMetaLocationReq = deserializer.Parse<SC2S_META_LOCATION_REQ>();
                    analyzedLogger.Log(C2SMetaLocationReq.ToString());
                    break;
                case PacketCommand.C2SClassEquipInfoReq:
                    var C2SClassEquipInfoReq = deserializer.Parse<SC2S_CLASS_EQUIP_INFO_REQ>();
                    analyzedLogger.Log(C2SClassEquipInfoReq.ToString());
                    break;
                case PacketCommand.C2SCustomizeItemInfoReq:
                    var C2SCustomizeItemInfoReq = deserializer.Parse<SC2S_CUSTOMIZE_ITEM_INFO_REQ>();
                    analyzedLogger.Log(C2SCustomizeItemInfoReq.ToString());
                    break;
                case PacketCommand.C2SClassLevelInfoReq:
                    var C2SClassLevelInfoReq = deserializer.Parse<SC2S_CLASS_LEVEL_INFO_REQ>();
                    analyzedLogger.Log(C2SClassLevelInfoReq.ToString());
                    break;
                case PacketCommand.C2SAutoMatchRegReq:
                    var C2SAutoMatchRegReq = deserializer.Parse<SC2S_AUTO_MATCH_REG_REQ>();
                    analyzedLogger.Log(C2SAutoMatchRegReq.ToString());
                    break;
                case PacketCommand.C2SMerchantListReq:
                    var C2SMerchantListReq = deserializer.Parse<SC2S_MERCHANT_LIST_REQ>();
                    analyzedLogger.Log(C2SMerchantListReq.ToString());
                    break;
                case PacketCommand.C2SRankingRangeReq:
                    var C2SRankingRangeReq = deserializer.Parse<SC2S_RANKING_RANGE_REQ>();
                    analyzedLogger.Log(C2SRankingRangeReq.ToString());
                    break;
                case PacketCommand.C2SGatheringHallChannelListReq:
                    var C2SGatheringHallChannelListReq = deserializer.Parse<SC2S_GATHERING_HALL_CHANNEL_LIST_REQ>();
                    analyzedLogger.Log(C2SGatheringHallChannelListReq.ToString());
                    break;
                case PacketCommand.C2SPartyInviteReq:
                    var C2SPartyInviteReq = deserializer.Parse<SC2S_PARTY_INVITE_REQ>();
                    analyzedLogger.Log(C2SPartyInviteReq.ToString());
                    break;
                case PacketCommand.C2SPartyExitReq:
                    var C2SPartyExitReq = deserializer.Parse<SC2S_PARTY_EXIT_REQ>();
                    analyzedLogger.Log(C2SPartyExitReq.ToString());
                    break;
                case PacketCommand.C2SPartyInviteAnswerReq:
                    var C2SPartyInviteAnswerReq = deserializer.Parse<SC2S_PARTY_INVITE_ANSWER_REQ>();
                    analyzedLogger.Log(C2SPartyInviteAnswerReq.ToString());
                    break;
                case PacketCommand.C2SPartyMemberKickReq:
                    var C2SPartyMemberKickReq = deserializer.Parse<SC2S_PARTY_MEMBER_KICK_REQ>();
                    analyzedLogger.Log(C2SPartyMemberKickReq.ToString());
                    break;
                case PacketCommand.C2SPartyReadyReq:
                    var C2SPartyReadyReq = deserializer.Parse<SC2S_PARTY_READY_REQ>();
                    analyzedLogger.Log(C2SPartyReadyReq.ToString());
                    break;
                case PacketCommand.C2SPartyChatReq:
                    var C2SPartyChatReq = deserializer.Parse<SC2S_PARTY_CHAT_REQ>();
                    analyzedLogger.Log(C2SPartyChatReq.ToString());
                    break;






                case PacketCommand.S2CAliveRes:
                    var S2CAliveRes = deserializer.Parse<SS2C_ALIVE_RES>();
                    analyzedLogger.Log(S2CAliveRes.ToString());
                    break;
                case PacketCommand.S2CAccountLoginRes:
                    var S2CAccountLoginRes = deserializer.Parse<SS2C_ACCOUNT_LOGIN_RES>();
                    analyzedLogger.Log(S2CAccountLoginRes.ToString());
                    break;
                case PacketCommand.S2CAccountCharacterCreateRes:
                    var S2CAccountCharacterCreateRes = deserializer.Parse<SS2C_ACCOUNT_CHARACTER_CREATE_RES>();
                    analyzedLogger.Log(S2CAccountCharacterCreateRes.ToString());
                    break;
                case PacketCommand.S2CAccountCharacterListRes:
                    var S2CAccountCharacterListRes = deserializer.Parse<SS2C_ACCOUNT_CHARACTER_LIST_RES>();
                    analyzedLogger.Log(S2CAccountCharacterListRes.ToString());
                    break;
                case PacketCommand.S2CAccountCharacterDeleteRes:
                    var S2CAccountCharacterDeleteRes = deserializer.Parse<SS2C_ACCOUNT_CHARACTER_DELETE_RES>();
                    analyzedLogger.Log(S2CAccountCharacterDeleteRes.ToString());
                    break;
                case PacketCommand.S2CLobbyEnterRes:
                    var S2CLobbyEnterRes = deserializer.Parse<SS2C_LOBBY_ENTER_RES>();
                    analyzedLogger.Log(S2CLobbyEnterRes.ToString());
                    break;
                case PacketCommand.S2CCustomizeCharacterInfoRes:
                    var S2CCustomizeCharacterInfoRes = deserializer.Parse<SS2C_CUSTOMIZE_CHARACTER_INFO_RES>();
                    analyzedLogger.Log(S2CCustomizeCharacterInfoRes.ToString());
                    break;
                case PacketCommand.S2CCustomizeActionInfoRes:
                    var S2CCustomizeActionInfoRes = deserializer.Parse<SS2C_CUSTOMIZE_ACTION_INFO_RES>();
                    analyzedLogger.Log(S2CCustomizeActionInfoRes.ToString());
                    break;
                case PacketCommand.S2COpenLobbyMapRes:
                    var S2COpenLobbyMapRes = deserializer.Parse<SS2C_OPEN_LOBBY_MAP_RES>();
                    analyzedLogger.Log(S2COpenLobbyMapRes.ToString());
                    break;
                case PacketCommand.S2CMetaLocationRes:
                    var S2CMetaLocationRes = deserializer.Parse<SS2C_META_LOCATION_RES>();
                    analyzedLogger.Log(S2CMetaLocationRes.ToString());
                    break;
                case PacketCommand.S2CClassEquipInfoRes:
                    var S2CClassEquipInfoRes = deserializer.Parse<SS2C_CLASS_EQUIP_INFO_RES>();
                    analyzedLogger.Log(S2CClassEquipInfoRes.ToString());
                    break;
                case PacketCommand.S2CCustomizeItemInfoRes:
                    var S2CCustomizeItemInfoRes = deserializer.Parse<SS2C_CUSTOMIZE_ITEM_INFO_RES>();
                    analyzedLogger.Log(S2CCustomizeItemInfoRes.ToString());
                    break;
                case PacketCommand.S2CClassLevelInfoRes:
                    var S2CClassLevelInfoRes = deserializer.Parse<SS2C_CLASS_LEVEL_INFO_RES>();
                    analyzedLogger.Log(S2CClassLevelInfoRes.ToString());
                    break;
                case PacketCommand.S2CAutoMatchRegRes:
                    var S2CAutoMatchRegRes = deserializer.Parse<SS2C_AUTO_MATCH_REG_RES>();
                    analyzedLogger.Log(S2CAutoMatchRegRes.ToString());
                    break;
                case PacketCommand.S2CMerchantListRes:
                    var S2CMerchantListRes = deserializer.Parse<SS2C_MERCHANT_LIST_RES>();
                    analyzedLogger.Log(S2CMerchantListRes.ToString());
                    break;
                case PacketCommand.S2CRankingRangeRes:
                    var S2CRankingRangeRes = deserializer.Parse<SS2C_RANKING_RANGE_RES>();
                    analyzedLogger.Log(S2CRankingRangeRes.ToString());
                    break;
                case PacketCommand.S2CGatheringHallChannelListRes:
                    var S2CGatheringHallChannelListRes = deserializer.Parse<SS2C_GATHERING_HALL_CHANNEL_LIST_RES>();
                    analyzedLogger.Log(S2CGatheringHallChannelListRes.ToString());
                    break;
                case PacketCommand.S2CPartyInviteRes:
                    var S2CPartyInviteRes = deserializer.Parse<SS2C_PARTY_INVITE_RES>();
                    analyzedLogger.Log(S2CPartyInviteRes.ToString());
                    break;
                case PacketCommand.S2CPartyExitRes:
                    var S2CPartyExitRes = deserializer.Parse<SS2C_PARTY_EXIT_RES>();
                    analyzedLogger.Log(S2CPartyExitRes.ToString());
                    break;
                case PacketCommand.S2CPartyInviteAnswerRes:
                    var S2CPartyInviteAnswerRes = deserializer.Parse<SS2C_PARTY_INVITE_ANSWER_RES>();
                    analyzedLogger.Log(S2CPartyInviteAnswerRes.ToString());
                    break;
                case PacketCommand.S2CPartyMemberKickRes:
                    var S2CPartyMemberKickRes = deserializer.Parse<SS2C_PARTY_MEMBER_KICK_RES>();
                    analyzedLogger.Log(S2CPartyMemberKickRes.ToString());
                    break;
                case PacketCommand.S2CPartyReadyRes:
                    var S2CPartyReadyRes = deserializer.Parse<SS2C_PARTY_READY_RES>();
                    analyzedLogger.Log(S2CPartyReadyRes.ToString());
                    break;
                case PacketCommand.S2CPartyChatRes:
                    var S2CPartyChatRes = deserializer.Parse<SS2C_PARTY_CHAT_RES>();
                    analyzedLogger.Log(S2CPartyChatRes.ToString());
                    break;
                default:
                    Console.WriteLine($"Unhandeled packet command {command.ToString()}");
                    break;
            }
        }

    }
}
