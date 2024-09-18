using Hangfire;
using Hangfire.Server;
using TGS.Core.Infrastructures.Nats.Interface;
using Tgs.Report.Aggregate.Client;
using Tgs.Report.Aggregate.Client.Models;

namespace Tgs.Schedule.Service.Jobs;

public class MilestonePromotionActivator(
    IMessageManager messageManager,
    ILogger<MilestonePromotionActivator> logger)
    : JobActivator
{
    [Queue("promotion")]
    [AutomaticRetry(Attempts = 1)]
    public async Task MilestonePromotionDistributionAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        logger.LogInformation("#### milestone promotion execution time: {createdTime} ####", $"{createdTime:u}");

        await messageManager.PublishAsync("tgs.promotion.milestone.distribute", createdTime);
    }
}