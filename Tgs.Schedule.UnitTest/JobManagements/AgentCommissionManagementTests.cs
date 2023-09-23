using Microsoft.Extensions.Logging.Abstractions;

using NSubstitute;

using Tgs.ProfitRebate.Client;
using Tgs.ProfitRebate.Client.Models;
using Tgs.Schedule.Service.JobManagements;

namespace Tgs.Schedule.UnitTest.JobManagements;

[TestFixture]
[Parallelizable]
public class AgentCommissionManagementTests
{
    [Test]
    public async Task Create_Agent_Commissions()
    {
        var fakeProfitRebateClient = Substitute.For<IProfitRebateClient>();

        var sut = new AgentCommissionManagement(
            fakeProfitRebateClient,
            NullLogger<AgentCommissionManagement>.Instance);

        var brandCode = "GPI";

        await sut.CreateAgentCommissionsAsync(brandCode);

        _ = await fakeProfitRebateClient.Received(1)
            .CreateAgentDividendAsync(Arg.Is<CreateAgentDividendsRequest>(x => x.BrandCode == brandCode));
    }
}