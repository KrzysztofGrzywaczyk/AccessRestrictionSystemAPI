
using Microsoft.AspNetCore.Http;

namespace AccessControlSystem.Api.HttpStatusCodes.Handlers;

public class BadRequestResponseHandler : StatusCodeHandlerBase
{
    public override int StatusCode => StatusCodes.Status400BadRequest;
}