
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.Api.HttpStatusCodes.Handlers;

public interface IStatusCodeHandler
{
    IActionResult HandleResponse(object message);
}