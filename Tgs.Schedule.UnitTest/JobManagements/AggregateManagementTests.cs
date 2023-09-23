using Microsoft.Extensions.Logging.Abstractions;

using NSubstitute;

using Tgs.Game.Type.Round.Aggregate.Client;
using Tgs.Report.Aggregate.Client;
using Tgs.Report.Aggregate.Client.Models;
using Tgs.Schedule.Service.JobManagements;

namespace Tgs.Schedule.UnitTest.JobManagements;

[TestFixture]
[Parallelizable]
public class AggregateManagementTests
{
    [Test]
    public async Task Manual_Create_Round_Aggregate()
    {
        var fakeReportAggregateClient = Substitute.For<IReportAggregateClient>();
        var fakeGameTypeRoundClient = Substitute.For<IGameTypeRoundClient>();

        var sut = new AggregateManagement(
            fakeReportAggregateClient,
            fakeGameTypeRoundClient,
            NullLogger<AggregateManagement>.Instance);

        var date = "2023/09/01";

        await sut.CreateRoundAggregateAsync(date, 16);

        _ = await fakeReportAggregateClient.Received(1)
            .ManualCreateRoundAggregateAsync(
                Arg.Is<ManualCreateUpdateRoundAggregateModel>(x => x.Date == date && x.Hour == 16));
    }

    [Test]
    public async Task Manual_Create_Player_Aggregate()
    {
        var fakeReportAggregateClient = Substitute.For<IReportAggregateClient>();
        var fakeGameTypeRoundClient = Substitute.For<IGameTypeRoundClient>();

        var sut = new AggregateManagement(
            fakeReportAggregateClient,
            fakeGameTypeRoundClient,
            NullLogger<AggregateManagement>.Instance);

        var date = "2023/09/01";

        await sut.CreatePlayerAggregateAsync(date, 16);

        _ = await fakeReportAggregateClient.Received(1)
            .ManualCreatePlayerAggregateAsync(
                Arg.Is<ManualCreateUpdatePlayerAggregateModel>(x => x.Date == date && x.Hour == 16));
    }

    [Test]
    public async Task Create_Game_Type_Round_Aggregate_By_Range()
    {
        var fakeReportAggregateClient = Substitute.For<IReportAggregateClient>();
        var fakeGameTypeRoundClient = Substitute.For<IGameTypeRoundClient>();

        var sut = new AggregateManagement(
            fakeReportAggregateClient,
            fakeGameTypeRoundClient,
            NullLogger<AggregateManagement>.Instance);

        var date = "2023/09/01 00:01:30";

        await sut.CreateGameTypeRoundAggregateAsync(date, 2);

        Received.InOrder(() =>
        {
            fakeGameTypeRoundClient.Received()
                .GenerateGameTypeRoundsAsync(Arg.Is(DateTime.Parse("2023/8/31 23:30:00Z").ToUniversalTime()),
                    Arg.Is(DateTime.Parse("2023/8/31 23:45:00Z").ToUniversalTime()));
            fakeGameTypeRoundClient.Received()
                .GenerateGameTypeRoundsAsync(Arg.Is(DateTime.Parse("2023/8/31 23:45:00Z").ToUniversalTime()),
                    Arg.Is(DateTime.Parse("2023/9/01 00:00:00Z").ToUniversalTime()));
        });
    }
}