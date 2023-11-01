using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Tgs.Player.All.Agent.Client;
using Tgs.Schedule.Service;
using Tgs.Schedule.Service.Jobs;

namespace Tgs.Schedule.UnitTest.Jobs;

[TestFixture]
[Parallelizable]
public class PlayerAllAgentRebateTests
{
    [Test]
    public async Task Create_Player_All_Agent_Rebate_Daily_Settlement()
    {
        var fakePerformContextMock = Substitute.For<PerformContextMock>();
        var fakePlayerAllAgentScheduleClient = Substitute.For<IPlayerAllAgentScheduleClient>();

        var sut = new PlayerAllAgentRebateSettlementActivator(
            fakePlayerAllAgentScheduleClient,
            NullLogger<PlayerAllAgentRebateSettlementActivator>.Instance);

        var request = fakePerformContextMock.GetPerformContextMock();

        await sut.CreatePlayerAllAgentRebateDailySettlementAsync(request.Value);

        await fakePlayerAllAgentScheduleClient.Received(1)
            .CreateDailyPlayerAllAgentRebatesAsync();
    }

    [Test]
    public async Task Create_Player_All_Agent_Rebate_Weekly_Settlement()
    {
        var fakePerformContextMock = Substitute.For<PerformContextMock>();
        var fakePlayerAllAgentScheduleClient = Substitute.For<IPlayerAllAgentScheduleClient>();

        var sut = new PlayerAllAgentRebateSettlementActivator(
            fakePlayerAllAgentScheduleClient,
            NullLogger<PlayerAllAgentRebateSettlementActivator>.Instance);

        var request = fakePerformContextMock.GetPerformContextMock();

        await sut.CreatePlayerAllAgentRebateWeeklySettlementAsync(request.Value);

        await fakePlayerAllAgentScheduleClient.Received(1)
            .CreateWeeklyPlayerAllAgentRebatesAsync();
    }

    [Test]
    public async Task Create_Player_All_Agent_Rebate_Monthly_Settlement()
    {
        var fakePerformContextMock = Substitute.For<PerformContextMock>();
        var fakePlayerAllAgentScheduleClient = Substitute.For<IPlayerAllAgentScheduleClient>();

        var sut = new PlayerAllAgentRebateSettlementActivator(
            fakePlayerAllAgentScheduleClient,
            NullLogger<PlayerAllAgentRebateSettlementActivator>.Instance);

        var request = fakePerformContextMock.GetPerformContextMock();

        await sut.CreatePlayerAllAgentRebateMonthlySettlementAsync(request.Value);

        await fakePlayerAllAgentScheduleClient.Received(1)
            .CreateMonthlyPlayerAllAgentRebatesAsync();
    }


    [Test]
    public async Task Create_Player_All_Agent_Rebate_Daily_Distribution()
    {
        var fakePerformContextMock = Substitute.For<PerformContextMock>();
        var fakePlayerAllAgentScheduleClient = Substitute.For<IPlayerAllAgentScheduleClient>();

        var sut = new PlayerAllAgentRebateDistributionActivator(
            fakePlayerAllAgentScheduleClient,
            NullLogger<PlayerAllAgentRebateDistributionActivator>.Instance);

        var request = fakePerformContextMock.GetPerformContextMock();

        await sut.CreatePlayerAllAgentRebateDailyDistributionAsync(request.Value);

        var from = DateOnly.FromDateTime(DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(8)).DateTime);

        await fakePlayerAllAgentScheduleClient.Received(1)
            .DistributeRebatesAsync(from, from);
    }

    [Test]
    public async Task Create_Player_All_Agent_Rebate_Weekly_Distribution()
    {
        var fakePerformContextMock = Substitute.For<PerformContextMock>();
        var fakePlayerAllAgentScheduleClient = Substitute.For<IPlayerAllAgentScheduleClient>();

        var sut = new PlayerAllAgentRebateDistributionActivator(
            fakePlayerAllAgentScheduleClient,
            NullLogger<PlayerAllAgentRebateDistributionActivator>.Instance);

        var request = fakePerformContextMock.GetPerformContextMock();

        await sut.CreatePlayerAllAgentRebateWeeklyDistributionAsync(request.Value);

        var from = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-7)
            .GetMonday()
            .ToOffset(TimeSpan.FromHours(8))
            .DateTime);

        var to = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-7)
            .GetSunday()
            .ToOffset(TimeSpan.FromHours(8))
            .DateTime);

        await fakePlayerAllAgentScheduleClient.Received(1)
            .DistributeRebatesAsync(from, to);
    }

    [Test]
    public async Task Create_Player_All_Agent_Rebate_Monthly_Distribution()
    {
        var fakePerformContextMock = Substitute.For<PerformContextMock>();
        var fakePlayerAllAgentScheduleClient = Substitute.For<IPlayerAllAgentScheduleClient>();

        var sut = new PlayerAllAgentRebateDistributionActivator(
            fakePlayerAllAgentScheduleClient,
            NullLogger<PlayerAllAgentRebateDistributionActivator>.Instance);

        var request = fakePerformContextMock.GetPerformContextMock();

        await sut.CreatePlayerAllAgentRebateMonthlyDistributionAsync(request.Value);

        var from = DateOnly.FromDateTime(DateTimeOffset.Now.AddMonths(-1)
            .GetFirstDayOfMonth()
            .ToOffset(TimeSpan.FromHours(8))
            .DateTime);

        var to = DateOnly.FromDateTime(DateTimeOffset.Now.AddMonths(-1)
            .GetLastDayOfMonth()
            .ToOffset(TimeSpan.FromHours(8))
            .DateTime);

        await fakePlayerAllAgentScheduleClient.Received(1)
            .DistributeRebatesAsync(from, to);
    }
}