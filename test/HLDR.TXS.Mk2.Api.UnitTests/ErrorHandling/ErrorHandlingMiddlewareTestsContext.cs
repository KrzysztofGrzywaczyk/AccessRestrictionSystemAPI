
using AccessControlSystem.Api.Middlewares;
using AccessControlSystem.SharedKernel.Logging;
using AccessControlSystem.UnitTests;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.ErrorHandling;

public sealed class ErrorHandlingMiddlewareTestsContext : UnitTestsContextBase
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    private Func<Task> requestDelegateMock;

    public ErrorHandlingMiddlewareTestsContext()
    {
        Task RequestDelegate(HttpContext httpContext) => requestDelegateMock();
        _logger = Substitute.For<ILogger<ErrorHandlingMiddleware>>();
        requestDelegateMock = () => Task.CompletedTask;
        Middleware = new ErrorHandlingMiddleware(RequestDelegate, _logger);
    }

    public ErrorHandlingMiddleware Middleware { get; }

    public void WithTaskCancelledExceptionThrown()
    {
        requestDelegateMock = () => throw new TaskCanceledException();
    }

    public void WithOperationOnNonExistentConnectionAttempted()
    {
        requestDelegateMock = () => throw new HttpListenerException(0, "An operation was attempted on a nonexistent network connection");
    }

    public void WithInvalidOperationException()
    {
        requestDelegateMock = () => throw new InvalidOperationException();
    }

    public void VerifyLoggerNotCalled()
    {
        _logger.DidNotReceive().LogError(Arg.Any<Exception>());
    }

    public void VerifyExceptionLogged()
    {
        _logger.Received(1).LogError(Arg.Any<InvalidOperationException>());
    }
}