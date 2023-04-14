using BarkAndBarker.Shared.Persistence;
using BarkAndBarker.Shared.Persistence.Models;
using BarkAndBarker.Shared.Persistence.Models.CharacterStatistics;
using FluentScheduler;

namespace BarkAndBarker.RankingCalculator.Jobs
{
    internal class FetchRankingJob : IJob
    {
        private struct MappedStatistics
        {
            public ModelCharacterStatistics CharacterStatistics { get; set; }
            public ModelCharacter Character { get; set; }

            public MappedStatistics(ModelCharacterStatistics characterStatistics, ModelCharacter character)
            {
                this.CharacterStatistics = characterStatistics;
                this.Character = character;
            }
        }

        private struct TopRankings
        {
            public TopRankingsOfType RankingVeteranAdventure { get; set; }
            public TopRankingsOfType RankingTreasureCollector { get; set; }
            public TopRankingsOfType RankingKillerOutlaw { get; set; }
            public TopRankingsOfType RankingEscapeArtist { get; set; }
            public TopRankingsOfType RankingLichSlayer { get; set; }
            public TopRankingsOfType RankingGhostKingSlayer { get; set; }
        }

        private struct TopRankingsOfType
        {
            public RankType RankType { get; set; }

            public List<ModelCharacterRankingTop> RankingAll { get; set; }
            public List<ModelCharacterRankingTop> RankingFighter { get; set; }
            public List<ModelCharacterRankingTop> RankingBarbarian { get; set; }
            public List<ModelCharacterRankingTop> RankingCleric { get; set; }
            public List<ModelCharacterRankingTop> RankingRogue { get; set; }
            public List<ModelCharacterRankingTop> RankingRanger { get; set; }
            public List<ModelCharacterRankingTop> RankingWizard { get; set; }
        }

        public void Execute()
        {
            var database = new Database();
            database.Connect();
            if (!database.IsConnected())
            {
                Console.WriteLine(nameof(FetchRankingJob) +  "> Could not connect to the database, please fix.");
                return;
            }

            var mappedStatistics = GetMappedStatistics(database);
            var rankings = CalculateCharacterRankings(mappedStatistics, database);
            var topRankings = CalculateTopRankings(rankings, database);
        }

        private List<MappedStatistics> GetMappedStatistics(Database database)
        {
            var mappedStatistics = new List<MappedStatistics>();

            foreach (var charStatistics in database.Select<ModelCharacterStatistics>(ModelCharacterStatistics.QuerySelectAll, null))
            {
                var character = database.Select<ModelCharacter>(ModelCharacter.QuerySelectCharacterByID, new { CID = charStatistics.CharID });

                mappedStatistics.Add(new MappedStatistics(charStatistics, character.First()));
            }
            
            return mappedStatistics;
        }

        private List<ModelCharacterRanking> CalculateCharacterRankings(List<MappedStatistics> mappedStatistics, Database database)
        {
            var characterRankings = new List<ModelCharacterRanking>();

            foreach (var mappedStatistic in mappedStatistics)
            {
                var characterRanking = new ModelCharacterRanking();
                characterRanking.CharID = mappedStatistic.CharacterStatistics.CharID;
                characterRanking.ClassName = mappedStatistic.Character.Class;
                characterRanking.VeteranAdventureCount = 0; //TODO
                characterRanking.TreasureCollectorCount = 0; //TODO
                characterRanking.KillerOutlawCount = mappedStatistic.CharacterStatistics.KillsHighroller;
                characterRanking.EscapeArtistCount = mappedStatistic.CharacterStatistics.SuccessfulExtractionsSolo +
                                                     mappedStatistic.CharacterStatistics.SuccessfulExtractionsNormal +
                                                     mappedStatistic.CharacterStatistics.SuccessfulExtractionsHighroller;
                characterRanking.LichSlayerCount = mappedStatistic.CharacterStatistics.LichKilled;
                characterRanking.GhostKingSlayerCount = mappedStatistic.CharacterStatistics.GhostKingKilled;

                characterRankings.Add(characterRanking);
            }

            return characterRankings;
        }

        private TopRankings CalculateTopRankings(List<ModelCharacterRanking> characterRankings, Database database)
        {
            var topRankings = new TopRankings();

            var bestFighters = characterRankings.Where(c => c.ClassName == "DesignDataPlayerCharacter:Id_PlayerCharacter_Fighter");

            return topRankings;
        }
    }
}
