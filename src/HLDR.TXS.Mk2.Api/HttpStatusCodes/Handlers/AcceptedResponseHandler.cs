using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.Api.HttpStatusCodes.Handlers;

public class AcceptedResponseHandler : IStatusCodeHandler
{
    public IActionResult HandleResponse(object message) => new AcceptedResult();
}