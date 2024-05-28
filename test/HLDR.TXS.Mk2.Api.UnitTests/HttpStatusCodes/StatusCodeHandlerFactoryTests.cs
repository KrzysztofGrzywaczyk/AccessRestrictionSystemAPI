using AccessControlSystem.Api.HttpStatusCodes;
using AccessControlSystem.Api.HttpStatusCodes.Handlers;
using AccessControlSystem.SharedKernel.ApiModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AccessControlSystem.Api.UnitTests.HttpStatusCodes;

public class StatusCodeHandlerFactoryTests
{
    [Theory]
    [InlineData(StatusCodes.Status200OK, typeof(OkResponseHandler))]
    [InlineData(StatusCodes.Status201Created, typeof(CreatedResponseHandler))]
    [InlineData(StatusCodes.Status202Accepted, typeof(AcceptedResponseHandler))]
    [InlineData(StatusCodes.Status204NoContent, typeof(NoContentResponseHandler))]
    [InlineData(StatusCodes.Status400BadRequest, typeof(BadRequestResponseHandler))]
    [InlineData(StatusCodes.Status409Conflict, typeof(ConflictResponseHandler))]
    [InlineData(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseHandler))]
    public void GetResponseHandler_WhenInErrorAndStatusCodeFoundAndIsBadRequest_ReturnsBadRequestError(int statusCode, Type handlerType)
    {
        var serviceResult = new ServiceError("errorType", status: statusCode).Failure<TestResult>();
        var result = StatusCodeHandlerFactory.GetResponseHandler(serviceResult);

        result.Should().BeOfType(handlerType);
    }

    [Fact]
    public void GetResponseHandler_WhenNoHandlerForStatusCodeFound_ThrowsInvalidOperationException()
    {
        var serviceResult = new ServiceError("errorType", status: StatusCodes.Status418ImATeapot).Failure<TestResult>();
        var result = StatusCodeHandlerFactory.GetResponseHandler(serviceResult);

        result.Should().BeOfType<DefaultResponseHandler>()
            .Which.HandleResponse(serviceResult).Should().BeOfType<ContentResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status418ImATeapot);
    }
}
