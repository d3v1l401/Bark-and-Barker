using FluentScheduler;

namespace BarkAndBarker.Jobs
{
    internal class JobController
    {
        public void Init()
        {
            var jobRegistry = new SchedulerRegistry();

            JobManager.JobException += info => Console.WriteLine("An error just happened with a scheduled job: " + info.Exception);
            JobManager.JobStart += info => Console.WriteLine($"{info.Name}: started");
            JobManager.JobEnd += info => Console.WriteLine($"{info.Name}: ended ({info.Duration})");

            JobManager.Initialize(jobRegistry);
        }
    }
}
