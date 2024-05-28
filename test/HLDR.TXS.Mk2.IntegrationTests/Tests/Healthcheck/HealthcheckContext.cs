using AccessControlSystem.IntegrationTests.Components;
using AccessControlSystem.IntegrationTests.Core.Configuration;
using AccessControlSystem.IntegrationTests.Core.Logging;

namespace AccessControlSystem.IntegrationTests.Tests.Healthcheck;

public class HealthcheckContext(AssemblySharedFixture assemblySharedFixture, Logger logger) : BaseContext(assemblySharedFixture, logger)
{
    private readonly HealthCheckApiClient _apiClient = new(
        assemblySharedFixture.HttpClient,
        assemblySharedFixture.Configuration.GetProperty(ConfigKey.TestApiAddress),
        assemblySharedFixture.Configuration.GetProperty(ConfigKey.ApiKey));

    public Task WhenQueriedHealthcheck()
    {
        Logger.Info(this, $"Queried healthcheck");
        var response = _apiClient.GetHealthcheck();

        LastApiResponse = response.Result;

        return Task.CompletedTask;
    }

    public Task WhenQueriedHealthcheckWithNoDependencies()
    {
        Logger.Info(this, $"Queried healthcheck with no-dependencies");
        var response = _apiClient.GetHealthcheckWithNoDependencies();

        LastApiResponse = response.Result;

        return Task.CompletedTask;
    }
}