using Hangfire;
using Hangfire.Dashboard.Management.v2.Metadata;
using Hangfire.Dashboard.Management.v2.Support;
using SabaPlus.Client;
using System.ComponentModel;

namespace Tgs.Schedule.Service.JobManagements;

[ManagementPage(MenuName = "SabaPlus", Title = "SabaPlus")]
public class SabaPlusManagement(ISabaPlusClient client, ILogger<SabaPlusManagement> logger) : IJob
{
    [DisplayName("Bet result check")]
    [Queue("sabaplus")]
    public async Task BetResultCheckAsync([DisplayData("Diff in Minute", "5")] int minute)
    {
        logger.LogInformation($"Start time:{DateTime.UtcNow}");

        await client.BetResultCheck(minute, default);
    }
}