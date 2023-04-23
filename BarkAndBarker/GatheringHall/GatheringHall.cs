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

        private List<ClientSession> CurrentUsers { get; set; }

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
            CurrentUsers.Add(client);
        }

        public void Leave(ClientSession client)
        {
            CurrentUsers.Remove(client);
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