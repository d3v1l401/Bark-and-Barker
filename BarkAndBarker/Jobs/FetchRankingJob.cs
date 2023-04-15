using BarkAndBarker.Ranking;
using BarkAndBarker.Shared.Persistence;
using BarkAndBarker.Shared.Persistence.Models.CharacterStatistics;
using BarkAndBarker.Shared.Ranking;
using FluentScheduler;

namespace BarkAndBarker.Jobs
{
    internal class FetchRankingJob : IJob
    {
        public void Execute()
        {
            var database = new Database();
            database.Connect();
            if (!database.IsConnected())
            {
                Console.WriteLine(nameof(FetchRankingJob) + "> Could not connect to the database, please fix.");
                return;
            }

            var topRankings = GetTopRankings(database);
            RankingCache.Update(topRankings);
        }

        private TopRankings GetTopRankings(Database database)
        {
            var topRankings = new TopRankings();

            var unfilteredRanking = GetUnfilteredTopRankingsFromDatabase(database);

            foreach (var modelCharacterRankingTop in unfilteredRanking)
            {
                switch (modelCharacterRankingTop.RankType)
                {
                    case RankType.VeteranAdventureCount:
                        switch (modelCharacterRankingTop.ClassType)
                        {
                            case ClassType.Fighter:
                                topRankings.RankingVeteranAdventure.RankingFighter.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Barbarian:
                                topRankings.RankingVeteranAdventure.RankingBarbarian.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Cleric:
                                topRankings.RankingVeteranAdventure.RankingCleric.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Rogue:
                                topRankings.RankingVeteranAdventure.RankingRogue.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Ranger:
                                topRankings.RankingVeteranAdventure.RankingRanger.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Wizard:
                                topRankings.RankingVeteranAdventure.RankingWizard.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.All:
                                topRankings.RankingVeteranAdventure.RankingAll.Add(modelCharacterRankingTop);
                                break;
                        }

                        break;
                    case RankType.TreasureCollectorCount:
                        switch (modelCharacterRankingTop.ClassType)
                        {
                            case ClassType.Fighter:
                                topRankings.RankingTreasureCollector.RankingFighter.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Barbarian:
                                topRankings.RankingTreasureCollector.RankingBarbarian.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Cleric:
                                topRankings.RankingTreasureCollector.RankingCleric.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Rogue:
                                topRankings.RankingTreasureCollector.RankingRogue.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Ranger:
                                topRankings.RankingTreasureCollector.RankingRanger.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Wizard:
                                topRankings.RankingTreasureCollector.RankingWizard.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.All:
                                topRankings.RankingTreasureCollector.RankingAll.Add(modelCharacterRankingTop);
                                break;
                        }

                        break;
                    case RankType.KillerOutlawCount:
                        switch (modelCharacterRankingTop.ClassType)
                        {
                            case ClassType.Fighter:
                                topRankings.RankingKillerOutlaw.RankingFighter.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Barbarian:
                                topRankings.RankingKillerOutlaw.RankingBarbarian.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Cleric:
                                topRankings.RankingKillerOutlaw.RankingCleric.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Rogue:
                                topRankings.RankingKillerOutlaw.RankingRogue.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Ranger:
                                topRankings.RankingKillerOutlaw.RankingRanger.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Wizard:
                                topRankings.RankingKillerOutlaw.RankingWizard.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.All:
                                topRankings.RankingKillerOutlaw.RankingAll.Add(modelCharacterRankingTop);
                                break;
                        }
                        break;
                    case RankType.EscapeArtistCount:
                        switch (modelCharacterRankingTop.ClassType)
                        {
                            case ClassType.Fighter:
                                topRankings.RankingEscapeArtist.RankingFighter.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Barbarian:
                                topRankings.RankingEscapeArtist.RankingBarbarian.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Cleric:
                                topRankings.RankingEscapeArtist.RankingCleric.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Rogue:
                                topRankings.RankingEscapeArtist.RankingRogue.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Ranger:
                                topRankings.RankingEscapeArtist.RankingRanger.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Wizard:
                                topRankings.RankingEscapeArtist.RankingWizard.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.All:
                                topRankings.RankingEscapeArtist.RankingAll.Add(modelCharacterRankingTop);
                                break;
                        }
                        break;
                    case RankType.LichSlayerCount:
                        switch (modelCharacterRankingTop.ClassType)
                        {
                            case ClassType.Fighter:
                                topRankings.RankingLichSlayer.RankingFighter.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Barbarian:
                                topRankings.RankingLichSlayer.RankingBarbarian.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Cleric:
                                topRankings.RankingLichSlayer.RankingCleric.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Rogue:
                                topRankings.RankingLichSlayer.RankingRogue.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Ranger:
                                topRankings.RankingLichSlayer.RankingRanger.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Wizard:
                                topRankings.RankingLichSlayer.RankingWizard.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.All:
                                topRankings.RankingLichSlayer.RankingAll.Add(modelCharacterRankingTop);
                                break;
                        }
                        break;
                    case RankType.GhostKingSlayerCount:
                        switch (modelCharacterRankingTop.ClassType)
                        {
                            case ClassType.Fighter:
                                topRankings.RankingGhostKingSlayer.RankingFighter.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Barbarian:
                                topRankings.RankingGhostKingSlayer.RankingBarbarian.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Cleric:
                                topRankings.RankingGhostKingSlayer.RankingCleric.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Rogue:
                                topRankings.RankingGhostKingSlayer.RankingRogue.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Ranger:
                                topRankings.RankingGhostKingSlayer.RankingRanger.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.Wizard:
                                topRankings.RankingGhostKingSlayer.RankingWizard.Add(modelCharacterRankingTop);
                                break;
                            case ClassType.All:
                                topRankings.RankingGhostKingSlayer.RankingAll.Add(modelCharacterRankingTop);
                                break;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return topRankings;
        }

        private List<ModelCharacterRankingTop> GetUnfilteredTopRankingsFromDatabase(Database database)
        {
            var topRankings = database.Select<ModelCharacterRankingTop>(
                ModelCharacterRankingTop.QuerySelectAll, null);

            return topRankings.ToList();
        }
    }
}
