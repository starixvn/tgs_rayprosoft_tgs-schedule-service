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

        _recurringJobManager.AddOrUpdate<PlayerAllAgentRebateActivator>("PlayerAllAgentRebateDistribution",
            x => x.CreatePlayerAllAgentRebateAsync(default),
            Cron.Daily(16, 35));

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