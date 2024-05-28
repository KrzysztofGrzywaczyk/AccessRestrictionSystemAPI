using Xunit.Sdk;

namespace AccessControlSystem.IntegrationTests.Core.Logging;

public class MessageSinkOutput : ILogOutput
{
    private readonly IMessageSink _sink;

    public MessageSinkOutput(IMessageSink sink) => _sink = sink;

    public void WriteLine(string message) => _sink.OnMessage(new DiagnosticMessage(message));
}
