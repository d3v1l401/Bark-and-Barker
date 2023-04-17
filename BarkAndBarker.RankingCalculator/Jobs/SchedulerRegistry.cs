using FluentScheduler;

namespace BarkAndBarker.RankingCalculator.Jobs
{
    internal class SchedulerRegistry : Registry
    {
        //TODO Change interval later if db grows bigger / on productive use
        private static readonly int UpdateRankingJobIntervalInMinutes = 1;

        public SchedulerRegistry()
        {
            Schedule<UpdateRankingJob>().NonReentrant().ToRunNow().AndEvery(UpdateRankingJobIntervalInMinutes).Minutes();
        }
    }
}
