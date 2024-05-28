
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using System.Diagnostics.CodeAnalysis;

namespace AccessControlSystem.SharedKernel.ApplicationInsights;

[ExcludeFromCodeCoverage]
public class TelemetryFilterProcessor(ITelemetryProcessor next, ITelemetryFilterConfiguration filterConfiguration) : ITelemetryProcessor
{
    public void Process(ITelemetry item)
    {
        if (item is OperationTelemetry)
        {
            if (filterConfiguration.EnableTelemetry)
            {
                next.Process(item);
            }

            return;
        }

        next.Process(item);
    }
}