
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models;
using AccessControlSystem.Api.Models.Requests.Accesses;
using AccessControlSystem.Api.Models.Responses.Accesses;
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

namespace AccessControlSystem.Api.Handlers.Accesses;

public class RemoveAccessesHandler(SqlDbContext sqlDbContext) : IRequestHandler<RemoveAccessesRequest, RemoveAccessesResponse>
{
    public async Task<RemoveAccessesResponse> Handle(RemoveAccessesRequest request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var device = await sqlDbContext.Devices.AsNoTracking().FirstOrDefaultAsync(d => d.DeviceName == request.DeviceId, cancellationToken);

        if (device is null)
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.DeviceNotFound), ValidationMessages.DeviceNotFound)
                .Failure<RemoveAccessesResponse>();
        }

        var slots = new List<Slot>();
        if (request.SlotNames is null && request.SlotNumbers is null)
        {
            slots = await sqlDbContext.Slots.AsNoTracking().Where(s => s.DeviceId == device.DeviceId).Include(s => s.AccessCards).ToListAsync(cancellationToken);
        }
        else if (request.SlotNames is not null)
        {
            slots = await sqlDbContext.Slots.AsNoTracking().Where(s => request.SlotNames.Contains(s.SlotName) && s.DeviceId == device.DeviceId).Include(s => s.AccessCards).ToListAsync(cancellationToken);
        }
        else if (request.SlotNumbers is not null)
        {
            slots = await sqlDbContext.Slots.AsNoTracking().Where(s => request.SlotNumbers.Contains((SlotNumbersEnum)s.SlotNumber) && s.DeviceId == device.DeviceId).Include(s => s.AccessCards).ToListAsync(cancellationToken);
        }

        var presentMappings = new List<AccessMapping>();
        foreach (var slot in slots)
        {
            var mappings = await sqlDbContext.Mappings.AsNoTracking().Where(m => m.SlotId == slot.SlotId).ToListAsync(cancellationToken);
            presentMappings.AddRange(mappings);
        }

        var mappingsToDelete = new List<AccessMapping>();

        if (request.AccessCardValues is not null)
        {
            var accessCards = new List<AccessCard>();
            foreach (var accessCardValue in request.AccessCardValues!)
            {
                var allAccessCards = await sqlDbContext.AccessCards.AsNoTracking().Where(t => t.Value == accessCardValue).ToListAsync(cancellationToken);
                accessCards.AddRange(allAccessCards);
            }

            foreach (var slot in slots)
            {
                foreach (var accessCard in accessCards)
                {
                    var newMapping = new AccessMapping()
                    {
                        SlotId = slot.SlotId,
                        AccessCardId = accessCard.AccessCardId
                    };

                    if (presentMappings.Contains(newMapping))
                    {
                        mappingsToDelete.Add(newMapping);
                    }
                    else
                    {
                        return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.AccessesNotFound), ValidationMessages.AccessesNotFound)
                            .Failure<RemoveAccessesResponse>();
                    }
                }
            }
        }
        else
        {
            foreach (var slot in slots)
            {
                mappingsToDelete.AddRange(presentMappings.Where(m => m.SlotId == slot.SlotId).ToList());
            }
        }

        if (!mappingsToDelete.Any())
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.AccessesNotFound), ValidationMessages.AccessesNotFound)
                .Failure<RemoveAccessesResponse>();
        }

        foreach (var mapping in mappingsToDelete)
        {
            sqlDbContext.Mappings.Remove(mapping);
        }

        await sqlDbContext.SaveChangesAsync(cancellationToken);

        return new RemoveAccessesResponse()
        {
            StatusCode = StatusCodes.Status204NoContent,
        };
    }
}
