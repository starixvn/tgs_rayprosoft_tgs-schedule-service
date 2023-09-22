using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Tgs.Report.Aggregate.Client;
using Tgs.Report.Aggregate.Client.Models;
using Tgs.Schedule.Service.Jobs;

namespace Tgs.Schedule.UnitTest.Jobs;

[TestFixture]
[Parallelizable]
public class PlayerAggregateTests
{
    [Test]
    public async Task Create_Player_Aggregate()
    {
        var fakePerformContextMock = Substitute.For<PerformContextMock>();
        var fakeReportAggregateClient = Substitute.For<IReportAggregateClient>();

        var sut = new PlayerAggregateActivator(
            fakeReportAggregateClient,
            NullLogger<PlayerAggregateActivator>.Instance);

        var request = fakePerformContextMock.GetPerformContextMock();

        await sut.CreatePlayerAggregateAsync(request.Value);

        _ = await fakeReportAggregateClient.Received(1)
            .CreatePlayerAggregateAsync(Arg.Any<CreateUpdatePlayerAggregateModel>());
    }
}