using BarkAndBarker.Shared.Persistence.Models.CharacterStatistics;

namespace BarkAndBarker.Shared.Ranking
{
    public class TopRankings
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
}