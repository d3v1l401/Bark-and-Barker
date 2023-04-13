namespace BarkAndBarker.Persistence.Models
{
    internal class ModelCharacterStatistics : IModel
    {
        public string CharID { get; set; } //PK, FK
        public int PlaytimeInMinutes { get; set; }
        public long ExperienceCollected { get; set; }
        public int GamesPlayed { get; set; }
        public int LootCollected { get; set; }
        public long ValueOfTreasureCollected { get; set; }
        public long DamageDonePvP { get; set; }
        public long DamageDonePvE { get; set; }
        public int KillsSolo { get; set; }
        public int KillsNormal { get; set; }
        public int KillsHighroller { get; set; }
        public int SuccessfulExtractionsSolo { get; set; }
        public int SuccessfulExtractionsNormal { get; set; }
        public int SuccessfulExtractionsHighroller { get; set; }
        public int NpcKilled { get; set; }
        public int LichKilled { get; set; }
        public int GhostKingKilled { get; set; }

        public static readonly string QueryCreateTable = @"CREATE TABLE IF NOT EXISTS character_statistics (
                                                `CharID` VARCHAR(45),
                                                `PlaytimeInMinutes` INT NOT NULL DEFAULT 0,
                                                `ExperienceCollected` BIGINT NOT NULL DEFAULT 0,
                                                `GamesPlayed` INT NOT NULL DEFAULT 0,
                                                `LootCollected` INT NOT NULL DEFAULT 0,
                                                `ValueOfTreasureCollected` BIGINT NOT NULL DEFAULT 0,
                                                `DamageDonePvP` BIGINT NOT NULL DEFAULT 0,
                                                `DamageDonePvE` BIGINT NOT NULL DEFAULT 0,
                                                `KillsSolo` INT NOT NULL DEFAULT 0,
                                                `KillsNormal` INT NOT NULL DEFAULT 0,
                                                `KillsHighroller` INT NOT NULL DEFAULT 0,
                                                `SuccessfulExtractionsSolo` INT NOT NULL DEFAULT 0,
                                                `SuccessfulExtractionsNormal` INT NOT NULL DEFAULT 0,
                                                `SuccessfulExtractionsHighroller` INT NOT NULL DEFAULT 0,
                                                `NpcKilled` INT NOT NULL DEFAULT 0,
                                                `LichKilled` INT NOT NULL DEFAULT 0,
                                                `GhostKingKilled` INT NOT NULL DEFAULT 0,
                                                PRIMARY KEY (`CharID`),
                                                CONSTRAINT `realChar` FOREIGN KEY (`CharID`) REFERENCES `characters` (`CharID`)
                                                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;";

        public static readonly int TableCreationOrder = 1;
    }
}