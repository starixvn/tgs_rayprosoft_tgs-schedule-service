using Hangfire;
using Hangfire.Server;
using Tgs.Report.Aggregate.Client;
using Tgs.Report.Aggregate.Client.Models;

namespace Tgs.Schedule.Service.Jobs;

public class RoundAggregateActivator : JobActivator
{
    private readonly ILogger<RoundAggregateActivator> _logger;
    private readonly IReportAggregateClient _reportAggregateClient;

    public RoundAggregateActivator(
        IReportAggregateClient reportAggregateClient,
        ILogger<RoundAggregateActivator> logger)
    {
        _reportAggregateClient = reportAggregateClient;
        _logger = logger;
    }

    [Queue("aggregate")]
    [AutomaticRetry(Attempts = 1)]
    public async Task CreateRoundAggregateAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        _logger.LogInformation("#### round aggregate execution time: {createdTime} ####", $"{createdTime:u}");

        _ = await _reportAggregateClient.CreateRoundAggregateAsync(new CreateRoundAggregateModel());
    }
}