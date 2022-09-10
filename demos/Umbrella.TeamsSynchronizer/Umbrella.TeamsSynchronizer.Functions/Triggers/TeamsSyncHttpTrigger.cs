using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbrella.TeamsSynchronizer.Functions.Orchestrators;

namespace Umbrella.TeamsSynchronizer.Functions.Triggers;

public class TeamsSyncHttpTrigger
{
    [FunctionName(nameof(TeamsSyncHttpTrigger))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "teams/sync/start/{itemsToProcessCount:int}")]
        HttpRequest req,
        int itemsToProcessCount,
        CancellationToken cancellationToken,
        [DurableClient] IDurableOrchestrationClient starter,
        ILogger log)
    {
        log.LogInformation($"TeamsSyncHttpTrigger running. ItemsToProcess: {itemsToProcessCount}");

        if (itemsToProcessCount < 1) return new BadRequestObjectResult("Items to process must be greater than 0");

        var instanceId = await starter.StartNewAsync(nameof(TeamsSynchronisationOrchestrator), (object) itemsToProcessCount);

        return starter.CreateCheckStatusResponse(req, instanceId);
    }
}