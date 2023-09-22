using Hangfire;
using Hangfire.Server;
using Hangfire.Storage;
using NSubstitute;

namespace Tgs.Schedule.UnitTest;

public class PerformContextMock
{
    public Lazy<PerformContext> GetPerformContextMock(DateTime? createdOn = null)
    {
        var storage = Substitute.For<JobStorage>();
        var connection = Substitute.For<IStorageConnection>();
        var backgroundJob = Substitute.For<BackgroundJobMock>();
        var cancellationToken = Substitute.For<IJobCancellationToken>();

        backgroundJob.CreatedAt = createdOn ?? DateTime.UtcNow;

        return new Lazy<PerformContext>(
            () => new PerformContext(storage, connection, backgroundJob.Object, cancellationToken));
    }
}