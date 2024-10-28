using Hangfire;
using Hangfire.Server;
using Tgs.BrandCertificate;
using Tgs.BrandCertificate.Clients;
using TGS.Core.Infrastructures.Nats.Interface;

namespace Tgs.Schedule.Service.Jobs;

public class BrandGameProviderAggregateDailyActivator(
    IBrandServiceClient brandServiceClient,
    IMessageManager messageManager,
    ILogger<BrandGameProviderAggregateDailyActivator> logger)
    : JobActivator
{
    [Queue("aggregate")]
    [AutomaticRetry(Attempts = 1)]
    public async Task GenerateBrandGameProviderDailyAggregateAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        logger.LogInformation("#### Daily brand game provider aggregate execution time: {createdTime} ####", $"{createdTime:u}");

        var now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(8));

        var endDate = new DateTimeOffset(now.Year, now.Month, now.Day, 0, 0, 0, now.Offset);

        var startDate = endDate.AddDays(-1);

        var brands = brandServiceClient.QueryBrandListAsync(new BrandListQuery());

        await brands.Where(x => x.Status != BrandStatus.Archived).ForEachAwaitAsync(async x =>
        {
            await messageManager.PublishAsync("tgs.report.aggregate.brand.game.provider.schedule", new
            {
                x.BrandId,
                StartDate = startDate,
                EndDate = endDate
            });
        });
    }
}