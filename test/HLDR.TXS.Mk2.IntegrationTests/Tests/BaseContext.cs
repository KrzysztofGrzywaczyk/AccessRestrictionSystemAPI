using AccessControlSystem.IntegrationTests.Core.Logging;
using System.Net;

namespace AccessControlSystem.IntegrationTests.Tests;

public abstract class BaseContext : IAsyncLifetime
{
    protected BaseContext(AssemblySharedFixture assemblySharedFixture, Logger logger)
    {
        AssemblySharedFixture = assemblySharedFixture;
        Logger = logger;
    }

    protected AssemblySharedFixture AssemblySharedFixture { get; }

    protected Logger Logger { get; }

    protected HttpResponseMessage? LastApiResponse { get; set; }

    public virtual Task InitializeAsync() => Task.CompletedTask;

    public virtual Task DisposeAsync() => Task.CompletedTask;

    public async Task ThenStatusCodeIsReturned(HttpStatusCode statusCode)
    {
        var content = await LastApiResponse!.Content.ReadAsStringAsync();

        LastApiResponse.Should().NotBeNull();
        LastApiResponse!.StatusCode.Should().Be(statusCode);
    }
}