using AccessControlSystem.Api.HttpStatusCodes.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.Api.UnitTests.HttpStatusCodes.Handlers;

public class NoContentResponseHandlerTests
{
    [Fact]
    public void NoContentResponseHandler_Returns_ExpectedCode()
    {
        var expected = StatusCodes.Status204NoContent;

        var sut = new NoContentResponseHandler();
        var result = sut.HandleResponse(new object()) as NoContentResult;

        result?.StatusCode.Should().Be(expected);
    }
}