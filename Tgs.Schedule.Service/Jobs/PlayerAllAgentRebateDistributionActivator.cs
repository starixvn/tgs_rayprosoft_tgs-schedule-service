using Hangfire;
using Hangfire.Server;
using Tgs.Player.All.Agent.Client;

namespace Tgs.Schedule.Service.Jobs;

public class PlayerAllAgentRebateDistributionActivator : JobActivator
{
    private readonly ILogger<PlayerAllAgentRebateDistributionActivator> _logger;
    private readonly IPlayerAllAgentScheduleClient _playerScheduleClient;

    public PlayerAllAgentRebateDistributionActivator(
        IPlayerAllAgentScheduleClient playerScheduleClient,
        ILogger<PlayerAllAgentRebateDistributionActivator> logger)
    {
        _playerScheduleClient = playerScheduleClient;
        _logger = logger;
    }

    [Queue("player")]
    [AutomaticRetry(Attempts = 0)]
    public async Task CreatePlayerAllAgentRebateDailyDistributionAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        _logger.LogInformation("#### player all agent rebate daily distribution execution time: {createdTime} ####", $"{createdTime:u}");

        var now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(8));

        var from = DateOnly.FromDateTime(now.DateTime);

        await _playerScheduleClient.DistributeRebatesAsync(from, from);
    }

    [Queue("player")]
    [AutomaticRetry(Attempts = 0)]
    public async Task CreatePlayerAllAgentRebateWeeklyDistributionAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        _logger.LogInformation("#### player all agent rebate weekly distribution execution time: {createdTime} ####", $"{createdTime:u}");

        var now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(8));

        var from = DateOnly.FromDateTime(now.AddDays(-7).GetMonday().DateTime);

        var to = DateOnly.FromDateTime(now.AddDays(-7).GetSunday().DateTime);

        await _playerScheduleClient.DistributeRebatesAsync(from, to);
    }

    [Queue("player")]
    [AutomaticRetry(Attempts = 0)]
    public async Task CreatePlayerAllAgentRebateMonthlyDistributionAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        _logger.LogInformation("#### player all agent rebate monthly distribution execution time: {createdTime} ####", $"{createdTime:u}");

        var now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(8));

        var from = DateOnly.FromDateTime(now.AddMonths(-1).GetFirstDayOfMonth().DateTime);

        var to = DateOnly.FromDateTime(now.AddMonths(-1).GetLastDayOfMonth().DateTime);

        await _playerScheduleClient.DistributeRebatesAsync(from, to);
    }
}