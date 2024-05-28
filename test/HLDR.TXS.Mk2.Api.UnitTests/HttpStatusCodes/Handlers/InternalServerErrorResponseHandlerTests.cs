using AccessControlSystem.Api.HttpStatusCodes.Handlers;
using Microsoft.AspNetCore.Http;

namespace AccessControlSystem.Api.UnitTests.HttpStatusCodes.Handlers;

public class InternalServerErrorResponseHandlerTests
{
    [Fact]
    public void InternalServerErrorResponseHandler_Returns_ExpectedCode()
    {
        var expected = StatusCodes.Status500InternalServerError;

        var sut = new InternalServerErrorResponseHandler();

        sut.StatusCode.Should().Be(expected);
    }
}