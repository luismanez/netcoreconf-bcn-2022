using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
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

            log.LogInformation($"TeamsSynchronisationOrchestrator_Started. InstanceId: {context.InstanceId}. ParentId: {context.ParentInstanceId}");

            var teamsToProcess = 
                await context.CallActivityAsync<IEnumerable<SyncTeamDto>>(nameof(GetAllTeamsFromGraphActivity), null);

            // Fan out / in Durable Functions pattern
            var teamsToSyncTasks = new List<Task<SyncTeamDto>>();
            foreach (var team in teamsToProcess)
            {
                Task<SyncTeamDto> teamToSyncTask =
                    context.CallActivityAsync<SyncTeamDto>(
                        nameof(GetTeamExtendedInformationAndIndexItActivity), 
                        team);

                teamsToSyncTasks.Add(teamToSyncTask);
            }
            var syncedTeams = (await Task.WhenAll(teamsToSyncTasks)).ToList();


            await context.CallActivityAsync(nameof(CleanUpActivity), syncedTeams);

            await context.CallActivityAsync(
                nameof(SendNotificationActivity), 
                syncedTeams);

            log.LogInformation($"TeamsSynchronisationOrchestrator_Finished. InstanceId: {context.InstanceId}. ParentId: {context.ParentInstanceId}");
        }
        catch (Exception ex)
        {
            log.LogError(ex, "TeamsSynchronisationOrchestrator_Something_Went_Wrong");
        }
    }
}
