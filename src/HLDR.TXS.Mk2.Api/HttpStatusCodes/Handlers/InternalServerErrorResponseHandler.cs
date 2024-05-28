
using Microsoft.AspNetCore.Http;

namespace AccessControlSystem.Api.HttpStatusCodes.Handlers;

public class InternalServerErrorResponseHandler : StatusCodeHandlerBase
{
    public override int StatusCode => StatusCodes.Status500InternalServerError;
}