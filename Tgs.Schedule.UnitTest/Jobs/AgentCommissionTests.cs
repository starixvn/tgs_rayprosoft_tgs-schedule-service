using Microsoft.Extensions.Logging.Abstractions;

using NSubstitute;
using Tgs.Agent.Dividend.Client;
using Tgs.BrandCertificate;
using Tgs.BrandCertificate.Clients;
using Tgs.ProfitRebate.Client;
using Tgs.ProfitRebate.Client.Models;
using Tgs.Schedule.Service.Jobs;

namespace Tgs.Schedule.UnitTest.Jobs;

[TestFixture]
[Parallelizable]
public class AgentCommissionTests
{
    [Test]
    public async Task Create_Agent_Commissions()
    {
        var fakePerformContextMock = Substitute.For<PerformContextMock>();
        var fakeAgentDividendClient = Substitute.For<IAgentDividendClient>();

        var sut = new AgentCommissionActivator(
            fakeAgentDividendClient,
            NullLogger<AgentCommissionActivator>.Instance);

        var request = fakePerformContextMock.GetPerformContextMock();

        await sut.CreateAgentCommissionAsync(request.Value);

        await fakeAgentDividendClient.Received(1)
            .GenerateAgentDividend(Arg.Any<CancellationToken>());
    }
}