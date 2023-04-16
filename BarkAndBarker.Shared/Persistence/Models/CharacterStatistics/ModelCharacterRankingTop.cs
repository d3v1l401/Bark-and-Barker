namespace BarkAndBarker.Shared.Persistence.Models.CharacterStatistics
{
    public class ModelCharacterRankingTop : IModel
    {
        public int ID { get; set; } //PK
        public string CharID { get; set; } //FK
        public int AccountID { get; set; } //FK
        public string Nickname { get; set; }
        public ClassType ClassType { get; set; }
        public RankType RankType { get; set; }
        public int Rank { get; set; }
        public int Score { get; set; }

        public static readonly string QueryResetTable = @"DELETE FROM character_ranking_top";
        public static readonly string QueryPopulate = @"INSERT INTO character_ranking_top
                (CharID, AccountID, Nickname, ClassType, RankType, `Rank`, Score)
                VALUES ";

        public static readonly string QuerySelectAll = "SELECT * FROM barker.character_ranking_top;";

        public static readonly string QueryCreateTable = $@"CREATE TABLE IF NOT EXISTS character_ranking_top (
                                                `{nameof(ID)}` int AUTO_INCREMENT,
                                                `{nameof(CharID)}` VARCHAR(45),
                                                `{nameof(AccountID)}` int,
                                                `{nameof(Nickname)}` VARCHAR(20),
                                                `{nameof(ClassType)}` int,
                                                `{nameof(RankType)}` int,
                                                `{nameof(Rank)}` int,
                                                `{nameof(Score)}` int,

                                                PRIMARY KEY (`{nameof(ID)}`),
                                                CONSTRAINT `charRankingId` FOREIGN KEY (`{nameof(CharID)}`) REFERENCES `characters` (`{nameof(ModelCharacter.CharID)}`),
                                                CONSTRAINT `accRankingId` FOREIGN KEY (`{nameof(AccountID)}`) REFERENCES `accounts` (`{nameof(ModelAccount.ID)}`)
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
        Wizard,
        Bard
    }

    public enum RankType : int
    {
        VeteranAdventureCount = 1,
        TreasureCollectorCount,
        KillerOutlawCount,
        EscapeArtistCount,
        LichSlayerCount,
        GhostKingSlayerCount
    }
}