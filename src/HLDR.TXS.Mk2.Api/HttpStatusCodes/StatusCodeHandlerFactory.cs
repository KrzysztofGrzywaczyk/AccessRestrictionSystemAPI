
using AccessControlSystem.Api.HttpStatusCodes.Handlers;
using AccessControlSystem.SharedKernel.ApiModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace AccessControlSystem.Api.HttpStatusCodes;

public static class StatusCodeHandlerFactory
{
    private static readonly Dictionary<int, IStatusCodeHandler> StatusCodeHandlers;

    static StatusCodeHandlerFactory()
    {
        StatusCodeHandlers = new Dictionary<int, IStatusCodeHandler>
        {
            [StatusCodes.Status200OK] = new OkResponseHandler(),
            [StatusCodes.Status201Created] = new CreatedResponseHandler(),
            [StatusCodes.Status202Accepted] = new AcceptedResponseHandler(),
            [StatusCodes.Status204NoContent] = new NoContentResponseHandler(),
            [StatusCodes.Status400BadRequest] = new BadRequestResponseHandler(),
            [StatusCodes.Status409Conflict] = new ConflictResponseHandler(),
            [StatusCodes.Status500InternalServerError] = new InternalServerErrorResponseHandler()
        };
    }

    public static IStatusCodeHandler GetResponseHandler<TResult>(TResult result)
        where TResult : ServiceResultBase
    {
        if (!StatusCodeHandlers.TryGetValue(result.StatusCode, out var handler))
        {
            return new DefaultResponseHandler(result.StatusCode);
        }

        return handler;
    }
}