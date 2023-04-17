using BarkAndBarker.RankingCalculator.Jobs;
using BarkAndBarker.Shared.Persistence;
using BarkAndBarker.Shared.Settings;
using FluentScheduler;

Console.WriteLine("Bark!");

#if DEBUG
Console.WriteLine("Importing settings from 'settings.json'...");
var settings = Settings.ImportSettings("./settings.json");
#else
Console.WriteLine("Importing settings from environment...");
var settings = Settings.ImportSettings();
#endif
if (settings == null)
{
    Console.WriteLine("Could not import settings.");
    return;
}

Console.WriteLine("Connecting to database...");
Database.ConnectionString = settings.DBConnectionString;

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