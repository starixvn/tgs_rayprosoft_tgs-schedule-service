namespace Tgs.Schedule.Service;

public static class Extension
{
    public static IApplicationBuilder JobRegistration(this IApplicationBuilder app)
    {
        var applicationServices = app.ApplicationServices;

        var recurringJob = applicationServices.GetRequiredService<IScheduleRecurringJob>();

        recurringJob.RecurringJobs();

        return app;
    }
}