using Microsoft.Extensions.Logging.Abstractions;

using NSubstitute;

using Tgs.ProfitRebate.Client;
using Tgs.ProfitRebate.Client.Models;
using Tgs.Schedule.Service.JobManagements;

namespace Tgs.Schedule.UnitTest.JobManagements;

[TestFixture]
[Parallelizable]
public class PlayerRebateManagementTests
{
    [Test]
    public async Task Create_Player_Rebate()
    {
        var fakeProfitRebateClient = Substitute.For<IProfitRebateClient>();

        var sut = new PlayerRebateManagement(
            fakeProfitRebateClient,
            NullLogger<PlayerRebateManagement>.Instance);

        var date = "2023/09/01";

        await sut.CreatePlayerRebateAsync(date);

        _ = await fakeProfitRebateClient.Received(1)
            .CreatePlayerRebateAsync(Arg.Is<CreatePlayerRebateRequest>(x => x.Date == date));
    }

    [Test]
    public async Task Create_Player_All_Agent_Rebate()
    {
        var fakeProfitRebateClient = Substitute.For<IProfitRebateClient>();

        var sut = new PlayerRebateManagement(
            fakeProfitRebateClient,
            NullLogger<PlayerRebateManagement>.Instance);

        var date = "2023/09/01";

        await sut.CreatePlayerAllAgentRebateAsync(date);

        _ = await fakeProfitRebateClient.Received(1)
            .CreatePlayerAllAgentRebateAsync(Arg.Is<CreateAllAgentPlayerRebatesRequest>(x => x.Date == date));
    }
}