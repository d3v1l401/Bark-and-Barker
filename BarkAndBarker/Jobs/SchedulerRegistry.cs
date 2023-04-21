using FluentScheduler;

namespace BarkAndBarker.Jobs
{
    internal class SchedulerRegistry : Registry
    {
        //TODO Change interval later if db grows bigger / on productive use
        private static readonly int FetchRankingJobIntervalInMinutes = 1;

        public SchedulerRegistry()
        {
            Schedule<FetchRankingJob>().NonReentrant().ToRunNow().AndEvery(FetchRankingJobIntervalInMinutes).Minutes();
        }
    }
}
