using BarkAndBarker.Shared.Persistence.Models;

namespace BarkAndBarker.GatheringHall
{
    internal class GatheringHall
    {
        public string ChannelId { get; set; }
        public uint ChannelIndex { get; set; }
        public uint GroupIndex { get; set; }
        public uint MemberCount => (uint)CurrentUsers.Count;

        private List<ClientSession> CurrentUsers { get; set; }

        public GatheringHall(string channelId, uint channelIndex, uint groupIndex)
        {
            ChannelId = channelId;
            ChannelIndex = channelIndex;
            GroupIndex = groupIndex;

            CurrentUsers = new List<ClientSession>();
        }

        public void Join(ClientSession client)
        {
            CurrentUsers.Add(client);
        }
    }
}
