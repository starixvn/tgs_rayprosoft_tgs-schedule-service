using Hangfire;
using Hangfire.Server;

using Tgs.Report.Aggregate.Client;
using Tgs.Report.Aggregate.Client.Models;

namespace Tgs.Schedule.Service.Jobs;

public class RoundAggregateActivator(
    IReportAggregateClient reportAggregateClient,
    ILogger<RoundAggregateActivator> logger)
    : JobActivator
{
    [Queue("aggregate")]
    [AutomaticRetry(Attempts = 1)]
    public async Task CreateRoundAggregateAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        logger.LogInformation("#### round aggregate execution time: {createdTime} ####", $"{createdTime:u}");

        _ = await reportAggregateClient.CreateRoundAggregateAsync(new CreateRoundAggregateModel());
    }
}