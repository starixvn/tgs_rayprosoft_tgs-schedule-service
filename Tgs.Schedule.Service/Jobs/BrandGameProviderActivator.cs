using Hangfire;
using Hangfire.Server;

using Tgs.Brand.GameProvider.Client;

namespace Tgs.Schedule.Service.Jobs;

public class BrandGameProviderActivator(
    IBrandGameProviderClient client,
    ILogger<BrandGameProviderActivator> logger)
    : JobActivator
{
    [Queue("brand")]
    [AutomaticRetry(Attempts = 1)]
    public async Task ChangeStatusByScheduleAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        logger.LogInformation("#### change brand game provider status execution time: {createdTime} ####",
            $"{createdTime:u}");

        await client.ChangeStatusByScheduleAsync();
    }
}