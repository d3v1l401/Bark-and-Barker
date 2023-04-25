namespace BarkAndBarker.GatheringHall
{
    internal static class GatheringHallManager
    {
        public static List<GatheringHall> GatheringHalls = new List<GatheringHall>();

        public static void Init()
        {
            GatheringHalls.Add(new GatheringHall("ChatRoomData:Id_ChatRoom_GatheringHall_EU_Central_Frankfurt", 1, 1));
            GatheringHalls.Add(new GatheringHall("Cooler Channel", 2, 1));
            GatheringHalls.Add(new GatheringHall("Why", 3, 1));
            GatheringHalls.Add(new GatheringHall("So", 4, 1));
            GatheringHalls.Add(new GatheringHall("Serious", 5, 1));
        }


        public static bool Join(ClientSession client, uint gatheringHallIndex)
        {
            foreach (var gatheringHall in GatheringHalls)
            {
                if (gatheringHall.ChannelIndex == gatheringHallIndex)
                {
                    gatheringHall.Join(client);
                    return true;
                }
            }

            return false;
        }

        public static bool Leave(ClientSession client)
        {
            foreach (var gatheringHall in GatheringHalls)
            {
                if (gatheringHall.IsMember(client))
                {
                    gatheringHall.Leave(client);
                    return true;
                }
            }

            return false;
        }

        public static void AddMessage(ChatMessage message, ClientSession client)
        {
            foreach (var gatheringHall in GatheringHalls)
            {
                if (gatheringHall.IsMember(client))
                {
                    gatheringHall.AddMessage(message);
                }
            }
        }

        public static List<ClientSession> GetUserList(ClientSession client)
        {
            foreach (var gatheringHall in GatheringHalls)
            {
                if (gatheringHall.IsMember(client))
                {
                    return gatheringHall.CurrentUsers;
                }
            }

            return Enumerable.Empty<ClientSession>().ToList();
        }
    }
}
