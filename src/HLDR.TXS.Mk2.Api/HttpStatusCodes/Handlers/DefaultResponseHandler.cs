
using AccessControlSystem.SharedKernel.JsonSerialization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AccessControlSystem.Api.HttpStatusCodes.Handlers;

public class DefaultResponseHandler : IStatusCodeHandler
{
    private readonly int _statusCode;

    public DefaultResponseHandler(int statusCode)
    {
        _statusCode = statusCode;
    }

    public IActionResult HandleResponse(object message)
    {
        var response = new ContentResult
        {
            Content = JsonSerializer.Serialize(message, JsonSerializerOptionsProvider.Default),
            ContentType = "application/json",
            StatusCode = _statusCode
        };

        return response;
    }
}