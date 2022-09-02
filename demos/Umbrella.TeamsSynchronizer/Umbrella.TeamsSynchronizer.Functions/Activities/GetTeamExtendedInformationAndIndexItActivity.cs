using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Umbrella.TeamsSynchronizer.Models;
using System.Diagnostics;

namespace Umbrella.TeamsSynchronizer.Functions.Activities;

public class GetTeamExtendedInformationAndIndexItActivity
{
    [FunctionName(nameof(GetTeamExtendedInformationAndIndexItActivity))]
    public async Task<SyncTeamDto> Run(
            [ActivityTrigger] IDurableActivityContext context,
            ILogger log)
    {
        log.LogInformation($"{nameof(GetTeamExtendedInformationAndIndexItActivity)}_Started. InstanceId: {context.InstanceId}");

        var stopWatch = new Stopwatch();
        stopWatch.Start();

        // Simulates getting information for that MS Teams that can be in external systems
        // and also to Index all the info gathered for the Team, into an Azure Search Index
        var teamWithGroup = context.GetInput<SyncTeamDto>();
        var delay = int.Parse(Environment.GetEnvironmentVariable("GetTeamExtendedInformationAndIndexItActivityDelayInMiliseconds") ?? "10");
        await Task.Delay(delay);

        teamWithGroup.UpdateTeamWithGraphInfo();

        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);

        log.LogInformation($"{nameof(GetTeamExtendedInformationAndIndexItActivity)}_Finished. InstanceId: {context.InstanceId}. Elapsed: {elapsedTime}");

        return teamWithGroup;
    }
}