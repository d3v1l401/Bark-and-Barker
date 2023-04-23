﻿namespace BarkAndBarker.GatheringHall
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
    }
}
