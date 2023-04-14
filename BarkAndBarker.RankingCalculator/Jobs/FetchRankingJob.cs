using BarkAndBarker.Shared.Persistence;
using BarkAndBarker.Shared.Persistence.Models;
using BarkAndBarker.Shared.Persistence.Models.CharacterStatistics;
using FluentScheduler;
using System.Linq;

namespace BarkAndBarker.RankingCalculator.Jobs
{
    internal class FetchRankingJob : IJob
    {
        private class MappedStatistics
        {
            public ModelCharacterStatistics CharacterStatistics { get; set; }
            public ModelCharacter Character { get; set; }

            public MappedStatistics(ModelCharacterStatistics characterStatistics, ModelCharacter character)
            {
                this.CharacterStatistics = characterStatistics;
                this.Character = character;
            }
        }

        private class TopRankings
        {
            public TopRankingsOfType RankingVeteranAdventure { get; set; }
            public TopRankingsOfType RankingTreasureCollector { get; set; }
            public TopRankingsOfType RankingKillerOutlaw { get; set; }
            public TopRankingsOfType RankingEscapeArtist { get; set; }
            public TopRankingsOfType RankingLichSlayer { get; set; }
            public TopRankingsOfType RankingGhostKingSlayer { get; set; }
            public IEnumerable<ModelCharacterRankingTop> GetAll =>
                RankingVeteranAdventure.GetAll.Concat(RankingTreasureCollector.GetAll)
                    .Concat(RankingKillerOutlaw.GetAll)
                    .Concat(RankingEscapeArtist.GetAll)
                    .Concat(RankingLichSlayer.GetAll)
                    .Concat(RankingGhostKingSlayer.GetAll);

            public TopRankings()
            {
                RankingVeteranAdventure = new TopRankingsOfType(RankType.VeteranAdventureCount);
                RankingTreasureCollector = new TopRankingsOfType(RankType.TreasureCollectorCount);
                RankingKillerOutlaw = new TopRankingsOfType(RankType.KillerOutlawCount);
                RankingEscapeArtist = new TopRankingsOfType(RankType.EscapeArtistCount);
                RankingLichSlayer = new TopRankingsOfType(RankType.LichSlayerCount);
                RankingGhostKingSlayer = new TopRankingsOfType(RankType.GhostKingSlayerCount);
            }
        }

        private class TopRankingsOfType
        {
            public RankType RankType { get; set; }

            public List<ModelCharacterRankingTop> RankingAll { get; set; }
            public List<ModelCharacterRankingTop> RankingFighter { get; set; }
            public List<ModelCharacterRankingTop> RankingBarbarian { get; set; }
            public List<ModelCharacterRankingTop> RankingCleric { get; set; }
            public List<ModelCharacterRankingTop> RankingRogue { get; set; }
            public List<ModelCharacterRankingTop> RankingRanger { get; set; }
            public List<ModelCharacterRankingTop> RankingWizard { get; set; }

            public IEnumerable<ModelCharacterRankingTop> GetAll =>
                RankingAll.Concat(RankingFighter)
                    .Concat(RankingBarbarian)
                    .Concat(RankingCleric)
                    .Concat(RankingRogue)
                    .Concat(RankingRanger)
                    .Concat(RankingWizard);

            public TopRankingsOfType(RankType rankType)
            {
                this.RankType = rankType;
                RankingAll = new List<ModelCharacterRankingTop>();
                RankingFighter = new List<ModelCharacterRankingTop>();
                RankingBarbarian = new List<ModelCharacterRankingTop>();
                RankingCleric = new List<ModelCharacterRankingTop>();
                RankingRogue = new List<ModelCharacterRankingTop>();
                RankingRanger = new List<ModelCharacterRankingTop>();
                RankingWizard = new List<ModelCharacterRankingTop>();
            }

            public void AddRankingsForClass(IEnumerable<ModelCharacterRanking> characterRankings,
                Func<ModelCharacterRanking, int> selector, ClassType classType, RankType rankType)
            {
                List<ModelCharacterRankingTop> rankings;

                switch (classType)
                {
                    case ClassType.Fighter:
                        rankings = RankingFighter;
                        break;
                    case ClassType.Barbarian:
                        rankings = RankingBarbarian;
                        break;
                    case ClassType.Cleric:
                        rankings = RankingCleric;
                        break;
                    case ClassType.Rogue:
                        rankings = RankingRogue;
                        break;
                    case ClassType.Ranger:
                        rankings = RankingRanger;
                        break;
                    case ClassType.Wizard:
                        rankings = RankingWizard;
                        break;
                    case ClassType.All:
                        rankings = RankingAll;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(classType), classType, "Invalid class type");
                }

                rankings.AddRange(characterRankings.OrderByDescending(selector)
                    .Take(100)
                    .Select((character, i) => new ModelCharacterRankingTop
                    {
                        CharID = character.CharID,
                        ClassType = classType,
                        RankType = rankType,
                        Rank = i+1
                    }));
            }
        }

        public void Execute()
        {
            var database = new Database();
            database.Connect();
            if (!database.IsConnected())
            {
                Console.WriteLine(nameof(FetchRankingJob) + "> Could not connect to the database, please fix.");
                return;
            }

            var mappedStatistics = GetMappedStatistics(database);
            var rankings = CalculateCharacterRankings(mappedStatistics, database);
            var topRankings = CalculateTopRankings(rankings, database);

            database.Execute(ModelCharacterRanking.QueryResetTable, null);
            database.Execute(ModelCharacterRankingTop.QueryResetTable, null);

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
                characterRankingTopValues.Add($"(@CharID{i}, @ClassType{i}, @RankType{i}, @Rank{i})");

                characterRankingTopParameters.Add($"@CharID{i}", ranking.CharID);
                characterRankingTopParameters.Add($"@ClassType{i}", ranking.ClassType);
                characterRankingTopParameters.Add($"@RankType{i}", ranking.RankType);
                characterRankingTopParameters.Add($"@Rank{i}", ranking.Rank);
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

            var classTypes = new List<ClassType> { ClassType.Fighter, ClassType.Barbarian, ClassType.Cleric, ClassType.Rogue, ClassType.Ranger, ClassType.Wizard, ClassType.All };

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