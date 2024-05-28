using AccessControlSystem.Api.HttpStatusCodes.Handlers;
using Microsoft.AspNetCore.Http;

namespace AccessControlSystem.Api.UnitTests.HttpStatusCodes.Handlers;

public class BadRequestHandlerTests
{
    [Fact]
    public void BadRequestResponseHandler_Returns_ExpectedCode()
    {
        var expected = StatusCodes.Status400BadRequest;

        var sut = new BadRequestResponseHandler();

        sut.StatusCode.Should().Be(expected);
    }
}
