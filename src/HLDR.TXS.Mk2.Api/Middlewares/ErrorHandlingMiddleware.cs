
using AccessControlSystem.SharedKernel.ApiModels;
using AccessControlSystem.SharedKernel.Logging;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next.Invoke(context).ConfigureAwait(false);
        }
        catch (TaskCanceledException)
        {
            //// when caller aborts request runtime throws this exception
        }
        catch (HttpListenerException ex) when (ex.Message == "An operation was attempted on a nonexistent network connection")
        {
            //// when caller aborts request while any of the components are writing to the response stream
        }
        catch (Exception ex)
        {
            await HandleException(context, ex).ConfigureAwait(false);
        }
    }

    private async Task HandleException(HttpContext context, Exception ex)
    {
        logger.LogError(ex);

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        if (context.Response.Body.CanWrite)
        {
            await context.Response.WriteAsJsonAsync(new ServiceError(ErrorType.InternalServerError, status: StatusCodes.Status500InternalServerError));
        }
    }
}
