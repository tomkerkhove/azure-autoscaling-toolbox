using Arcus.Observability.Correlation;
using Arcus.Observability.Telemetry.Core;
using Arcus.Observability.Telemetry.Serilog.Filters;
using FunctionApp1;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

[assembly: FunctionsStartup(typeof(Startup))]
namespace FunctionApp1
{
    public class Startup : FunctionsStartup
    {
        public static ServiceProvider Services { get; private set; }

        public const string ComponentName = "Autoscaling Metrics";

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = builder.Services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            builder.Services.AddLogging();
            builder.Services.AddCorrelation();
            Services = builder.Services.BuildServiceProvider();
            var instrumentationKey = GetApplicationInsightsTelemetryKey(configuration);

            AddLogging(builder, configuration, instrumentationKey);
        }

        protected virtual string GetApplicationInsightsTelemetryKey(IConfiguration config)
        {
            var instrumentationKey = config.GetValue<string>("APPINSIGHTS_INSTRUMENTATIONKEY");
            return instrumentationKey;
        }

        protected virtual void AddLogging(IFunctionsHostBuilder builder, IConfiguration configuration, string instrumentationKey)
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithComponentName(ComponentName)
                .Enrich.WithCorrelationInfo(new DefaultCorrelationInfoAccessor())
                .WriteTo.Logger(consoleLoggerConfiguration =>
                {
                    // For our logs we are not interested in requests, dependencies and metrics
                    // This only increases the cost on our logs
                    consoleLoggerConfiguration.Filter.With(TelemetryTypeFilter.On(TelemetryType.Request))
                        .Filter.With(TelemetryTypeFilter.On(TelemetryType.Dependency))
                        .Filter.With(TelemetryTypeFilter.On(TelemetryType.Metrics))
                        .WriteTo.Console();
                })
                .WriteTo.AzureApplicationInsights(instrumentationKey)
                .CreateLogger();

            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProvidersExceptFunctionProviders();
                loggingBuilder.AddSerilog(logger);
            });
        }
    }
}