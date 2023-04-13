namespace BarkAndBarker.Persistence.Models.CharacterStatistics
{
    internal class ModelCharacterRankingTop : IModel
    {
        public int ID { get; set; } //PK
        public int CharacterRankingID { get; set; } //FK
        public RankType RankType { get; set; }
        public int Rank { get; set; }


        public static readonly string QueryCreateTable = $@"CREATE TABLE IF NOT EXISTS character_ranking_top (
                                                `{nameof(ID)}` int,
                                                `{nameof(CharacterRankingID)}` int,
                                                `{nameof(RankType)}` int,
                                                `{nameof(Rank)}` int,

                                                PRIMARY KEY (`{nameof(ID)}`),
                                                CONSTRAINT `charRankingId` FOREIGN KEY (`{nameof(CharacterRankingID)}`) REFERENCES `character_ranking` (`{nameof(ModelCharacterRanking.ID)}`)
                                                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;";

        public static readonly int TableCreationOrder = 1;
    }

    internal enum RankType : int
    {
        All,
        Fighter,
        Barbarian,
        Cleric,
        Ranger,
        Rogue,
        Wizard
    }
}
