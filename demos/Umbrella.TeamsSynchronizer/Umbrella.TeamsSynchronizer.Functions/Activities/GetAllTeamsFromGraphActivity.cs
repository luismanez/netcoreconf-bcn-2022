using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbrella.TeamsSynchronizer.Models;
using Microsoft.Extensions.Logging;

namespace Umbrella.TeamsSynchronizer.Functions.Activities;

public class GetAllTeamsFromGraphActivity
{
    [FunctionName(nameof(GetAllTeamsFromGraphActivity))]
    public async Task<IEnumerable<SyncTeamDto>> Run(
            [ActivityTrigger] IDurableActivityContext activityContext,
            ILogger log)
    {
        var totalTeamsToProcessCount = activityContext.GetInput<int>();
       
        log.LogInformation($"GetAllTeamsFromGraphActivity_Started. ItemsToProcess: {totalTeamsToProcessCount}. InstanceId: {activityContext.InstanceId}");

        var delay = int.Parse(Environment.GetEnvironmentVariable("GetAllTeamsFromGraphActivityDelayInMiliseconds") ?? "10");
        await Task.Delay(delay);      

        var highVolumeList = new List<SyncTeamDto>();

        // Simulate calling an external API and return a High volume list of data
        // The issue happens with about 25k items. Tests were done with 30k.
        for (var i = 0; i < totalTeamsToProcessCount; i++)
        {
            highVolumeList.Add(SyncTeamDto.NewFakeTeamDto());
        }

        log.LogInformation($"GetAllTeamsFromGraphActivity_Finished. ItemsToProcess: {totalTeamsToProcessCount}. InstanceId: {activityContext.InstanceId}");

        return highVolumeList;
    }
}