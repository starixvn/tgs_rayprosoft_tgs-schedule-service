using Hangfire;
using Hangfire.Server;
using Tgs.Report.Aggregate.Client;
using Tgs.Report.Aggregate.Client.Models;

namespace Tgs.Schedule.Service.Jobs;

public class PlayerAggregateActivator : JobActivator
{
    private readonly ILogger<PlayerAggregateActivator> _logger;
    private readonly IReportAggregateClient _reportAggregateClient;

    public PlayerAggregateActivator(
        IReportAggregateClient reportAggregateClient,
        ILogger<PlayerAggregateActivator> logger)
    {
        _reportAggregateClient = reportAggregateClient;
        _logger = logger;
    }

    [Queue("aggregate")]
    [AutomaticRetry(Attempts = 1)]
    public async Task CreatePlayerAggregateAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        _logger.LogInformation("#### player aggregate execution time: {createdTime} ####", $"{createdTime:u}");

        _ = await _reportAggregateClient.CreatePlayerAggregateAsync(new CreateUpdatePlayerAggregateModel());
    }
}