
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.Api.HttpStatusCodes.Handlers;

public class NoContentResponseHandler : IStatusCodeHandler
{
    public IActionResult HandleResponse(object message) => new NoContentResult();
}