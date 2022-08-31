using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbrella.TeamsSynchronizer.Models;
using Microsoft.Extensions.Logging;

namespace Umbrella.TeamsSynchronizer.Functions.Activities;

public class GetAllTeamsFromGraphActivity
{
    [FunctionName(nameof(GetAllTeamsFromGraphActivity))]
    public async Task<IEnumerable<SyncWorkspaceDto>> Run(
            [ActivityTrigger] IDurableActivityContext activityContext,
            ILogger log)
    {
        var totalWorkspacesToProcessCount = 
            int.Parse(Environment.GetEnvironmentVariable("NumberOfItemsToProcess") ?? "3");
        
        log.LogInformation($"GetAllTeamsFromGraphActivity: {totalWorkspacesToProcessCount}. InstanceId: {activityContext.InstanceId}");

        var delay = int.Parse(Environment.GetEnvironmentVariable("GetAllTeamsFromGraphActivityDelayInMiliseconds") ?? "10");
        await Task.Delay(delay);
        
        log.LogDebug($"GetAllTeamsFromGraphActivityDelay: {delay}");

        var highVolumeList = new List<SyncWorkspaceDto>();

        // Simulate calling an external API and return a High volume list of data
        // The issue happens with about 25k items. Tests were done with 30k.
        // (we have other production environments with about 10k items and are working fine)
        for (var i = 0; i < totalWorkspacesToProcessCount; i++)
        {
            highVolumeList.Add(SyncWorkspaceDto.NewFakeWorkspaceDto());
        }

        return highVolumeList;
    }
}