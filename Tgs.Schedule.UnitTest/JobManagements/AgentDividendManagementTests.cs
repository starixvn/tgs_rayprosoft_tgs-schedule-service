using Microsoft.Extensions.Logging.Abstractions;

using NSubstitute;
using Tgs.Agent.Dividend.Client;
using Tgs.ProfitRebate.Client;
using Tgs.ProfitRebate.Client.Models;
using Tgs.Schedule.Service.JobManagements;

namespace Tgs.Schedule.UnitTest.JobManagements;

[TestFixture]
[Parallelizable]
public class AgentDividendManagementTests
{
    [Test]
    public async Task Create_Agent_Commissions()
    {
        var fakeAgentDividendClient = Substitute.For<IAgentDividendClient>();

        var sut = new AgentDividendManagement(
            fakeAgentDividendClient,
            NullLogger<AgentDividendManagement>.Instance);

        var brandId = Guid.NewGuid();

        await sut.CreateAgentDividendsAsync(brandId);

        await fakeAgentDividendClient.Received(1)
            .GenerateAgentDividend(Arg.Is<Guid>(brandId), Arg.Any<CancellationToken>());
    }
}