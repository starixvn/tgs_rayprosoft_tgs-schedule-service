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
public class AgentDividendTests
{
    [Test]
    public async Task Create_Agent_Commissions()
    {
        var fakePerformContextMock = Substitute.For<PerformContextMock>();
        var fakeAgentDividendClient = Substitute.For<IAgentDividendClient>();

        var sut = new AgentDividendActivator(
            fakeAgentDividendClient,
            NullLogger<AgentDividendActivator>.Instance);

        var request = fakePerformContextMock.GetPerformContextMock();

        await sut.CreateAgentDividendAsync(request.Value);

        await fakeAgentDividendClient.Received(1)
            .GenerateAgentDividend(Arg.Any<CancellationToken>());
    }
}