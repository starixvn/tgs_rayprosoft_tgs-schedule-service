using Hangfire;
using Hangfire.Server;
using Tgs.BrandCertificate.Clients;
using Tgs.ProfitRebate.Client;
using Tgs.ProfitRebate.Client.Models;

namespace Tgs.Schedule.Service.Jobs;

public class AgentCommissionActivator : JobActivator
{
    private readonly IBrandServiceClient _brandServiceClient;
    private readonly ILogger<AgentCommissionActivator> _logger;
    private readonly IProfitRebateClient _profitRebateClient;

    public AgentCommissionActivator(IBrandServiceClient brandServiceClient,
        IProfitRebateClient profitRebateClient,
        ILogger<AgentCommissionActivator> logger
    )
    {
        _brandServiceClient = brandServiceClient;
        _profitRebateClient = profitRebateClient;
        _logger = logger;
    }

    [Queue("agent")]
    [AutomaticRetry(Attempts = 0)]
    public async Task CreateAgentCommissionAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        _logger.LogInformation("#### agent commission execution time: {createdTime} ####", $"{createdTime:u}");

        var brands = _brandServiceClient.QueryBrandListAsync(new BrandListQuery());

        await brands.ForEachAwaitAsync(async x =>
        {
            var brand = await _brandServiceClient.GetBrandInfoAsync(x.BrandId);

            _ = await _profitRebateClient.CreateAgentDividendAsync(new CreateAgentDividendsRequest(brand!.Code));
        });
    }
}