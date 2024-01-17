using System.ComponentModel;

using Hangfire;
using Hangfire.Dashboard.Management.v2.Metadata;
using Hangfire.Dashboard.Management.v2.Support;

using Tgs.ProfitRebate.Client;
using Tgs.ProfitRebate.Client.Models;

namespace Tgs.Schedule.Service.JobManagements;

[ManagementPage(MenuName = "AgentCommission", Title = "代理分紅")]
public class AgentCommissionManagement(
    IProfitRebateClient profitRebateClient,
    ILogger<AgentCommissionManagement> logger)
    : IJob
{
    [DisplayName("create agent commissions")]
    [Description("分紅指定的品牌")]
    [Queue("agent")]
    public async Task CreateAgentCommissionsAsync(
        [DisplayData("指定品牌",
            "brand code",
            "TGS")]
        string brandCode = "")
    {
        logger.LogInformation($"Start time:{DateTime.UtcNow}");

        await profitRebateClient.CreateAgentDividendAsync(new CreateAgentDividendsRequest(brandCode));
    }
}