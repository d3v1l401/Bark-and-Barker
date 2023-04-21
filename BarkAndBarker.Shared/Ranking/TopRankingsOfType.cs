using BarkAndBarker.Shared.Persistence.Models.CharacterStatistics;

namespace BarkAndBarker.Shared.Ranking
{
    public class TopRankingsOfType
    {
        public RankType RankType { get; set; }

        public List<ModelCharacterRankingTop> RankingAll { get; set; }
        public List<ModelCharacterRankingTop> RankingFighter { get; set; }
        public List<ModelCharacterRankingTop> RankingBarbarian { get; set; }
        public List<ModelCharacterRankingTop> RankingCleric { get; set; }
        public List<ModelCharacterRankingTop> RankingRogue { get; set; }
        public List<ModelCharacterRankingTop> RankingRanger { get; set; }
        public List<ModelCharacterRankingTop> RankingWizard { get; set; }
        public List<ModelCharacterRankingTop> RankingBard { get; set; }

        public IEnumerable<ModelCharacterRankingTop> GetAll =>
            RankingAll.Concat(RankingFighter)
                .Concat(RankingBarbarian)
                .Concat(RankingCleric)
                .Concat(RankingRogue)
                .Concat(RankingRanger)
                .Concat(RankingWizard)
                .Concat(RankingBard);

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
            RankingBard = new List<ModelCharacterRankingTop>();
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
                case ClassType.Bard:
                    rankings = RankingBard;
                    break;
                case ClassType.All:
                    rankings = RankingAll;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(classType), classType, "Invalid class type");
            }
            
            

            rankings.AddRange(characterRankings.OrderByDescending(selector)
                .Take(100)
                .Select((character, i) =>
                {
                    var score = 0;
                    switch (rankType)
                    {
                        case RankType.VeteranAdventureCount:
                            score = character.VeteranAdventureCount;
                            break;
                        case RankType.TreasureCollectorCount:
                            score = character.TreasureCollectorCount;
                            break;
                        case RankType.KillerOutlawCount:
                            score = character.KillerOutlawCount;
                            break;
                        case RankType.EscapeArtistCount:
                            score = character.EscapeArtistCount;
                            break;
                        case RankType.LichSlayerCount:
                            score = character.LichSlayerCount;
                            break;
                        case RankType.GhostKingSlayerCount:
                            score = character.GhostKingSlayerCount;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(rankType), rankType, null);
                    }
                    return new ModelCharacterRankingTop
                    {
                        CharID = character.CharID,
                        ClassType = classType,
                        RankType = rankType,
                        Rank = i + 1,
                        Score = score
                    };
                }));
        }
    }
}
