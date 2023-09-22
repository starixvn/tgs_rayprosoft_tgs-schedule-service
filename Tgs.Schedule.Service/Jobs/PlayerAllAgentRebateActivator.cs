using Hangfire;
using Hangfire.Server;
using Tgs.ProfitRebate.Client;
using Tgs.ProfitRebate.Client.Models;

namespace Tgs.Schedule.Service.Jobs;

public class PlayerAllAgentRebateActivator : JobActivator
{
    private readonly ILogger<PlayerAllAgentRebateActivator> _logger;
    private readonly IProfitRebateClient _profitRebateClient;

    public PlayerAllAgentRebateActivator(
        IProfitRebateClient profitRebateClient,
        ILogger<PlayerAllAgentRebateActivator> logger)
    {
        _profitRebateClient = profitRebateClient;
        _logger = logger;
    }

    [Queue("player")]
    [AutomaticRetry(Attempts = 0)]
    public async Task CreatePlayerAllAgentRebateAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        _logger.LogInformation("#### player all agent rebate execution time: {createdTime} ####", $"{createdTime:u}");

        _ = await _profitRebateClient.CreatePlayerAllAgentRebateAsync(new CreateAllAgentPlayerRebatesRequest(null));
    }
}