using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Umbrella.TeamsSynchronizer.Models;
using Umbrella.TeamsSynchronizer.Functions.Activities;

namespace Umbrella.TeamsSynchronizer.Functions.Orchestrators;

public class TeamsSynchronisationOrchestrator
{
    [FunctionName(nameof(TeamsSynchronisationOrchestrator))]
    public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger log)
    {
		try
		{
            log = context.CreateReplaySafeLogger(log);

            var teamsToProcess = 
                await context.CallActivityAsync<IEnumerable<SyncWorkspaceDto>>(nameof(GetAllTeamsFromGraphActivity), null);

            // Fan out / in Durable Functions pattern
            var workspacesToSyncTasks = new List<Task<SyncWorkspaceDto>>();
            foreach (var workspace in teamsToProcess)
            {
                Task<SyncWorkspaceDto> workspacesToSyncTask =
                    context.CallActivityAsync<SyncWorkspaceDto>(
                        nameof(GetTeamExtendedInformationAndIndexItActivity), 
                        workspace);

                workspacesToSyncTasks.Add(workspacesToSyncTask);
            }
            var syncedTeams = (await Task.WhenAll(workspacesToSyncTasks)).ToList();


            await context.CallActivityAsync(nameof(CleanUpActivity), syncedTeams);

            await context.CallActivityAsync(
                nameof(SendNotificationActivity), 
                syncedTeams);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "TeamsSynchronisationOrchestrator_Something_Went_Wrong");
        }
    }
}
