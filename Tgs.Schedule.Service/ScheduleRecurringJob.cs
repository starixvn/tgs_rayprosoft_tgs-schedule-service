using Hangfire;

using Tgs.Schedule.Service.Jobs;

namespace Tgs.Schedule.Service;

public class ScheduleRecurringJob : IScheduleRecurringJob
{
    private readonly IRecurringJobManager _recurringJobManager;

    public ScheduleRecurringJob(IRecurringJobManager recurringJobManager)
    {
        _recurringJobManager = recurringJobManager;
    }

    public void RecurringJobs()
    {
        _recurringJobManager.AddOrUpdate<AgentCommissionActivator>("AgentCommissionDistribution",
            x => x.CreateAgentCommissionAsync(default),
            Cron.Daily(16, 30));

        _recurringJobManager.AddOrUpdate<PlayerAllAgentRebateSettlementActivator>("PlayerAllAgentRebateDailySettlement",
            x => x.CreatePlayerAllAgentRebateDailySettlementAsync(default),
            Cron.Daily(19));

        _recurringJobManager.AddOrUpdate<PlayerAllAgentRebateSettlementActivator>("PlayerAllAgentRebateWeeklySettlement",
            x => x.CreatePlayerAllAgentRebateWeeklySettlementAsync(default),
            Cron.Weekly(DayOfWeek.Sunday, 19));

        _recurringJobManager.AddOrUpdate<PlayerAllAgentRebateSettlementActivator>("PlayerAllAgentRebateMonthlySettlement",
            x => x.CreatePlayerAllAgentRebateMonthlySettlementAsync(default),
            "* * 19 L * ?");

        _recurringJobManager.AddOrUpdate<PlayerAllAgentRebateDistributionActivator>("PlayerAllAgentRebateDailyDistribution",
            x => x.CreatePlayerAllAgentRebateDailyDistributionAsync(default),
            Cron.Daily(3, 30));

        _recurringJobManager.AddOrUpdate<PlayerAllAgentRebateDistributionActivator>("PlayerAllAgentRebateWeeklyDistribution",
            x => x.CreatePlayerAllAgentRebateWeeklyDistributionAsync(default),
            Cron.Weekly(DayOfWeek.Monday, 3, 30));

        _recurringJobManager.AddOrUpdate<PlayerAllAgentRebateDistributionActivator>("PlayerAllAgentRebateMonthlyDistribution",
            x => x.CreatePlayerAllAgentRebateMonthlyDistributionAsync(default),
            Cron.Monthly(1, 3, 30));

        _recurringJobManager.AddOrUpdate<PlayerRebateActivator>("PlayerRebateDistribution",
            x => x.CreatePlayerRebateAsync(default),
            Cron.Daily(2, 15));

        _recurringJobManager.AddOrUpdate<PlayerAggregateActivator>("CreatePlayerAggregate",
            x => x.CreatePlayerAggregateAsync(default),
            Cron.Hourly(10));

        _recurringJobManager.AddOrUpdate<RoundAggregateActivator>("CreateRoundAggregate",
            x => x.CreateRoundAggregateAsync(default),
            Cron.Hourly(1));

        _recurringJobManager.AddOrUpdate<GameTypeRoundAggregateActivator>("GenerateGameTypeRounds",
            x => x.GenerateGameTypeRoundsAsync(default),
            "2,17,32,47 * * * *");

        _recurringJobManager.AddOrUpdate<BrandGameProviderActivator>("BrandGameProviderChangeStatus",
            x => x.ChangeStatusByScheduleAsync(default),
            Cron.Minutely);
    }
}