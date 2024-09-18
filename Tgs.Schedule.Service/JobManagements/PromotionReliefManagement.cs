using System.ComponentModel;

using Hangfire;
using Hangfire.Dashboard.Management.v2.Metadata;
using Hangfire.Dashboard.Management.v2.Support;

using Tgs.Promotion.ReliefMoney.Client;

namespace Tgs.Schedule.Service.JobManagements;

[ManagementPage(MenuName = "SOS", Title = "救援金")]
public class PromotionReliefManagement(
    IPromotionReliefClient promotionReliefClient,
    ILogger<PromotionReliefManagement> logger)
    : IJob
{
    [DisplayName("promotion relief settle daily")]
    [Description("SOS每日結算(未派發僅計算)")]
    [Queue("promotion")]
    public async Task SettlePromotionReliefAsync(
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
        logger.LogInformation($"Start time:{DateTime.UtcNow}");

        await promotionReliefClient.SettleAsync(DateOnly.Parse(date), hour);
    }

    [DisplayName("promotion relief send daily and weekly and monthly")]
    [Description("SOS每日每周每月派發")]
    [Queue("promotion")]
    public async Task SendPromotionReliefAsync(
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
        logger.LogInformation($"Start time:{DateTime.UtcNow}");

        await promotionReliefClient.SendBonusAsync(DateOnly.Parse(date), hour);
    }
}