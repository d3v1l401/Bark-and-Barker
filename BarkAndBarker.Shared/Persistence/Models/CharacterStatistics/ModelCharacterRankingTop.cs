namespace BarkAndBarker.Shared.Persistence.Models.CharacterStatistics
{
    public class ModelCharacterRankingTop : IModel
    {
        public int ID { get; set; } //PK
        public string CharID { get; set; } //FK
        public ClassType ClassType { get; set; }
        public RankType RankType { get; set; }
        public int Rank { get; set; }

        public static readonly string QueryResetTable = @"DELETE FROM character_ranking_top";
        public static readonly string QueryPopulate = @"INSERT INTO character_ranking_top
                (CharID, ClassType, RankType, `Rank`)
                VALUES ";

        public static readonly string QuerySelectAll = "SELECT * FROM barker.character_ranking_top;";

        public static readonly string QueryCreateTable = $@"CREATE TABLE IF NOT EXISTS character_ranking_top (
                                                `{nameof(ID)}` int AUTO_INCREMENT,
                                                `{nameof(CharID)}` VARCHAR(45),
                                                `{nameof(ClassType)}` int,
                                                `{nameof(RankType)}` int,
                                                `{nameof(Rank)}` int,

                                                PRIMARY KEY (`{nameof(ID)}`),
                                                CONSTRAINT `charRankingId` FOREIGN KEY (`{nameof(CharID)}`) REFERENCES `character_ranking` (`{nameof(ModelCharacterRanking.CharID)}`)
                                                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;";

        public static readonly int TableCreationOrder = 1;
    }

    public enum ClassType : int
    {
        All,
        Fighter,
        Barbarian,
        Cleric,
        Ranger,
        Rogue,
        Wizard
    }

    public enum RankType : int
    {
        VeteranAdventureCount,
        TreasureCollectorCount,
        KillerOutlawCount,
        EscapeArtistCount,
        LichSlayerCount,
        GhostKingSlayerCount
    }
}