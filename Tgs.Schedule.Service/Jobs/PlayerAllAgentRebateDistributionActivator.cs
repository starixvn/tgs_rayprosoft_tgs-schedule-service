using Hangfire;
using Hangfire.Server;
using Tgs.Player.All.Agent.Client;

namespace Tgs.Schedule.Service.Jobs;

public class PlayerAllAgentRebateDistributionActivator(
    IPlayerAllAgentScheduleClient playerScheduleClient,
    ILogger<PlayerAllAgentRebateDistributionActivator> logger)
    : JobActivator
{
    [Queue("player")]
    [AutomaticRetry(Attempts = 0)]
    public async Task CreatePlayerAllAgentRebateDailyDistributionAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        logger.LogInformation("#### player all agent rebate daily distribution execution time: {createdTime} ####", $"{createdTime:u}");

        var now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(8));

        var from = DateOnly.FromDateTime(now.AddDays(-1).DateTime);

        await playerScheduleClient.DistributeRebatesAsync(from, from);
    }

    [Queue("player")]
    [AutomaticRetry(Attempts = 0)]
    public async Task CreatePlayerAllAgentRebateWeeklyDistributionAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        logger.LogInformation("#### player all agent rebate weekly distribution execution time: {createdTime} ####", $"{createdTime:u}");

        var now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(8));

        var from = DateOnly.FromDateTime(now.AddDays(-7).GetMonday().DateTime);

        var to = DateOnly.FromDateTime(now.AddDays(-7).GetSunday().DateTime);

        await playerScheduleClient.DistributeRebatesAsync(from, to);
    }

    [Queue("player")]
    [AutomaticRetry(Attempts = 0)]
    public async Task CreatePlayerAllAgentRebateMonthlyDistributionAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        logger.LogInformation("#### player all agent rebate monthly distribution execution time: {createdTime} ####", $"{createdTime:u}");

        var now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(8));

        var from = DateOnly.FromDateTime(now.AddMonths(-1).GetFirstDayOfMonth().DateTime);

        var to = DateOnly.FromDateTime(now.AddMonths(-1).GetLastDayOfMonth().DateTime);

        await playerScheduleClient.DistributeRebatesAsync(from, to);
    }
}