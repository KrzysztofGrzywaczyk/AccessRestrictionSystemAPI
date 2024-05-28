
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.ApplicationInsights;
using System.Diagnostics.CodeAnalysis;

namespace AccessControlSystem.SharedKernel.ApplicationInsights;

[ExcludeFromCodeCoverage]
public static class TelemetryExtensions
{
    public static IServiceCollection AddAppInsightsTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        var appInsightsSection = configuration.GetSection(ConfigKey.ApplicationInsights);
        var telemetryFilterConfiguration = appInsightsSection.Get<TelemetryFilterConfiguration>() ?? new TelemetryFilterConfiguration();

        services
            .AddOptions<ApplicationInsightsServiceOptions>()
            .Bind(appInsightsSection);

        services
            .AddOptions<ApplicationInsightsLoggerOptions>()
            .Bind(appInsightsSection);

        services.AddApplicationInsightsTelemetry();
        services.AddApplicationInsightsTelemetryProcessor<TelemetryFilterProcessor>();

        services.AddSingleton<ITelemetryFilterConfiguration>(telemetryFilterConfiguration);
        services.AddSingleton<ITelemetryInitializer, TelemetryRoleNameInitializer>();

        return services;
    }

    public static IServiceCollection AddAppInsightsCustomConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var appInsightsSection = configuration.GetSection(ConfigKey.ApplicationInsights);
        return services
            .AddTelemetryInitializer(appInsightsSection);
    }

    private static IServiceCollection AddTelemetryInitializer(this IServiceCollection services, IConfigurationSection appInsightsSection)
    {
        var telemetryFilterConfiguration = appInsightsSection.Get<TelemetryFilterConfiguration>() ?? new TelemetryFilterConfiguration();
        services.AddSingleton<ITelemetryFilterConfiguration>(telemetryFilterConfiguration);
        services.AddSingleton<ITelemetryInitializer, TelemetryRoleNameInitializer>();

        return services;
    }
}
