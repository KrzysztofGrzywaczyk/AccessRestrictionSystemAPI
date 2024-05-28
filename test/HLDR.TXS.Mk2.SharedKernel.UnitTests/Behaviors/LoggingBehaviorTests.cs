using AccessControlSystem.UnitTests;

namespace AccessControlSystem.SharedKernel.UnitTests.Behaviors;

public class LoggingBehaviorTests : UnitTestsBase<LoggingBehaviorTestsContext>
{
    [Theory]
    [AutoSubstituteData]
    public async Task WhenExecuted_ShouldLogDebugRequest(DummyRequest request)
    {
        var requestHandler = Context.RequestHandlerDelegate;

        await Context.LoggingBehavior.Handle(request, requestHandler, CancellationToken.None);

        Context.VerifyDebugCalled(request);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenExecuted_ShouldLogInfoResponse(DummyRequest request)
    {
        var requestHandler = Context.RequestHandlerDelegate;

        var response = await Context.LoggingBehavior.Handle(request, requestHandler, CancellationToken.None);

        Context.VerifyInfoCalled(response);
    }
}