using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Umbrella.TeamsSynchronizer.Models;
using System.Linq;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace Umbrella.TeamsSynchronizer.Functions.Activities;

public class SendNotificationActivity
{
    [FunctionName(nameof(SendNotificationActivity))]
    public async Task Run(
            [ActivityTrigger] IDurableActivityContext context,
            ILogger log)
    {
        // Simulates creating a report with details about all the Teams that have been synced/removed
        // and sends a notification (Email, Teams feed...) with that report
        var syncedTeams = context.GetInput<IEnumerable<SyncWorkspaceDto>>();
        var delay = int.Parse(Environment.GetEnvironmentVariable("SendNotificationActivityDelayInMiliseconds") ?? "10");
        await Task.Delay(delay);

        log.LogInformation(
            $"{nameof(SendNotificationActivity)}_Started. Teams synced: {syncedTeams.Count()}");
    }
}
