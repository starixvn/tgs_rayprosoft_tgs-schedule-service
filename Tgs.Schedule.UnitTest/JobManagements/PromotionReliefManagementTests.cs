using Microsoft.Extensions.Logging.Abstractions;

using NSubstitute;

using Tgs.Promotion.ReliefMoney.Client;
using Tgs.Schedule.Service.JobManagements;

namespace Tgs.Schedule.UnitTest.JobManagements;

[TestFixture]
[Parallelizable]
public class PromotionReliefManagementTests
{
    [Test]
    public async Task Settle_Promotion_Relief()
    {
        var fakePromotionReliefClient = Substitute.For<IPromotionReliefClient>();

        var sut = new PromotionReliefManagement(
            fakePromotionReliefClient,
            NullLogger<PromotionReliefManagement>.Instance);

        const string date = "2023/09/01";

        await sut.SettlePromotionReliefAsync(date);

        await fakePromotionReliefClient.Received(1)
            .SettleAsync(Arg.Is(DateOnly.Parse(date)), Arg.Is(16));
    }

    [Test]
    public async Task Send_Promotion_Relief()
    {
        var fakePromotionReliefClient = Substitute.For<IPromotionReliefClient>();

        var sut = new PromotionReliefManagement(
            fakePromotionReliefClient,
            NullLogger<PromotionReliefManagement>.Instance);

        const string date = "2023/09/01";

        await sut.SendPromotionReliefAsync(date);

        await fakePromotionReliefClient.Received(1)
            .SendBonusAsync(Arg.Is(DateOnly.Parse(date)), Arg.Is(16));
    }
}