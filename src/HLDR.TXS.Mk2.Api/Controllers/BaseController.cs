
using AccessControlSystem.Api.HttpStatusCodes;
using AccessControlSystem.SharedKernel.ApiModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.Controllers;

[ExcludeFromCodeCoverage]
public abstract class BaseController(IMediator mediator) : Controller
{
    protected async Task<IActionResult> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        where TRequest : IRequest<TResponse>
        where TResponse : ServiceResultBase
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var serviceResult = await mediator.Send(request, cancellationToken);

        var responseHandler = StatusCodeHandlerFactory.GetResponseHandler(serviceResult);
        return serviceResult.IsSuccess ? responseHandler.HandleResponse(serviceResult) : responseHandler.HandleResponse(serviceResult.Error!);
    }
}