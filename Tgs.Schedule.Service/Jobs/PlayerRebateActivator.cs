using Hangfire;
using Hangfire.Server;
using Tgs.ProfitRebate.Client;
using Tgs.ProfitRebate.Client.Models;

namespace Tgs.Schedule.Service.Jobs;

public class PlayerRebateActivator : JobActivator
{
    private readonly ILogger<PlayerRebateActivator> _logger;
    private readonly IProfitRebateClient _profitRebateClient;

    public PlayerRebateActivator(
        IProfitRebateClient profitRebateClient,
        ILogger<PlayerRebateActivator> logger)
    {
        _profitRebateClient = profitRebateClient;
        _logger = logger;
    }

    [Queue("player")]
    [AutomaticRetry(Attempts = 0)]
    public async Task CreatePlayerRebateAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        _logger.LogInformation("#### player rebate execution time: {createdTime} ####", $"{createdTime:u}");

        _ = await _profitRebateClient.CreatePlayerRebateAsync(new CreatePlayerRebateRequest());
    }
}