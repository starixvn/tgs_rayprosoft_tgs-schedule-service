using System.ComponentModel;

using Hangfire;
using Hangfire.Dashboard.Management.v2.Metadata;
using Hangfire.Dashboard.Management.v2.Support;

using Tgs.ProfitRebate.Client;
using Tgs.ProfitRebate.Client.Models;

namespace Tgs.Schedule.Service.JobManagements;

[ManagementPage(MenuName = "PlayerRebate", Title = "玩家反水")]
public class PlayerRebateManagement : IJob
{
    private readonly ILogger<PlayerRebateManagement> _logger;
    private readonly IProfitRebateClient _profitRebateClient;

    public PlayerRebateManagement(IProfitRebateClient profitRebateClient, ILogger<PlayerRebateManagement> logger)
    {
        _profitRebateClient = profitRebateClient;
        _logger = logger;
    }

    [DisplayName("create player vip rebates")]
    [Description("vip返水指定的時間")]
    [Queue("player")]
    public async Task CreatePlayerRebateAsync(
        [DisplayData("指定日期",
            "2021/02/18",
            "Default offset +08:00",
            "2023/09/18")]
        string date = "")
    {
        _logger.LogInformation($"Start time:{DateTime.UtcNow}");

        await _profitRebateClient.CreatePlayerRebateAsync(new CreatePlayerRebateRequest(date));
    }

    [DisplayName("create player all agent rebates")]
    [Description("全民代理返水指定的時間")]
    [Queue("player")]
    public async Task CreatePlayerAllAgentRebateAsync(
        [DisplayData("指定日期",
            "2021/02/18",
            "Default offset +08:00",
            "2023/09/18")]
        string dateTime = "")
    {
        _logger.LogInformation($"Start time:{DateTime.UtcNow}");

        await _profitRebateClient.CreatePlayerAllAgentRebateAsync(new CreateAllAgentPlayerRebatesRequest(dateTime));
    }
}