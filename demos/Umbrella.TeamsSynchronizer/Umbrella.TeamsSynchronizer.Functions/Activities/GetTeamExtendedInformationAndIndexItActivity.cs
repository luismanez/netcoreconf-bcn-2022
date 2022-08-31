using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Umbrella.TeamsSynchronizer.Models;

namespace Umbrella.TeamsSynchronizer.Functions.Activities;

public class GetTeamExtendedInformationAndIndexItActivity
{
    [FunctionName(nameof(GetTeamExtendedInformationAndIndexItActivity))]
    public async Task<SyncWorkspaceDto> Run(
            [ActivityTrigger] SyncWorkspaceDto workspaceWithGroup,
            ILogger log)
    {
        log.LogInformation($"{nameof(GetTeamExtendedInformationAndIndexItActivity)}_Started.");

        // Simulates getting information for that MS Teams that can be in external systems
        // and also to Index all the info gathered for the Team, into an Azure Search Index
        var delay = int.Parse(Environment.GetEnvironmentVariable("GetTeamExtendedInformationAndIndexItActivityDelayInMiliseconds") ?? "10");
        await Task.Delay(delay);

        workspaceWithGroup.UpdateWorkspaceWithGraphInfo();

        return workspaceWithGroup;
    }
}