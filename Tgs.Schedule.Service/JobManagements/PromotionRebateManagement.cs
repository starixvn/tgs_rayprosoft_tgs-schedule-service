using System.ComponentModel;

using Hangfire;
using Hangfire.Dashboard.Management.v2.Metadata;
using Hangfire.Dashboard.Management.v2.Support;

using Tgs.Promotion.RebateService.Client;

namespace Tgs.Schedule.Service.JobManagements;

[ManagementPage(MenuName = "PromotionRebate", Title = "活動反水")]
public class PromotionRebateManagement : IJob
{
    private readonly ILogger<PromotionRebateManagement> _logger;
    private readonly IPromotionRebateClient _promotionRebateClient;

    public PromotionRebateManagement(IPromotionRebateClient promotionRebateClient,
        ILogger<PromotionRebateManagement> logger)
    {
        _promotionRebateClient = promotionRebateClient;
        _logger = logger;
    }

    [DisplayName("promotion rebate settle daily")]
    [Description("返水每日結算(未派發僅計算)")]
    [Queue("promotion")]
    public async Task SettlePromotionRebateAsync(
        [DisplayData("指定日期",
            "2021/02/18",
            "依照時區做結算",
            "2023/09/18")]
        string date,
        [DisplayData("指定utc小時",
            "",
            "+08時區整點 = 16")]
        int hour = 16)
    {
        _logger.LogInformation($"Start time:{DateTime.UtcNow}");

        await _promotionRebateClient.SettleAsync(DateOnly.Parse(date), hour);
    }

    [DisplayName("promotion rebate send daily and weekly")]
    [Description("返水每日每周反水派發")]
    [Queue("promotion")]
    public async Task SendPromotionRebateAsync(
        [DisplayData("指定日期",
            "2021/02/18",
            "依照時區做結算",
            "2023/09/18")]
        string date,
        [DisplayData("指定utc小時",
            "",
            "+08時區整點 = 16")]
        int hour = 16)
    {
        _logger.LogInformation($"Start time:{DateTime.UtcNow}");

        await _promotionRebateClient.SendBonusAsync(DateOnly.Parse(date), hour);
    }
}