using Hangfire;
using Hangfire.Server;
using SabaPlus.Client;

namespace Tgs.Schedule.Service.Jobs;

public class SabaPlusActivator(ILogger<SabaPlusActivator> logger, ISabaPlusClient client) : JobActivator
{
    [Queue("sabaplus")]
    [AutomaticRetry(Attempts = 1)]
    public async Task BetResultCheck(int minutes, PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        logger.LogInformation("#### SabaPlus: BetResultCheck at {createdTime} ####", $"{createdTime:u}");

        await client.BetResultCheck(minutes);
    }
}