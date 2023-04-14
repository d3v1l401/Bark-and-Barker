using FluentScheduler;

namespace BarkAndBarker.Jobs
{
    internal class FetchRankingJob : IJob
    {
        public void Execute()
        {
            Console.WriteLine("Ich war noch niemals in New York");
        }
    }
}
