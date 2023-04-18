using BarkAndBarker.Shared.Persistence;
using BarkAndBarker.Shared.Persistence.Models;
using BarkAndBarker.Shared.Persistence.Models.CharacterStatistics;
using FluentScheduler;
using System.Linq;
using BarkAndBarker.Shared.Ranking;

namespace BarkAndBarker.RankingCalculator.Jobs
{
    internal class UpdateRankingJob : IJob
    {
        public class MappedStatistics
        {
            public ModelCharacterStatistics CharacterStatistics { get; set; }
            public ModelCharacter Character { get; set; }

            public MappedStatistics(ModelCharacterStatistics characterStatistics, ModelCharacter character)
            {
                this.CharacterStatistics = characterStatistics;
                this.Character = character;
            }
        }

        public void Execute()
        {
            var database = new Database();
            database.Connect();
            if (!database.IsConnected())
            {
                Console.WriteLine(nameof(UpdateRankingJob) + "> Could not connect to the database, please fix.");
                return;
            }

            var mappedStatistics = GetMappedStatistics(database);
            var rankings = CalculateCharacterRankings(mappedStatistics, database);
            var topRankings = CalculateTopRankings(rankings, database);

            database.Execute(ModelCharacterRankingTop.QueryResetTable, null);
            database.Execute(ModelCharacterRanking.QueryResetTable, null);

            var characterRankingValues = new List<string>();
            var characterRankingParameters = new Dictionary<string, object>();
            for (int i = 0; i < rankings.Count; i++)
            {
                var ranking = rankings[i];
                characterRankingValues.Add($"(@CharID{i}, @ClassName{i}, @VeteranAdventureCount{i}, @TreasureCollectorCount{i}, @KillerOutlawCount{i}, @EscapeArtistCount{i}, @LichSlayerCount{i}, @GhostKingSlayerCount{i})");

                characterRankingParameters.Add($"@CharID{i}", ranking.CharID);
                characterRankingParameters.Add($"@ClassName{i}", ranking.ClassName);
                characterRankingParameters.Add($"@VeteranAdventureCount{i}", ranking.VeteranAdventureCount);
                characterRankingParameters.Add($"@TreasureCollectorCount{i}", ranking.TreasureCollectorCount);
                characterRankingParameters.Add($"@KillerOutlawCount{i}", ranking.KillerOutlawCount);
                characterRankingParameters.Add($"@EscapeArtistCount{i}", ranking.EscapeArtistCount);
                characterRankingParameters.Add($"@LichSlayerCount{i}", ranking.LichSlayerCount);
                characterRankingParameters.Add($"@GhostKingSlayerCount{i}", ranking.GhostKingSlayerCount);
            }
            var populateRankingQuery = ModelCharacterRanking.QueryPopulate;
            populateRankingQuery += string.Join(", ", characterRankingValues);

            if(characterRankingParameters.Count > 0)
                database.Execute(populateRankingQuery, characterRankingParameters);


            var characterRankingTopValues = new List<string>();
            var characterRankingTopParameters = new Dictionary<string, object>();
            var topRankingsList = topRankings.GetAll.ToList();
            for (int i = 0; i < topRankingsList.Count; i++)
            {
                var ranking = topRankingsList[i];
                characterRankingTopValues.Add($"(@CharID{i}, @AccountID{i}, @Nickname{i}, @ClassType{i}, @RankType{i}, @Rank{i}, @Score{i})");

                characterRankingTopParameters.Add($"@CharID{i}", ranking.CharID);
                characterRankingTopParameters.Add($"@AccountID{i}", ranking.AccountID);
                characterRankingTopParameters.Add($"@Nickname{i}", ranking.Nickname);
                characterRankingTopParameters.Add($"@ClassType{i}", ranking.ClassType);
                characterRankingTopParameters.Add($"@RankType{i}", ranking.RankType);
                characterRankingTopParameters.Add($"@Rank{i}", ranking.Rank);
                characterRankingTopParameters.Add($"@Score{i}", ranking.Score);
            }

            var populateRankingTopQuery = ModelCharacterRankingTop.QueryPopulate;
            populateRankingTopQuery += string.Join(", ", characterRankingTopValues);

            if(characterRankingTopParameters.Count > 0)
                database.Execute(populateRankingTopQuery, characterRankingTopParameters);
        }

        private List<MappedStatistics> GetMappedStatistics(Database database)
        {
            var mappedStatistics = new List<MappedStatistics>();

            foreach (var charStatistics in database.Select<ModelCharacterStatistics>(
                         ModelCharacterStatistics.QuerySelectAll, null))
            {
                var character = database.Select<ModelCharacter>(ModelCharacter.QuerySelectCharacterByID,
                    new { CID = charStatistics.CharID });

                mappedStatistics.Add(new MappedStatistics(charStatistics, character.First()));
            }

            return mappedStatistics;
        }

        private List<ModelCharacterRanking> CalculateCharacterRankings(List<MappedStatistics> mappedStatistics,
            Database database)
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
                                                     mappedStatistic.CharacterStatistics
                                                         .SuccessfulExtractionsNormal +
                                                     mappedStatistic.CharacterStatistics
                                                         .SuccessfulExtractionsHighroller;
                characterRanking.LichSlayerCount = mappedStatistic.CharacterStatistics.LichKilled;
                characterRanking.GhostKingSlayerCount = mappedStatistic.CharacterStatistics.GhostKingKilled;

                characterRankings.Add(characterRanking);
            }

            return characterRankings;
        }

        private TopRankings CalculateTopRankings(List<ModelCharacterRanking> characterRankings, Database database)
        {
            var topRankings = new TopRankings();
            
            var classTypes = Enum.GetValues<ClassType>();

            foreach (var classType in classTypes)
            {
                var filteredRankings = FilterRankingsByClassType(characterRankings, classType);

                topRankings.RankingVeteranAdventure.AddRankingsForClass(filteredRankings, f => f.VeteranAdventureCount, classType, RankType.VeteranAdventureCount);
                topRankings.RankingTreasureCollector.AddRankingsForClass(filteredRankings, f => f.TreasureCollectorCount, classType, RankType.TreasureCollectorCount);
                topRankings.RankingKillerOutlaw.AddRankingsForClass(filteredRankings, f => f.KillerOutlawCount, classType, RankType.KillerOutlawCount);
                topRankings.RankingEscapeArtist.AddRankingsForClass(filteredRankings, f => f.EscapeArtistCount, classType, RankType.EscapeArtistCount);
                topRankings.RankingLichSlayer.AddRankingsForClass(filteredRankings, f => f.LichSlayerCount, classType, RankType.LichSlayerCount);
                topRankings.RankingGhostKingSlayer.AddRankingsForClass(filteredRankings, f => f.GhostKingSlayerCount, classType, RankType.GhostKingSlayerCount);
            }

            var accounts = database.Select<ModelAccount>(ModelAccount.QuerySelectAllAccounts, null);
            var characters = database.Select<ModelCharacter>(ModelCharacter.QuerySelectAllCharacters, null);

            foreach (var modelCharacterRankingTop in topRankings.GetAll)
            {
                var character = characters.First(c => c.CharID == modelCharacterRankingTop.CharID);
                modelCharacterRankingTop.Nickname = character .Nickname;
                modelCharacterRankingTop.AccountID = accounts.First(a => a.ID == character.accountID).ID;
            }

            return topRankings;
        }

        private IEnumerable<ModelCharacterRanking> FilterRankingsByClassType(List<ModelCharacterRanking> characterRankings, ClassType classType)
        {
            if (classType == ClassType.All)
                return characterRankings;

            var className = $"DesignDataPlayerCharacter:Id_PlayerCharacter_{classType}";
            return characterRankings.Where(c => c.ClassName == className);
        }
    }
}