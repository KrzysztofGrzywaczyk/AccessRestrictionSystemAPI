
using Microsoft.AspNetCore.Http;

namespace AccessControlSystem.Api.HttpStatusCodes.Handlers;

public class ConflictResponseHandler : StatusCodeHandlerBase
{
    public override int StatusCode => StatusCodes.Status409Conflict;
}