using System.ComponentModel;

using Hangfire;
using Hangfire.Dashboard.Management.v2.Metadata;
using Hangfire.Dashboard.Management.v2.Support;
using Tgs.Agent.Dividend.Client;

namespace Tgs.Schedule.Service.JobManagements;

[ManagementPage(MenuName = "AgentDividend", Title = "代理分紅")]
public class AgentDividendManagement(
    IAgentDividendClient agentDividendClient,
    ILogger<AgentDividendManagement> logger)
    : IJob
{
    [DisplayName("create agent dividends")]
    [Description("分紅指定的品牌")]
    [Queue("agent")]
    public async Task CreateAgentDividendsAsync(
        [DisplayData("指定品牌",
            "brand id",
            "TGS")]
        Guid brandId)
    {
        logger.LogInformation($"Start time:{DateTime.UtcNow}");

        await agentDividendClient.GenerateAgentDividend(brandId, default);
    }
}