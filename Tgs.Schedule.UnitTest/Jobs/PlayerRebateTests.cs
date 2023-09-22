using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Tgs.ProfitRebate.Client;
using Tgs.ProfitRebate.Client.Models;
using Tgs.Schedule.Service.Jobs;

namespace Tgs.Schedule.UnitTest.Jobs;

[TestFixture]
[Parallelizable]
public class PlayerRebateTests
{
    [Test]
    public async Task Create_Game_Type_Round_Aggregate()
    {
        var fakePerformContextMock = Substitute.For<PerformContextMock>();
        var fakeProfitRebateClient = Substitute.For<IProfitRebateClient>();

        var sut = new PlayerRebateActivator(
            fakeProfitRebateClient,
            NullLogger<PlayerRebateActivator>.Instance);

        var request = fakePerformContextMock.GetPerformContextMock();

        await sut.CreatePlayerRebateAsync(request.Value);

        _ = await fakeProfitRebateClient.Received(1).CreatePlayerRebateAsync(Arg.Any<CreatePlayerRebateRequest>());
    }
}