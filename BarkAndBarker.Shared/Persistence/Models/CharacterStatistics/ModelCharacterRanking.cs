namespace BarkAndBarker.Shared.Persistence.Models.CharacterStatistics
{
    public class ModelCharacterRanking : IModel
    {
        public int ID { get; set; } //PK
        public string CharID { get; set; } //FK
        public string ClassName { get; set; }
        public int VeteranAdventureCount { get; set; }
        public int TreasureCollectorCount { get; set; }
        public int KillerOutlawCount { get; set; }
        public int EscapeArtistCount { get; set; }
        public int LichSlayerCount { get; set; }
        public int GhostKingSlayerCount { get; set; }

        public static readonly string QueryResetTable = @"DELETE FROM character_ranking";

        public static readonly string QueryPopulate = @"INSERT INTO character_ranking
                (CharID, ClassName, VeteranAdventureCount, TreasureCollectorCount, KillerOutlawCount, EscapeArtistCount, LichSlayerCount, GhostKingSlayerCount)
                VALUES ";

        public static readonly string QueryCreateTable = $@"CREATE TABLE IF NOT EXISTS character_ranking (
                                                `{nameof(ID)}` int AUTO_INCREMENT,
                                                `{nameof(CharID)}` VARCHAR(45),
                                                `{nameof(ClassName)}` VARCHAR(255),
                                                `{nameof(VeteranAdventureCount)}` int,
                                                `{nameof(TreasureCollectorCount)}` int,
                                                `{nameof(KillerOutlawCount)}` int,
                                                `{nameof(EscapeArtistCount)}` int,
                                                `{nameof(LichSlayerCount)}` int,
                                                `{nameof(GhostKingSlayerCount)}` int,

                                                PRIMARY KEY (`{nameof(ID)}`),
                                                CONSTRAINT `rankingChar` FOREIGN KEY (`{nameof(CharID)}`) REFERENCES `characters` (`{nameof(ModelCharacter.CharID)}`)
                                                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;";

        public static readonly int TableCreationOrder = 2;
    }
}
