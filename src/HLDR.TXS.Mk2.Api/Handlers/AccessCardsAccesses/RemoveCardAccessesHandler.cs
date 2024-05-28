
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models;
using AccessControlSystem.Api.Models.Requests.AccessCardAccesses;
using AccessControlSystem.Api.Models.Responses.AccessCardAccesses;
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

namespace AccessControlSystem.Api.Handlers.AccessCardAccesses;

public class RemoveCardAccessesHandler(SqlDbContext sqlDbContext) : IRequestHandler<RemoveCardAccessesRequest, RemoveCardAccessesResponse>
{
    public async Task<RemoveCardAccessesResponse> Handle(RemoveCardAccessesRequest request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var accessCard = await sqlDbContext.AccessCards.FirstOrDefaultAsync(t => t.Value == request.AccessCardValue, cancellationToken);

        if (accessCard is null)
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.AccessCardsNotFound), ValidationMessages.AccessCardsNotFound)
                .Failure<RemoveCardAccessesResponse>();
        }

        var accessesToDelete = new List<AccessMapping>();

        if (request.DeviceId is null)
        {
            accessesToDelete = await sqlDbContext.Mappings.Where(m => m.AccessCardId == accessCard.AccessCardId).ToListAsync(cancellationToken);
        }
        else
        {
            var device = await sqlDbContext.Devices.Include(d => d.Slots).FirstOrDefaultAsync(d => d.DeviceName == request.DeviceId);
            if (device is null)
            {
                return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.DeviceNotFound), ValidationMessages.DeviceNotFound)
                    .Failure<RemoveCardAccessesResponse>();
            }

            if (device.Slots is null)
            {
                {
                    return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.SlotsNotFound), ValidationMessages.SlotsNotFound)
                        .Failure<RemoveCardAccessesResponse>();
                }
            }

            var slots = new List<Slot>();
            if (request.SlotNames is not null)
            {
                slots = await sqlDbContext.Slots
                    .Where(s => s.DeviceId == device!.DeviceId && request.SlotNames.Contains(s.SlotName))
                    .ToListAsync(cancellationToken);
            }
            else if (request.SlotNumbers is not null)
            {
                slots = await sqlDbContext.Slots
                    .Where(s => s.DeviceId == device!.DeviceId && request.SlotNumbers.Contains((SlotNumbersEnum)s.SlotNumber))
                    .ToListAsync(cancellationToken);
            }
            else
            {
                slots = await sqlDbContext.Slots.Where(s => s.DeviceId == device!.DeviceId)
                    .ToListAsync(cancellationToken);
            }

            var slotsIds = slots.Select(s => s.SlotId).ToList();
            accessesToDelete = await sqlDbContext.Mappings.Where(m => m.AccessCardId == accessCard.AccessCardId && slotsIds.Contains(m.SlotId)).ToListAsync(cancellationToken);
        }

        if (!accessesToDelete.Any())
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.AccessesNotFound), ValidationMessages.AccessesNotFound)
                .Failure<RemoveCardAccessesResponse>();
        }

        sqlDbContext.RemoveRange(accessesToDelete);
        await sqlDbContext.SaveChangesAsync();

        return new RemoveCardAccessesResponse()
        {
            StatusCode = StatusCodes.Status204NoContent
        };
    }
}
