
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models.DataTransferObjects;
using AccessControlSystem.Api.Models.Requests.AccessCards;
using AccessControlSystem.Api.Models.Responses.AccessCards;
using AccessControlSystem.Api.Validation.Helpers;
using AccessControlSystem.SharedKernel.ApiModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.Handlers.AccessCards;

public class GetAccessCardsHandler(SqlDbContext sqlDbContext) : IRequestHandler<GetAccessCardsRequest, GetAccessCardsResponse>
{
    public async Task<GetAccessCardsResponse> Handle(GetAccessCardsRequest request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var accessCards = request.AccessCardValues == null ?
            await sqlDbContext.AccessCards.ToListAsync() :
            await sqlDbContext.AccessCards.Where(ac => request.AccessCardValues.Contains(ac.Value)).ToListAsync();

        if (!accessCards.Any())
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.AccessCardsNotFound), ValidationMessages.AccessCardsNotFound)
                .Failure<GetAccessCardsResponse>();
        }

        return new GetAccessCardsResponse()
        {
            StatusCode = StatusCodes.Status200OK,
            Data = new AccessCardValueList() { AccessCardValues = accessCards.Where(accessCard => accessCard.Value != null).Select(accessCard => accessCard.Value!).ToList() }
        };
    }
}
