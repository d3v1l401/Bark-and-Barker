using FluentScheduler;

namespace BarkAndBarker.RankingCalculator.Jobs
{
    internal class SchedulerRegistry : Registry
    {
        public SchedulerRegistry()
        {
            //TODO Change interval later if db grows bigger / on productive use
            Schedule<FetchRankingJob>().NonReentrant().ToRunNow().AndEvery(1).Minutes();
        }
    }
}
