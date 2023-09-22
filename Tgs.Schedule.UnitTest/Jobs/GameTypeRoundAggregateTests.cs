using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Tgs.Game.Type.Round.Aggregate.Client;
using Tgs.Schedule.Service.Jobs;

namespace Tgs.Schedule.UnitTest.Jobs;

[TestFixture]
[Parallelizable]
public class GameTypeRoundAggregateTests
{
    [Test]
    public async Task Create_Game_Type_Round_Aggregate()
    {
        var fakePerformContextMock = Substitute.For<PerformContextMock>();
        var fakeGameTypeRoundClient = Substitute.For<IGameTypeRoundClient>();

        var sut = new GameTypeRoundAggregateActivator(
            fakeGameTypeRoundClient,
            NullLogger<GameTypeRoundAggregateActivator>.Instance);

        var request = fakePerformContextMock.GetPerformContextMock();

        await sut.GenerateGameTypeRoundsAsync(request.Value);

        _ = fakeGameTypeRoundClient.Received(4).GenerateGameTypeRoundsAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>());
    }
}