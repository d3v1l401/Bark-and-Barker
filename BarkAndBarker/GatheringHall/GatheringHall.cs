using BarkAndBarker.Network;
using BarkAndBarker.Shared.Persistence.Models;
using DC.Packet;

namespace BarkAndBarker.GatheringHall
{
    internal class GatheringHall
    {
        public string ChannelId { get; set; }
        public uint ChannelIndex { get; set; }
        public uint GroupIndex { get; set; }
        public uint MemberCount => (uint)CurrentUsers.Count;
        public List<ClientSession> CurrentUsers { get; set; }

        private List<ChatMessage> Messages { get; set; }

        public GatheringHall(string channelId, uint channelIndex, uint groupIndex)
        {
            ChannelId = channelId;
            ChannelIndex = channelIndex;
            GroupIndex = groupIndex;

            CurrentUsers = new List<ClientSession>();
            Messages = new List<ChatMessage>();
        }

        public void Join(ClientSession client)
        {
            var clientUpdateNot = new SS2C_GATHERING_HALL_CHANNEL_USER_UPDATE_NOT();

            var joinUpdate = new SGATHERING_HALL_USER_UPDATE_INFO
            {
                UpdateFlag = 1, // 1 = join
                Info = new SCHARACTER_GATHERING_HALL_INFO
                {
                    AccountId = client.m_currentPlayer.AccountID.ToString(),
                    NickName = new SACCOUNT_NICKNAME()
                    {
                        OriginalNickName = client.m_currentCharacter.Nickname,
                        StreamingModeNickName = client.m_currentCharacter.Nickname,
                        KarmaRating = client.m_currentCharacter.KarmaScore
                    },
                    CharacterClass = client.m_currentCharacter.Class,
                    CharacterId = client.m_currentCharacter.CharID,
                    Gender = (uint)client.m_currentCharacter.Gender,
                    Level = (uint)client.m_currentCharacter.Level
                }
            };

            clientUpdateNot.Updates.Add(joinUpdate);

            foreach (var clientSession in CurrentUsers)
            {
                var joinNot = new WrapperSerializer<SS2C_GATHERING_HALL_CHANNEL_USER_UPDATE_NOT>(
                    clientUpdateNot,
                    clientSession.m_currentPacketSequence++,
                    PacketCommand.S2CGatheringHallChannelUserUpdateNot);

                clientSession.SendAsync(joinNot.Serialize().ToArray());
            }

            CurrentUsers.Add(client);
        }

        public void Leave(ClientSession client)
        {
            CurrentUsers.Remove(client);

            var clientUpdateNot = new SS2C_GATHERING_HALL_CHANNEL_USER_UPDATE_NOT();

            var leaveUpdate = new SGATHERING_HALL_USER_UPDATE_INFO
            {
                UpdateFlag = 3, // 3 = leave
                Info = new SCHARACTER_GATHERING_HALL_INFO
                {
                    AccountId = client.m_currentPlayer.AccountID.ToString(),
                    NickName = new SACCOUNT_NICKNAME()
                    {
                        OriginalNickName = client.m_currentCharacter.Nickname,
                        StreamingModeNickName = client.m_currentCharacter.Nickname,
                        KarmaRating = client.m_currentCharacter.KarmaScore
                    },
                    CharacterClass = client.m_currentCharacter.Class,
                    CharacterId = client.m_currentCharacter.CharID,
                    Gender = (uint)client.m_currentCharacter.Gender,
                    Level = (uint)client.m_currentCharacter.Level
                }
            };

            clientUpdateNot.Updates.Add(leaveUpdate);

            foreach (var clientSession in CurrentUsers)
            {
                var joinNot = new WrapperSerializer<SS2C_GATHERING_HALL_CHANNEL_USER_UPDATE_NOT>(
                    clientUpdateNot,
                    clientSession.m_currentPacketSequence++,
                    PacketCommand.S2CGatheringHallChannelUserUpdateNot);

                clientSession.SendAsync(joinNot.Serialize().ToArray());
            }
        }

        public bool IsMember(ClientSession client)
        {
            return CurrentUsers.Contains(client);
        }

        public void AddMessage(ChatMessage message)
        {
            Messages.Add(message);

            var chatMessageNot = ChatMessage.CreateNotPacket(message);

            foreach (var clientSession in CurrentUsers)
            {
                var chatNot = new WrapperSerializer<SS2C_GATHERING_HALL_CHANNEL_CHAT_NOT>(
                        chatMessageNot,
                        clientSession.m_currentPacketSequence++,
                        PacketCommand.S2CGatheringHallChannelChatNot);

                clientSession.SendAsync(chatNot.Serialize().ToArray());
            }
        }
    }
}