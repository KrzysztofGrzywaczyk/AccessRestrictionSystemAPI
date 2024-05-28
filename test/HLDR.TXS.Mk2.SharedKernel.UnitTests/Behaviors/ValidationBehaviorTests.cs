using AccessControlSystem.SharedKernel.ApiModels;
using AccessControlSystem.UnitTests;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace AccessControlSystem.SharedKernel.UnitTests.Behaviors;

public class ValidationBehaviorTests : UnitTestsBase<ValidationBehaviorTestsContext>
{
    [Theory]
    [AutoSubstituteData]
    public async Task WhenValidatorsListIsEmpty_ShouldContinueExecution(DummyRequest request)
    {
        // arrange
        Context.WithEmptyValidatorsList();
        var requestHandler = Context.RequestHandlerDelegate;

        // act
        await Context.ValidationBehavior.Handle(request, requestHandler, CancellationToken.None);

        // assert
        Context.VerifyPipelineProceededTimesOnce();
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenValidatorsListIsEmpty_ShouldReturnExpectedResponse(DummyRequest request)
    {
        // arrange
        Context.SetupRequestHandlerWithResponse();
        var requestHandler = Context.RequestHandlerDelegate;

        // act
        var result = await Context.ValidationBehavior.Handle(request, requestHandler, CancellationToken.None);

        // assert
        result.Should().BeOfType(typeof(DummyResponse));
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenAnyValidatorReturnFails_ShouldReturnValidationError(DummyRequest request)
    {
        // arrange
        request.Count = 1;

        var requestHandler = Context.RequestHandlerDelegate;

        // act
        var result = await Context.ValidationBehavior.Handle(request, requestHandler, CancellationToken.None);

        // assert
        result.IsSuccess.Should().BeFalse();
        var serviceError = result.Error;
        serviceError?.Type.Should().Be(ErrorType.ValidationError);
        serviceError?.Status.Should().Be(StatusCodes.Status400BadRequest);
        serviceError?.Title.Should().Be("Request model not valid");
        serviceError?.Detail.Should().BeNull();
        serviceError?.InvalidParameters?.Single().Name.Should().Be(nameof(DummyRequest.Count));
        serviceError?.InvalidParameters?.Single().Reason.Should().Be("'Count' must be equal to '0'.");
    }
}