using Hangfire;
using Hangfire.Server;
using Tgs.Agent.Dividend.Client;

namespace Tgs.Schedule.Service.Jobs;

public class AgentCommissionActivator(
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

        await agentDividendClient.GenerateAgentDividend(default);
    }
}