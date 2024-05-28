using AccessControlSystem.IntegrationTests.Core;

namespace AccessControlSystem.IntegrationTests;

/// <summary>
/// This class holds references to all components that should live throughout the whole test session.
/// </summary>
public class AssemblySharedFixture : SharedFixtureBase
{
    public AssemblySharedFixture(IMessageSink messageSink)
        : base(messageSink)
    {
        Logger.Info(this, "Creating shared HTTP client");
        HttpClient = new HttpClient();
    }

    public HttpClient HttpClient { get; }
}
