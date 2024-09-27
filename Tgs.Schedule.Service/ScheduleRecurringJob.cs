using Hangfire;

using Tgs.Schedule.Service.Jobs;

namespace Tgs.Schedule.Service;

public class ScheduleRecurringJob(IRecurringJobManager recurringJobManager) : IScheduleRecurringJob
{
    public void RecurringJobs()
    {
        recurringJobManager.AddOrUpdate<AgentDividendActivator>("AgentDividendDistribution",
            x => x.CreateAgentDividendAsync(default),
            Cron.Daily(16, 30));

        recurringJobManager.AddOrUpdate<PlayerAllAgentRebateSettlementActivator>("PlayerAllAgentRebateDailySettlement",
            x => x.CreatePlayerAllAgentRebateDailySettlementAsync(default),
            Cron.Daily(19));

        recurringJobManager.AddOrUpdate<PlayerAllAgentRebateSettlementActivator>("PlayerAllAgentRebateWeeklySettlement",
            x => x.CreatePlayerAllAgentRebateWeeklySettlementAsync(default),
            Cron.Weekly(DayOfWeek.Sunday, 19));

        recurringJobManager.AddOrUpdate<PlayerAllAgentRebateSettlementActivator>("PlayerAllAgentRebateMonthlySettlement",
            x => x.CreatePlayerAllAgentRebateMonthlySettlementAsync(default),
            "* * 19 L * ?");

        recurringJobManager.AddOrUpdate<PlayerAllAgentRebateDistributionActivator>("PlayerAllAgentRebateDailyDistribution",
            x => x.CreatePlayerAllAgentRebateDailyDistributionAsync(default),
            Cron.Daily(3, 30));

        recurringJobManager.AddOrUpdate<PlayerAllAgentRebateDistributionActivator>("PlayerAllAgentRebateWeeklyDistribution",
            x => x.CreatePlayerAllAgentRebateWeeklyDistributionAsync(default),
            Cron.Weekly(DayOfWeek.Monday, 3, 30));

        recurringJobManager.AddOrUpdate<PlayerAllAgentRebateDistributionActivator>("PlayerAllAgentRebateMonthlyDistribution",
            x => x.CreatePlayerAllAgentRebateMonthlyDistributionAsync(default),
            Cron.Monthly(1, 3, 30));

        recurringJobManager.AddOrUpdate<PlayerRebateActivator>("PlayerRebateDistribution",
            x => x.CreatePlayerRebateAsync(default),
            Cron.Daily(2, 15));

        recurringJobManager.AddOrUpdate<PlayerAggregateActivator>("CreatePlayerAggregate",
            x => x.CreatePlayerAggregateAsync(default),
            Cron.Hourly(10));

        recurringJobManager.AddOrUpdate<RoundAggregateActivator>("CreateRoundAggregate",
            x => x.CreateRoundAggregateAsync(default),
            Cron.Hourly(1));

        recurringJobManager.AddOrUpdate<GameTypeRoundAggregateActivator>("GenerateGameTypeRounds",
           x => x.GenerateGameTypeRoundsAsync(default),
           "2,17,32,47 * * * *");

        recurringJobManager.AddOrUpdate<BrandGameProviderActivator>("BrandGameProviderChangeStatus",
           x => x.ChangeStatusByScheduleAsync(default),
           Cron.Minutely);

        recurringJobManager.AddOrUpdate<SabaPlusActivator>("SabaPlusBetResultCheck",
            x => x.BetResultCheck(5, default),
            "*/5 * * * *");
    }
}