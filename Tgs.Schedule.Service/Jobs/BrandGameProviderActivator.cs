using Hangfire;
using Hangfire.Server;

using Tgs.Brand.GameProvider.Client;

namespace Tgs.Schedule.Service.Jobs;

public class BrandGameProviderActivator : JobActivator
{
    private readonly IBrandGameProviderClient _client;
    private readonly ILogger<BrandGameProviderActivator> _logger;

    public BrandGameProviderActivator(
        IBrandGameProviderClient client,
        ILogger<BrandGameProviderActivator> logger)
    {
        _client = client;
        _logger = logger;
    }

    [Queue("brand")]
    [AutomaticRetry(Attempts = 1)]
    public async Task ChangeStatusByScheduleAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        _logger.LogInformation("#### change brand game provider status execution time: {createdTime} ####",
            $"{createdTime:u}");

        await _client.ChangeStatusByScheduleAsync();
    }
}