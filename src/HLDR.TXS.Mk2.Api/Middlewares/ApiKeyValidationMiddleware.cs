
using AccessControlSystem.Api.Configurations;
using AccessControlSystem.SharedKernel.ApiModels;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.Middlewares;

public class ApiKeyValidationMiddleware(RequestDelegate next, AuthorizationConfiguration authorizationConfiguration)
{
    private const string ApiKeyQuery = "api-key";

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Query.TryGetValue(ApiKeyQuery, out var extractedApiKey))
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            if (context.Response.Body.CanWrite)
            {
                await context.Response.WriteAsJsonAsync(new ServiceError(ErrorType.AuthorizationError, StatusCodes.Status401Unauthorized, "ApiKey was not provided."));
            }

            return;
        }

        if (!authorizationConfiguration.ApiKey.Equals(extractedApiKey))
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            if (context.Response.Body.CanWrite)
            {
                await context.Response.WriteAsJsonAsync(new ServiceError(ErrorType.AuthorizationError, StatusCodes.Status401Unauthorized, "Unauthorized client"));
            }

            return;
        }

        await next(context);
    }
}