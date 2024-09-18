using Hangfire;
using Hangfire.Server;
using Tgs.Player.All.Agent.Client;

namespace Tgs.Schedule.Service.Jobs;

public class PlayerAllAgentRebateSettlementActivator(
    IPlayerAllAgentScheduleClient playerScheduleClient,
    ILogger<PlayerAllAgentRebateSettlementActivator> logger)
    : JobActivator
{
    [Queue("player")]
    [AutomaticRetry(Attempts = 0)]
    public async Task CreatePlayerAllAgentRebateDailySettlementAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        logger.LogInformation("#### player all agent rebate daily settlement execution time: {createdTime} ####", $"{createdTime:u}");

        await playerScheduleClient.CreateDailyPlayerAllAgentRebatesAsync();
    }

    [Queue("player")]
    [AutomaticRetry(Attempts = 0)]
    public async Task CreatePlayerAllAgentRebateWeeklySettlementAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        logger.LogInformation("#### player all agent rebate weekly settlement execution time: {createdTime} ####", $"{createdTime:u}");

        await playerScheduleClient.CreateWeeklyPlayerAllAgentRebatesAsync();
    }

    [Queue("player")]
    [AutomaticRetry(Attempts = 0)]
    public async Task CreatePlayerAllAgentRebateMonthlySettlementAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        logger.LogInformation("#### player all agent rebate monthly settlement execution time: {createdTime} ####", $"{createdTime:u}");

        await playerScheduleClient.CreateMonthlyPlayerAllAgentRebatesAsync();
    }
}