using AccessControlSystem.Api.HttpStatusCodes.Handlers;
using Microsoft.AspNetCore.Http;

namespace AccessControlSystem.Api.UnitTests.HttpStatusCodes.Handlers;

public class CreatedResponseHandlerTests
{
    [Fact]
    public void CreateResponseHandler_Returns_ExpectedCode()
    {
        var expected = StatusCodes.Status201Created;

        var sut = new CreatedResponseHandler();

        sut.StatusCode.Should().Be(expected);
    }
}