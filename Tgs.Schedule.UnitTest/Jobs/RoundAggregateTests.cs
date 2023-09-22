using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Tgs.Report.Aggregate.Client;
using Tgs.Report.Aggregate.Client.Models;
using Tgs.Schedule.Service.Jobs;

namespace Tgs.Schedule.UnitTest.Jobs;

[TestFixture]
[Parallelizable]
public class RoundAggregateTests
{
    [Test]
    public async Task Create_Round_Aggregate()
    {
        var fakePerformContextMock = Substitute.For<PerformContextMock>();
        var fakeReportAggregateClient = Substitute.For<IReportAggregateClient>();

        var sut = new RoundAggregateActivator(
            fakeReportAggregateClient,
            NullLogger<RoundAggregateActivator>.Instance);

        var request = fakePerformContextMock.GetPerformContextMock();

        await sut.CreateRoundAggregateAsync(request.Value);

        _ = await fakeReportAggregateClient.Received(1).CreateRoundAggregateAsync(Arg.Any<CreateRoundAggregateModel>());
    }
}