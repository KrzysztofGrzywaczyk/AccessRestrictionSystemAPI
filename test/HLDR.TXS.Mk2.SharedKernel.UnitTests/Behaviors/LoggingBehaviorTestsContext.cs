using AccessControlSystem.SharedKernel.Behaviors;
using AccessControlSystem.SharedKernel.JsonSerialization;
using AccessControlSystem.SharedKernel.Logging;
using AccessControlSystem.UnitTests;
using MediatR;
using NSubstitute;
using System.Text.Json;

namespace AccessControlSystem.SharedKernel.UnitTests.Behaviors;

public class LoggingBehaviorTestsContext : UnitTestsContextBase
{
    private readonly ILogger<LoggingBehavior<DummyRequest, DummyResponse>> _logger;

    public LoggingBehaviorTestsContext()
    {
        _logger = Substitute.For<ILogger<LoggingBehavior<DummyRequest, DummyResponse>>>();
        RequestHandlerDelegate = Substitute.For<RequestHandlerDelegate<DummyResponse>>();
        RequestHandlerDelegate().Returns(Task.FromResult(new DummyResponse()));
        LoggingBehavior = new LoggingBehavior<DummyRequest, DummyResponse>(_logger);
    }

    public RequestHandlerDelegate<DummyResponse> RequestHandlerDelegate { get; }

    public LoggingBehavior<DummyRequest, DummyResponse> LoggingBehavior { get; }

    public void VerifyDebugCalled(DummyRequest request)
    {
        Func<Func<object[]>, bool> verifyParameters = f =>
        {
            var parameters = f();
            return parameters[0].ToString() == "DummyRequest" && parameters[1].ToString() == JsonSerializer.Serialize(request, JsonSerializerOptionsProvider.Sensitive);
        };

        _logger.Received(1).LogDebug("Handler for {requestType} triggered with request {request}", Arg.Is<Func<object[]>>(f => verifyParameters(f)));
    }

    public void VerifyInfoCalled(DummyResponse response)
    {
        Func<Func<object[]>, bool> verifyParameters = f =>
        {
            var parameters = f();
            return parameters[0].ToString() == "DummyRequest" && parameters[1].ToString() == JsonSerializer.Serialize(response, JsonSerializerOptionsProvider.Sensitive);
        };

        _logger.Received(1).LogInformation("Handler for {requestType} completed with response {response}", Arg.Is<Func<object[]>>(f => verifyParameters(f)));
    }
}
