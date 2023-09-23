using System.Reflection;
using Hangfire;
using Hangfire.Console;
using Hangfire.Dashboard.Management.v2;
using Hangfire.Dashboard.Management.v2.Support;
using Hangfire.Redis.StackExchange;
using Tgs.Brand.GameProvider.Client.DependencyInjection;
using Tgs.Game.Type.Round.Aggregate.Client.DependencyInjection;
using Tgs.Promotion.ReliefMoney.Client.DependencyInjection;
using Tgs.Schedule.Service;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Configuration
        .AddJsonFile("/config/appsettings.json", true)
        .AddEnvironmentVariables();

    builder.Services
        .AddReportAggregateClient((options, sp) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            options.BaseUrl = config.GetValue<Uri>("Schedule:ReportAggregate")!;
        })
        .AddPromotionRebateClient((options, sp) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            options.Url = config.GetValue<Uri>("Schedule:PromotionRebate")!;
        })
        .AddPromotionReliefClient((options, sp) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            options.Url = config.GetValue<Uri>("Schedule:PromotionRelief")!;
        })
        .AddGameTypeRoundAggregateClient((options, sp) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            options.Url = config.GetValue<Uri>("Schedule:GameTypeRoundAggregate")!;
        })
        .AddBrandServiceClient((options, sp) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            options.BrandCertificateBaseUrl = config.GetValue<Uri>("Schedule:Brand")!;
        })
        .AddBrandGameProviderClient((options, sp) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            options.Url = config.GetValue<Uri>("Schedule:BrandGameProvider")!;
        })
        .AddProfitRebateClient(builder.Configuration);

    builder.Services.AddHangfire((sp, config) =>
        {
            var connectionString = builder.Configuration.GetConnectionString("Redis")!;

            var options = new RedisStorageOptions
            {
                Prefix = builder.Configuration.GetValue<string>("Hangfire:Prefix")
            };
            _ = config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseRedisStorage(connectionString, options)
                .UseConsole();

            config.UseManagementPages(new[] { Assembly.GetExecutingAssembly() });
        })
        .AddSingleton<IScheduleRecurringJob, ScheduleRecurringJob>();

    builder.Services.AddHangfireServer(options =>
    {
        var queues = new List<string> { "brand", "agent", "player", "aggregate" };

        queues.AddRange(JobsHelper.GetAllQueues());

        options.Queues = queues.Distinct().ToArray();
    });

    builder.Services.AddHealthChecks();
}


var app = builder.Build();
{
    app.UseHangfireDashboard();

    app.MapHangfireDashboard("/hangfire",
        new DashboardOptions
        {
            Authorization = new[]
            {
                new IgnoreHangfireAuthorizationFilter()
            }
        });


    _ = app.UseAuthorization();


    app.JobRegistration();

    app.MapHealthChecks("/healthz");

    _ = app.UseHangfireRegisterAsync();

    _ = app.UseRegister();

    app.Run();
}