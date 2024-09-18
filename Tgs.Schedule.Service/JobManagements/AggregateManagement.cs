using System.ComponentModel;

using Hangfire;
using Hangfire.Dashboard.Management.v2.Metadata;
using Hangfire.Dashboard.Management.v2.Support;

using Tgs.Game.Type.Round.Aggregate.Client;
using Tgs.Report.Aggregate.Client;
using Tgs.Report.Aggregate.Client.Models;

namespace Tgs.Schedule.Service.JobManagements;

[ManagementPage(MenuName = "Aggregate", Title = "Aggregate")]
public class AggregateManagement(
    IReportAggregateClient aggregateClient,
    IGameTypeRoundClient gameTypeRoundClient,
    ILogger<AggregateManagement> logger)
    : IJob
{
    [DisplayName("create round aggregate")]
    [Description("新增局集合指定的時間")]
    [Queue("aggregate")]
    public async Task CreateRoundAggregateAsync(
        [DisplayData("指定日期",
            "2021/02/18",
            "Default offset +08:00",
            "2023/09/18")]
        string dateTime = "",
        [DisplayData("指定小時",
            "",
            "0~23, 13 = 13~14")]
        int hour = 0)
    {
        logger.LogInformation($"Start time:{DateTime.UtcNow}");

        if (DateTimeOffset.TryParse($"{dateTime} +8:00", out _))
            await aggregateClient.ManualCreateRoundAggregateAsync(
                new ManualCreateUpdateRoundAggregateModel(dateTime, hour));
    }

    [DisplayName("create player aggregate")]
    [Description("新增玩家集合指定的時間")]
    [Queue("aggregate")]
    public async Task CreatePlayerAggregateAsync(
        [DisplayData("指定日期",
            "2021/02/18",
            "Default offset +08:00",
            "2023/09/18")]
        string dateTime = "",
        [DisplayData("指定小時",
            "",
            "0~23, 13 = 13~14")]
        int hour = 0)
    {
        logger.LogInformation($"Start time:{DateTime.UtcNow}");

        if (DateTimeOffset.TryParse($"{dateTime} +8:00", out _))
            await aggregateClient.ManualCreatePlayerAggregateAsync(
                new ManualCreateUpdatePlayerAggregateModel(dateTime, hour));
    }

    [DisplayName("create game type round aggregate")]
    [Description("新增15分鐘局集合指定的時間")]
    [Queue("aggregate")]
    public async Task CreateGameTypeRoundAggregateAsync(
        [DisplayData("指定時間",
            "2021/02/18 00:15:00",
            "utc time",
            "2023/08/17 00:15:00")]
        string dateTime = "",
        [DisplayData("指定往前幾節",
            "",
            "例5: 依指定時間執行往前推5節時間段")]
        int count = 0)
    {
        if (!DateTimeOffset.TryParse(dateTime, out _)) return;

        var endDate = DateTimeOffset.Parse($"{dateTime}Z").UtcDateTime;

        var intervals = GenerateReverseTimeIntervals(endDate, count);

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