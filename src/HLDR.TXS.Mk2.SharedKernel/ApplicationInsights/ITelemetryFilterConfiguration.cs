
namespace AccessControlSystem.SharedKernel.ApplicationInsights;

public interface ITelemetryFilterConfiguration
{
    bool EnableTelemetry { get; set; }

    string? ApplicationName { get; set; }
}