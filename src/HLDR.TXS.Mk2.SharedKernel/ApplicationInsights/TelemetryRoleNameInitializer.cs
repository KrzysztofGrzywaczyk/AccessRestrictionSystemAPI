
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AccessControlSystem.SharedKernel.ApplicationInsights;

[ExcludeFromCodeCoverage]
public class TelemetryRoleNameInitializer : ITelemetryInitializer
{
    private readonly ITelemetryFilterConfiguration filterConfiguration;

    private readonly string _defaultRoleName;

    public TelemetryRoleNameInitializer(ITelemetryFilterConfiguration filterConfiguration)
    {
        this.filterConfiguration = filterConfiguration;

        var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
        _defaultRoleName = assembly.GetName().FullName;
    }

    public void Initialize(ITelemetry telemetry)
    {
        if (!string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
        {
            return;
        }

        telemetry.Context.Cloud.RoleName = !string.IsNullOrEmpty(filterConfiguration.ApplicationName) ? filterConfiguration.ApplicationName : _defaultRoleName;
    }
}