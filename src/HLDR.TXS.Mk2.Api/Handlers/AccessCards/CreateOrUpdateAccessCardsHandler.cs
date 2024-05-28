
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models.Requests.AccessCards;
using AccessControlSystem.Api.Models.Responses.AccessCards;
using AccessControlSystem.SharedKernel.ApiModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.Handlers.AccessCards;

public class CreateOrUpdateAccessCardsHandler(SqlDbContext sqlDbContext) : IRequestHandler<PutAccessCardsRequest, PutAccessCardsResponse>
{
    public async Task<PutAccessCardsResponse> Handle(PutAccessCardsRequest request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var uniqueValues = new HashSet<string>(request.AccessCardValues!);

        foreach (var value in uniqueValues)
        {
            var accessCard = await sqlDbContext.AccessCards.FirstOrDefaultAsync(t => t.Value == value, cancellationToken);

            if (accessCard is null)
            {
                accessCard = new AccessCard()
                {
                    Value = value
                };

                await sqlDbContext.AddAsync(accessCard, cancellationToken);
            }
            else
            {
                accessCard.Value = value;
            }
        }

        await sqlDbContext.SaveChangesAsync(cancellationToken);

        return new PutAccessCardsResponse() { StatusCode = StatusCodes.Status204NoContent };
    }
}
