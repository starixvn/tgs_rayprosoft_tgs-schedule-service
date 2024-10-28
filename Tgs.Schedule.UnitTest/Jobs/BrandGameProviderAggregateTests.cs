using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Tgs.BrandCertificate;
using Tgs.BrandCertificate.Clients;
using Tgs.Schedule.Service.Jobs;
using TGS.Core.Infrastructures.Nats.Interface;

namespace Tgs.Schedule.UnitTest.Jobs;

[TestFixture]
[Parallelizable]
public class BrandGameProviderAggregateTests
{
    [Test]
    public async Task Generate_Brand_Game_Provider_Aggregate()
    {
        var fakePerformContextMock = Substitute.For<PerformContextMock>();
        var fakeMessageManager = Substitute.For<IMessageManager>();
        var fakeBrandServiceClient = Substitute.For<IBrandServiceClient>();

        var sut = new BrandGameProviderAggregateDailyActivator(
            fakeBrandServiceClient,
            fakeMessageManager,
            NullLogger<BrandGameProviderAggregateDailyActivator>.Instance);

        fakeBrandServiceClient.QueryBrandListAsync(Arg.Any<BrandListQuery>()).Returns(
            new[] { 
                new BrandListItem(Guid.NewGuid(), BrandStatus.Active, "", "", DateTime.Now, DateTime.Now),
                new BrandListItem(Guid.NewGuid(), BrandStatus.Archived, "", "", DateTime.Now, DateTime.Now)
            }.ToAsyncEnumerable());

        var request = fakePerformContextMock.GetPerformContextMock();

        await sut.GenerateBrandGameProviderDailyAggregateAsync(request.Value);

        _ = fakeMessageManager.Received(1).PublishAsync("tgs.report.aggregate.brand.game.provider.schedule",Arg.Any<object>());
    }
}