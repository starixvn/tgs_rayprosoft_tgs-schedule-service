using System.Reflection;
using FluentdForward.OpenTelemetry.MessagePack;
using Hangfire;
using Hangfire.Console;
using Hangfire.Dashboard.Management.v2;
using Hangfire.Dashboard.Management.v2.Support;
using Hangfire.Redis.StackExchange;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Tgs.Brand.GameProvider.Client.DependencyInjection;
using Tgs.Game.Type.Round.Aggregate.Client.DependencyInjection;
using Tgs.Player.All.Agent.Client.DependencyInjection;
using Tgs.Promotion.ReliefMoney.Client.DependencyInjection;
using Tgs.Schedule.Service;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseSentry();

builder.Logging
    .AddOpenTelemetry(options =>
    {
        options.IncludeFormattedMessage = true;
        options.ParseStateValues = true;
        _ = options.AddFluentdForwardExporter(fluentOptions =>
        {
            var nodeIp = builder.Configuration.GetValue<string>("Otel:NodeIp")!;
            fluentOptions.Host = nodeIp;
            fluentOptions.Tag = builder.Configuration.GetValue<string>("Otel:ServiceName")!;
            fluentOptions.UseMessagePack(LogRecordFormatterResolver.GetResolverInstanceWithExtendInfo(
                "MachineInfo",
                () => new
                {
                    Environment.MachineName
                }));
        });
    });

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
    .AddPlayerAllAgentClient((options, sp) =>
    {
        var config = sp.GetRequiredService<IConfiguration>();
        options.Url = config.GetValue<Uri>("Schedule:PlayerAllAgent")!;
    })
    .AddProfitRebateClient(builder.Configuration)
    .AddAgentDividendClient((options, sp) =>
    {
        var config = sp.GetRequiredService<IConfiguration>();
        options.BaseUrl = config.GetValue<Uri>("Schedule:AgentDividend")!;
    });

builder.Services
    .AddOpenTelemetry()
    .ConfigureResource(_ => _.AddService(builder.Configuration.GetValue<string>("Otel:ServiceName")))
    .WithTracing(tracingBuilder => tracingBuilder
        .AddSource("Tgs.*")
        .AddSource("TGS.*")
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddProfileViewExporter()
        .AddOtlpExporter(options => options.Endpoint = builder.Configuration.GetValue<Uri>("Otel:Collector")))
    .WithMetrics()
    .Services
    .Configure<AspNetCoreTraceInstrumentationOptions>(options => options.Filter =
        httpContext => !httpContext.Request.Path.HasValue ||
                       (!httpContext.Request.Path.StartsWithSegments("/swagger") &&
                        !httpContext.Request.Path.StartsWithSegments("/health") &&
                        !httpContext.Request.Path.StartsWithSegments("/profiler/") &&
                        !new[] { "html", "ico", "png", "jpg" }.Any(
                            ext => httpContext.Request.Path.Value.EndsWith($".{ext}"))));


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

var app = builder.Build();

app.UseHangfireDashboard(
    "/hangfire",
    new DashboardOptions()
    {
        Authorization =
        new[]
        {
            new IgnoreHangfireAuthorizationFilter()
        }
    });

app.MapHealthChecks("/healthz");

app.JobRegistration();

SentrySdk.CaptureMessage($"Started result Schedule service at {DateTime.Now.ToString()}");

app.Run();
