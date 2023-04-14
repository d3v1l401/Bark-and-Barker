using BarkAndBarker.RankingCalculator.Jobs;using BarkAndBarker.Shared.Persistence;
using FluentScheduler;

Console.WriteLine("Bark!");

Database.ConnectionString = @"Server=127.0.0.1;User=bark;Password=barkbark;Database=barker;"; //TODO

var jobRegistry = new SchedulerRegistry();

JobManager.JobException += info => Console.WriteLine("An error just happened with a scheduled job: " + info.Exception);
JobManager.JobStart += info => Console.WriteLine($"{info.Name}: started");
JobManager.JobEnd += info => Console.WriteLine($"{info.Name}: ended ({info.Duration})");

JobManager.Initialize(jobRegistry);


while (true)
{
    Console.WriteLine("Input command:");
    var command = Console.ReadLine().Split(' ');
    if(command.Length == 0) continue;

    if (command[0] == "exit") return;
}