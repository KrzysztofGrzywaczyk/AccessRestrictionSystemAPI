using AccessControlSystem.Api.HttpStatusCodes.Handlers;
using Microsoft.AspNetCore.Http;

namespace AccessControlSystem.Api.UnitTests.HttpStatusCodes.Handlers;

public class OkResponseHandlerTests
{
    [Fact]
    public void OkResponseHandler_Returns_ExpectedCode()
    {
        var expected = StatusCodes.Status200OK;

        var sut = new OkResponseHandler();

        sut.StatusCode.Should().Be(expected);
    }
}