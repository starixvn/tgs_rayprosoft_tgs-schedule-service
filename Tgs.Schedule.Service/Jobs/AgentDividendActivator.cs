using Hangfire;
using Hangfire.Server;
using Tgs.Agent.Dividend.Client;

namespace Tgs.Schedule.Service.Jobs;

public class AgentDividendActivator(
    IAgentDividendClient agentDividendClient,
    ILogger<AgentDividendActivator> logger)
    : JobActivator
{
    [Queue("agent")]
    [AutomaticRetry(Attempts = 0)]
    public async Task CreateAgentDividendAsync(PerformContext? context)
    {
        var createdTime = context!.BackgroundJob.CreatedAt;

        logger.LogInformation("#### agent dividend execution time: {createdTime} ####", $"{createdTime:u}");

        await agentDividendClient.GenerateAgentDividend(default);
    }
}