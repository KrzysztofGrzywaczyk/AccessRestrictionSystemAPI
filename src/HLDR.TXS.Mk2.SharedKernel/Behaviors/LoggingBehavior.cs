
using AccessControlSystem.SharedKernel.ApiModels;
using AccessControlSystem.SharedKernel.JsonSerialization;
using AccessControlSystem.SharedKernel.Logging;
using MediatR;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.SharedKernel.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ServiceResultBase
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogDebug(
            "Handler for {requestType} triggered with request {request}",
            () => new object[]
            {
                request.GetType().Name,
                JsonSerializer.Serialize(request, JsonSerializerOptionsProvider.Sensitive)
            });

        var response = await next();

        logger.LogInformation(
            "Handler for {requestType} completed with response {response}",
            () => new object[]
            {
                request.GetType().Name,
                JsonSerializer.Serialize(response, JsonSerializerOptionsProvider.Sensitive)
            });

        return response;
    }
}