using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbrella.TeamsSynchronizer.Models;

namespace Umbrella.TeamsSynchronizer.Functions.Activities;

public class CleanUpActivity
{
    [FunctionName(nameof(CleanUpActivity))]
    public async Task Run(
            [ActivityTrigger] IDurableActivityContext context,
            ILogger log)
    {
        log.LogInformation($"{nameof(CleanUpActivity)}_Started. InstanceId: {context.InstanceId}");

        // Simulates some cleanup actions after Sync is done
        // (i.e: delete Teams in our custom index that does not exist anymore)
        var syncedTeams = context.GetInput<IEnumerable<SyncTeamDto>>();
        var shouldBeDeletedTeams = syncedTeams.Where(t => t.IsArchived).ToList();

        var delay = int.Parse(Environment.GetEnvironmentVariable("CleanUpActivityDelayInMiliseconds") ?? "10");
        await Task.Delay(delay);

        log.LogInformation($"{nameof(CleanUpActivity)}_Finished. Id: {context.InstanceId}");
    }
}
