using Hangfire.Dashboard;

namespace Tgs.Schedule.Service;

public sealed class IgnoreHangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}