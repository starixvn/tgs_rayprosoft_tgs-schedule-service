using Hangfire;
using Hangfire.Server;
using Tgs.Agent.Dividend.Client;
using Tgs.BrandCertificate.Clients;
using Tgs.ProfitRebate.Client;
using Tgs.ProfitRebate.Client.Models;

namespace Tgs.Schedule.Service.Jobs;

public class AgentCommissionActivator(
    IBrandServiceClient brandServiceClient,
    IAgentDividendClient agentDividendClient,
    ILogger<AgentCommissionActivator> logger)
    : JobActivator
{
    [Queue("agent")]
    [AutomaticRetry(Attempts = 0)]
    public async Task CreateAgentCommissionAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        logger.LogInformation("#### agent commission execution time: {createdTime} ####", $"{createdTime:u}");

        var brands = brandServiceClient.QueryBrandListAsync(new BrandListQuery());

        await brands.ForEachAwaitAsync(async x =>
        {
            var brand = await brandServiceClient.GetBrandInfoAsync(x.BrandId);

            await agentDividendClient.GenerateAgentDividend(brand!.Id);
        });
    }
}