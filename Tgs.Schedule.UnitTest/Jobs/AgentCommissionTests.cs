using Microsoft.Extensions.Logging.Abstractions;

using NSubstitute;

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
        var fakeBrandServiceClient = Substitute.For<IBrandServiceClient>();
        var fakeProfitRebateClient = Substitute.For<IProfitRebateClient>();

        var sut = new AgentCommissionActivator(
            fakeBrandServiceClient,
            fakeProfitRebateClient,
            NullLogger<AgentCommissionActivator>.Instance);

        fakeBrandServiceClient.QueryBrandListAsync(Arg.Any<BrandListQuery>())
            .Returns(new BrandListItem[]
                {
                    new(Guid.NewGuid(), BrandStatus.Active, null, null, DateTime.Now, DateTime.Now)
                }
                .ToAsyncEnumerable());

        fakeBrandServiceClient.GetBrandInfoAsync(Arg.Any<Guid>()).Returns(new BrandInfo { Code = "GPI" });

        var request = fakePerformContextMock.GetPerformContextMock();

        await sut.CreateAgentCommissionAsync(request.Value);

        _ = fakeBrandServiceClient.Received(1).QueryBrandListAsync(Arg.Any<BrandListQuery>());
        _ = await fakeProfitRebateClient.Received(1)
            .CreateAgentDividendAsync(Arg.Any<CreateAgentDividendsRequest>());
    }
}