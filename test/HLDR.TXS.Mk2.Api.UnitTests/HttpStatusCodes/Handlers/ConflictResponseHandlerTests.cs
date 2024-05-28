using AccessControlSystem.Api.HttpStatusCodes.Handlers;
using Microsoft.AspNetCore.Http;

namespace AccessControlSystem.Api.UnitTests.HttpStatusCodes.Handlers;

public class ConflictResponseHandlerTests
{
    [Fact]
    public void ConflictResponseHandler_Returns_ExpectedCode()
    {
        var expected = StatusCodes.Status409Conflict;

        var sut = new ConflictResponseHandler();

        sut.StatusCode.Should().Be(expected);
    }
}