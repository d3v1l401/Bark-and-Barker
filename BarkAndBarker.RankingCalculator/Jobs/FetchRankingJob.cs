using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;

namespace BarkAndBarker.RankingCalculator.Jobs
{
    internal class FetchRankingJob : IJob
    {
        public void Execute()
        {
            Console.WriteLine("Griechischer Wein");
        }
    }
}
