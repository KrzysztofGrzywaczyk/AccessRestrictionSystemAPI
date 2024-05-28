
using System.Diagnostics.CodeAnalysis;

namespace AccessControlSystem.SharedKernel.ApplicationInsights;

[ExcludeFromCodeCoverage]
public class TelemetryFilterConfiguration : ITelemetryFilterConfiguration
{
    public bool EnableTelemetry { get; set; }

    public string? ApplicationName { get; set; }
}