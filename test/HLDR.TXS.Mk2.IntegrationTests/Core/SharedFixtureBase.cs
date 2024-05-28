using AccessControlSystem.IntegrationTests.Core.Configuration;
using AccessControlSystem.IntegrationTests.Core.Logging;
using Xunit.Abstractions;

namespace AccessControlSystem.IntegrationTests.Core;

public abstract class SharedFixtureBase(IMessageSink messageSink) : IAsyncDisposable
{
    public TestsConfiguration Configuration { get; } = new TestsConfiguration();

    protected Logger Logger { get; } = new(new MessageSinkOutput(messageSink));

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}