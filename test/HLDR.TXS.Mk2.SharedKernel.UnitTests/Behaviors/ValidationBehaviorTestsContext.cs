using AccessControlSystem.SharedKernel.Behaviors;
using AccessControlSystem.UnitTests;
using FluentValidation;
using MediatR;
using NSubstitute;

namespace AccessControlSystem.SharedKernel.UnitTests.Behaviors;

public class ValidationBehaviorTestsContext : UnitTestsContextBase
{
    public ValidationBehaviorTestsContext()
    {
        var validator = new DummyValidator();
        RequestHandlerDelegate = Substitute.For<RequestHandlerDelegate<DummyResponse>>();
        ValidationBehavior = new ValidationBehavior<DummyRequest, DummyResponse>(new[] { validator });
    }

    public ValidationBehavior<DummyRequest, DummyResponse> ValidationBehavior { get; private set; }

    public RequestHandlerDelegate<DummyResponse> RequestHandlerDelegate { get; }

    public void WithEmptyValidatorsList()
    {
        ValidationBehavior = new ValidationBehavior<DummyRequest, DummyResponse>(new IValidator<DummyRequest>[] { });
    }

    public void VerifyPipelineProceededTimesOnce()
    {
        RequestHandlerDelegate.Received(1);
    }

    public void SetupRequestHandlerWithResponse()
    {
        RequestHandlerDelegate().Returns(Task.FromResult(new DummyResponse()));
    }
}
