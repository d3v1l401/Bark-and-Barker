using BarkAndBarker.Shared.Ranking;

namespace BarkAndBarker.Ranking
{
    internal static class RankingCache
    {
        public static DateTime LastUpdateAt = DateTime.MinValue;

        public static TopRankings CachedTopRankings = new TopRankings();


        public static void Update(TopRankings newTopRankings)
        {
            LastUpdateAt = DateTime.Now;
            CachedTopRankings = newTopRankings;
        }
    }
}
