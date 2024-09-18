using Hangfire;
using Hangfire.Server;

using Tgs.ProfitRebate.Client;
using Tgs.ProfitRebate.Client.Models;

namespace Tgs.Schedule.Service.Jobs;

public class PlayerRebateActivator(
    IProfitRebateClient profitRebateClient,
    ILogger<PlayerRebateActivator> logger)
    : JobActivator
{
    [Queue("player")]
    [AutomaticRetry(Attempts = 0)]
    public async Task CreatePlayerRebateAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        logger.LogInformation("#### player rebate execution time: {createdTime} ####", $"{createdTime:u}");

        _ = await profitRebateClient.CreatePlayerRebateAsync(new CreatePlayerRebateRequest());
    }
}