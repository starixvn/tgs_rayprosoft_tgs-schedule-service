using Microsoft.Extensions.Logging.Abstractions;

using NSubstitute;

using Tgs.Promotion.RebateService.Client;
using Tgs.Schedule.Service.JobManagements;

namespace Tgs.Schedule.UnitTest.JobManagements;

[TestFixture]
[Parallelizable]
public class PromotionRebateManagementTests
{
    [Test]
    public async Task Settle_Promotion_Rebate()
    {
        var fakePromotionRebateClient = Substitute.For<IPromotionRebateClient>();

        var sut = new PromotionRebateManagement(
            fakePromotionRebateClient,
            NullLogger<PromotionRebateManagement>.Instance);

        const string date = "2023/09/01";

        await sut.SettlePromotionRebateAsync(date);

        await fakePromotionRebateClient.Received(1)
            .SettleAsync(Arg.Is(DateOnly.Parse(date)), Arg.Is(16));
    }

    [Test]
    public async Task Send_Promotion_Rebate()
    {
        var fakePromotionRebateClient = Substitute.For<IPromotionRebateClient>();

        var sut = new PromotionRebateManagement(
            fakePromotionRebateClient,
            NullLogger<PromotionRebateManagement>.Instance);

        const string date = "2023/09/01";

        await sut.SendPromotionRebateAsync(date);

        await fakePromotionRebateClient.Received(1)
            .SendBonusAsync(Arg.Is(DateOnly.Parse(date)), Arg.Is(16));
    }
}