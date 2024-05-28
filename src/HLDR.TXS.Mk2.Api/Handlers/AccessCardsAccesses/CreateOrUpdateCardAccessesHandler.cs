
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models.DataTransferObjects;
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

public class CreateOrUpdateCardAccessesHandler(SqlDbContext sqlDbContext) : IRequestHandler<PutCardAccessesRequest, PutCardAccessesResponse>
{
    public async Task<PutCardAccessesResponse> Handle(PutCardAccessesRequest request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var accessCard = await sqlDbContext.AccessCards.FirstOrDefaultAsync(t => t.Value == request.AccessCardValue);

        if (accessCard is null)
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.AccessCardsNotFound), ValidationMessages.AccessCardsNotFound)
                .Failure<PutCardAccessesResponse>();
        }

        var succeededSlots = new List<Slot>();
        var failedSlotAccesses = new List<ProperSlot>();
        var succeededSlotAccesses = new List<ProperSlot>();
        foreach (var slotAccess in request.SlotAccesses!)
        {
            var device = await sqlDbContext.Devices.FirstOrDefaultAsync(d => d.DeviceName == slotAccess.SlotId);
            if (device is null)
            {
                failedSlotAccesses.Add(slotAccess);
            }
            else
            {
                var slot = slotAccess.SlotName is null ?
                await sqlDbContext.Slots.FirstOrDefaultAsync(s => s.DeviceId == device!.DeviceId && s.SlotNumber == slotAccess.SlotNumber, cancellationToken) :
                await sqlDbContext.Slots.FirstOrDefaultAsync(s => s.DeviceId == device!.DeviceId && s.SlotName == slotAccess.SlotName, cancellationToken);

                if (slot is null)
                {
                    failedSlotAccesses.Add(slotAccess);
                }
                else
                {
                    succeededSlots.Add(slot);
                    succeededSlotAccesses.Add(slotAccess);
                }
            }
        }

        var preExistingAccesses = await sqlDbContext.Mappings.Where(m => m.AccessCardId == accessCard.AccessCardId).ToListAsync(cancellationToken);
        var newAccesses = new List<AccessMapping>();
        foreach (var slot in succeededSlots)
        {
            var newMapping = new AccessMapping()
            {
                SlotId = slot.SlotId,
                AccessCardId = accessCard.AccessCardId
            };

            if (!preExistingAccesses.Contains(newMapping))
            {
                newAccesses.Add(newMapping);
            }
        }

        await sqlDbContext.Mappings.AddRangeAsync(newAccesses, cancellationToken);
        await sqlDbContext.SaveChangesAsync(cancellationToken);

        var assignedAndFailedSlotAccesses = failedSlotAccesses.Any() ?
            new AssignedAndFailedCardAccesses
            {
                AssignedSlotAccesses = succeededSlotAccesses,
                FailedSlotAccesses = failedSlotAccesses
            }
            :
            new AssignedAndFailedCardAccesses
            {
                AssignedSlotAccesses = succeededSlotAccesses
            };

        if (!succeededSlotAccesses.Any())
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.AccessesNotAssigned), ValidationMessages.AccessesNotAssigned)
                .Failure<PutCardAccessesResponse>();
        }

        if (failedSlotAccesses.Any())
        {
            return new PutCardAccessesResponse()
            {
                StatusCode = StatusCodes.Status207MultiStatus,
                Data = assignedAndFailedSlotAccesses
            };
        }

        return new PutCardAccessesResponse()
        {
            StatusCode = StatusCodes.Status204NoContent,
        };
    }
}
