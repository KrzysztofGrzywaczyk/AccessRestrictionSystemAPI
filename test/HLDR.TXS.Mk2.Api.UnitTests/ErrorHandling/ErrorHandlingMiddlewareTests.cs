
using AccessControlSystem.UnitTests;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.ErrorHandling;

public class ErrorHandlingMiddlewareTests : UnitTestsBase<ErrorHandlingMiddlewareTestsContext>
{
    [Fact]
    public async Task WhenTaskCancelledException_ShouldIgnoreRequest()
    {
        // arrange
        Context.WithTaskCancelledExceptionThrown();

        // act
        await Context.Middleware.Invoke(new DefaultHttpContext());

        // assert
        Context.VerifyLoggerNotCalled();
    }

    [Fact]
    public async Task WhenOperationAttemptedOnNonExistentConnection_ShouldIgnoreRequest()
    {
        // arrange
        Context.WithOperationOnNonExistentConnectionAttempted();

        // act
        await Context.Middleware.Invoke(new DefaultHttpContext());

        // assert
        Context.VerifyLoggerNotCalled();
    }

    [Fact]
    public async Task WhenUnhandledException_ShouldLogError()
    {
        // arrange
        Context.WithInvalidOperationException();

        // act
        await Context.Middleware.Invoke(new DefaultHttpContext());

        // assert
        Context.VerifyExceptionLogged();
    }

    [Fact]
    public async Task WhenUnhandledException_ShouldReturnInternalServerError()
    {
        // arrange
        Context.WithInvalidOperationException();
        var httpContext = new DefaultHttpContext();

        // act
        await Context.Middleware.Invoke(httpContext);
        var responseStatusCode = httpContext.Response.StatusCode;

        // assert
        responseStatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}
