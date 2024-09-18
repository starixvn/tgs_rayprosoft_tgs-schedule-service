using Hangfire;
using Hangfire.Server;

using Tgs.Game.Type.Round.Aggregate.Client;

namespace Tgs.Schedule.Service.Jobs;

public class GameTypeRoundAggregateActivator(
    IGameTypeRoundClient gameTypeRoundClient,
    ILogger<GameTypeRoundAggregateActivator> logger)
    : JobActivator
{
    [Queue("aggregate")]
    [AutomaticRetry(Attempts = 1)]
    public async Task GenerateGameTypeRoundsAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        logger.LogInformation("#### game type round aggregate execution time: {createdTime} ####", $"{createdTime:u}");

        var intervals = GenerateReverseTimeIntervals(DateTime.UtcNow, 4);

        foreach (var (_, from, to) in intervals)
        {
            await gameTypeRoundClient.GenerateGameTypeRoundsAsync(from, to).ConfigureAwait(false);

            await Task.Delay(3000);
        }
    }

    private static IEnumerable<(int number, DateTime from, DateTime to)> GenerateReverseTimeIntervals(DateTime endDate,
        int count)
    {
        var startDate = endDate;

        startDate = startDate.AddMinutes(-(startDate.Minute % 15)).AddSeconds(-startDate.Second);

        return Enumerable.Range(1, count)
            .Select(c =>
            {
                var nextStartDate = startDate.AddMinutes(-15);
                var toDate = startDate;
                startDate = nextStartDate;

                return (count: c, nextStartDate, toDate);
            })
            .OrderByDescending(x => x.count)
            .ToArray();
    }
}