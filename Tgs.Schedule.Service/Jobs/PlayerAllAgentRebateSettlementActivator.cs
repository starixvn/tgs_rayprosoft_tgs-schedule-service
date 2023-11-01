using Hangfire;
using Hangfire.Server;
using Tgs.Player.All.Agent.Client;

namespace Tgs.Schedule.Service.Jobs;

public class PlayerAllAgentRebateSettlementActivator : JobActivator
{
    private readonly ILogger<PlayerAllAgentRebateSettlementActivator> _logger;
    private readonly IPlayerAllAgentScheduleClient _playerScheduleClient;

    public PlayerAllAgentRebateSettlementActivator(
        IPlayerAllAgentScheduleClient playerScheduleClient,
        ILogger<PlayerAllAgentRebateSettlementActivator> logger)
    {
        _playerScheduleClient = playerScheduleClient;
        _logger = logger;
    }

    [Queue("player")]
    [AutomaticRetry(Attempts = 0)]
    public async Task CreatePlayerAllAgentRebateDailySettlementAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        _logger.LogInformation("#### player all agent rebate daily settlement execution time: {createdTime} ####", $"{createdTime:u}");

        await _playerScheduleClient.CreateDailyPlayerAllAgentRebatesAsync();
    }

    [Queue("player")]
    [AutomaticRetry(Attempts = 0)]
    public async Task CreatePlayerAllAgentRebateWeeklySettlementAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        _logger.LogInformation("#### player all agent rebate weekly settlement execution time: {createdTime} ####", $"{createdTime:u}");

        await _playerScheduleClient.CreateWeeklyPlayerAllAgentRebatesAsync();
    }

    [Queue("player")]
    [AutomaticRetry(Attempts = 0)]
    public async Task CreatePlayerAllAgentRebateMonthlySettlementAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        _logger.LogInformation("#### player all agent rebate monthly settlement execution time: {createdTime} ####", $"{createdTime:u}");

        await _playerScheduleClient.CreateMonthlyPlayerAllAgentRebatesAsync();
    }
}