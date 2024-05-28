
using Microsoft.AspNetCore.Http;

namespace AccessControlSystem.Api.HttpStatusCodes.Handlers;

public class OkResponseHandler : StatusCodeHandlerBase
{
    public override int StatusCode => StatusCodes.Status200OK;
}