
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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.Handlers.AccessCards;

public class RemoveAccessCardsHandler(SqlDbContext sqlDbContext) : IRequestHandler<RemoveAccessCardsRequest, RemoveAccessCardsResponse>
{
    public async Task<RemoveAccessCardsResponse> Handle(RemoveAccessCardsRequest request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        // Filter out any null, empty, or whitespace-only AccessCard values
        var filteredAccessCardValues = request.AccessCardValues!.Where(accessCard => !string.IsNullOrWhiteSpace(accessCard)).ToList();

        var removedCards = new AccessCardValueList() { AccessCardValues = new List<string>() };
        var cardsWithAccesses = new AccessCardValueList() { AccessCardValues = new List<string>() };
        var missingAccessCards = new AccessCardValueList() { AccessCardValues = new List<string>() };

        foreach (var accessCard in filteredAccessCardValues)
        {
            var queriedAccessCard = await sqlDbContext.AccessCards.FirstOrDefaultAsync(t => t.Value == accessCard, cancellationToken);
            if (queriedAccessCard is null)
            {
                missingAccessCards.AccessCardValues.Add(accessCard);
            }
            else
            {
                var accesses = await sqlDbContext.Mappings.Where(m => m.AccessCardId == queriedAccessCard.AccessCardId).ToListAsync(cancellationToken);
                if (accesses.Any())
                {
                    cardsWithAccesses.AccessCardValues.Add(accessCard);
                }
                else
                {
                    sqlDbContext.AccessCards.Remove(queriedAccessCard);
                    await sqlDbContext.SaveChangesAsync(cancellationToken);

                    removedCards.AccessCardValues.Add(accessCard);
                }
            }
        }

        if (missingAccessCards.AccessCardValues.Any() && !cardsWithAccesses.AccessCardValues.Any() && !removedCards.AccessCardValues.Any())
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.AccessCardsNotFound), ValidationMessages.AccessCardsNotFound)
                .Failure<RemoveAccessCardsResponse>();
        }

        if (cardsWithAccesses.AccessCardValues.Any() && !removedCards.AccessCardValues.Any())
        {
            return new ServiceError(ErrorType.ConflictError, StatusCodes.Status409Conflict, nameof(ValidationMessages.AccessCardsHaveAccesses), ValidationMessages.AccessCardsHaveAccesses)
                .Failure<RemoveAccessCardsResponse>();
        }

        if (cardsWithAccesses.AccessCardValues.Any() && removedCards.AccessCardValues.Any())
        {
            var responseData = new RemovedAndWithAccessesAccessCards
            {
                RemovedAccessCardValues = removedCards,
                AccessCardValuesWithAccess = cardsWithAccesses
            };

            return new RemoveAccessCardsResponse() { StatusCode = StatusCodes.Status207MultiStatus, Data = responseData };
        }

        return new RemoveAccessCardsResponse() { StatusCode = StatusCodes.Status204NoContent };
    }
}
