
using AccessControlSystem.SharedKernel.JsonSerialization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AccessControlSystem.Api.HttpStatusCodes.Handlers;

public abstract class StatusCodeHandlerBase : IStatusCodeHandler
{
    public abstract int StatusCode { get; }

    public IActionResult HandleResponse(object message)
    {
        var response = new ContentResult
        {
            Content = JsonSerializer.Serialize(message, JsonSerializerOptionsProvider.Default),
            ContentType = "application/json",
            StatusCode = StatusCode
        };

        return response;
    }
}