
using Microsoft.AspNetCore.Http;

namespace AccessControlSystem.Api.HttpStatusCodes.Handlers;

public class CreatedResponseHandler : StatusCodeHandlerBase
{
    public override int StatusCode => StatusCodes.Status201Created;
}