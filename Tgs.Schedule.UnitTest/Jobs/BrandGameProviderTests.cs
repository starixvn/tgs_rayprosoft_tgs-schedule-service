using Microsoft.Extensions.Logging.Abstractions;

using NSubstitute;

using Tgs.Brand.GameProvider.Client;
using Tgs.Schedule.Service.Jobs;

namespace Tgs.Schedule.UnitTest.Jobs;

[TestFixture]
[Parallelizable]
public class BrandGameProviderTests
{
    [Test]
    public async Task Change_Brand_Game_Provider_Status()
    {
        var fakePerformContextMock = Substitute.For<PerformContextMock>();
        var fakeBrandGameProviderClient = Substitute.For<IBrandGameProviderClient>();

        var sut = new BrandGameProviderActivator(
            fakeBrandGameProviderClient,
            NullLogger<BrandGameProviderActivator>.Instance);

        var request = fakePerformContextMock.GetPerformContextMock();

        await sut.ChangeStatusByScheduleAsync(request.Value);

        _ = fakeBrandGameProviderClient.Received(1).ChangeStatusByScheduleAsync();
    }
}