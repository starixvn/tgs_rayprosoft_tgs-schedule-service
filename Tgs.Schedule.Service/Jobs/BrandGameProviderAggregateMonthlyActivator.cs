using Hangfire;
using Hangfire.Server;
using Tgs.BrandCertificate;
using Tgs.BrandCertificate.Clients;
using TGS.Core.Infrastructures.Nats.Interface;

namespace Tgs.Schedule.Service.Jobs;

public class BrandGameProviderAggregateMonthlyActivator(
    IBrandServiceClient brandServiceClient,
    IMessageManager messageManager,
    ILogger<BrandGameProviderAggregateMonthlyActivator> logger)
    : JobActivator
{
    [Queue("aggregate")]
    [AutomaticRetry(Attempts = 1)]
    public async Task GenerateBrandGameProviderMonthlyAggregateAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        logger.LogInformation("#### Monthly brand game provider aggregate execution time: {createdTime} ####", $"{createdTime:u}");

        var now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(8));

        var startDate = DateTimeOffsetExtensions.GetFirstDayOfMonth(now.AddMonths(-1));

        var endDate = DateTimeOffsetExtensions.GetFirstDayOfMonth(startDate);

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