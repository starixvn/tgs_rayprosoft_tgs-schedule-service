using Microsoft.Extensions.Logging.Abstractions;

using NSubstitute;

using Tgs.ProfitRebate.Client;
using Tgs.ProfitRebate.Client.Models;
using Tgs.Schedule.Service.Jobs;

namespace Tgs.Schedule.UnitTest.Jobs;

[TestFixture]
[Parallelizable]
public class PlayerAllAgentRebateTests
{
    [Test]
    public async Task Create_Player_All_Agent_Rebate()
    {
        var fakePerformContextMock = Substitute.For<PerformContextMock>();
        var fakeProfitRebateClient = Substitute.For<IProfitRebateClient>();

        var sut = new PlayerAllAgentRebateActivator(
            fakeProfitRebateClient,
            NullLogger<PlayerAllAgentRebateActivator>.Instance);

        var request = fakePerformContextMock.GetPerformContextMock();

        await sut.CreatePlayerAllAgentRebateAsync(request.Value);

        _ = await fakeProfitRebateClient.Received(1)
            .CreatePlayerAllAgentRebateAsync(Arg.Any<CreateAllAgentPlayerRebatesRequest>());
    }
}