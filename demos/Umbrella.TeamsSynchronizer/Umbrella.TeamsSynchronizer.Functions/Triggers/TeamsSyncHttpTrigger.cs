using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Umbrella.TeamsSynchronizer.Functions.Orchestrators;

namespace Umbrella.TeamsSynchronizer.Functions.Triggers;

public class TeamsSyncHttpTrigger
{
    [FunctionName(nameof(TeamsSyncHttpTrigger))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "teams/sync/start")]
        HttpRequest req,
        CancellationToken cancellationToken,
        [DurableClient] IDurableOrchestrationClient starter,
        ILogger log)
    {
        log.LogInformation("TeamsSyncHttpTrigger HTTP trigger function started");

        var instanceId = await starter.StartNewAsync(nameof(TeamsSynchronisationOrchestrator));

        return starter.CreateCheckStatusResponse(req, instanceId);
    }
}