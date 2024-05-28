
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models;
using AccessControlSystem.Api.Models.DataTransferObjects;
using AccessControlSystem.Api.Models.Requests.Slots;
using AccessControlSystem.Api.Models.Responses.Slots;
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

namespace AccessControlSystem.Api.Handlers.Slots;

public class RemoveSlotsHandler(SqlDbContext sqlDbContext) : IRequestHandler<RemoveSlotsFromDeviceRequest, RemoveSlotsFromDeviceResponse>
{
    public async Task<RemoveSlotsFromDeviceResponse> Handle(RemoveSlotsFromDeviceRequest request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var device = await sqlDbContext.Devices.FirstOrDefaultAsync(d => d.DeviceName == request.DeviceId, cancellationToken);

        if (device is null)
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.DeviceNotFound), ValidationMessages.DeviceNotFound)
                .Failure<RemoveSlotsFromDeviceResponse>();
        }

        var missingSlots = new List<Slot>();
        var slots = new List<Slot>();
        if (request.SlotNames is null && request.SlotNumbers is null)
        {
            slots = await sqlDbContext.Slots.Include(s => s.AccessCards).Where(s => s.DeviceId == device.DeviceId).ToListAsync(cancellationToken);
        }
        else if (request.SlotNames is not null)
        {
            slots = await sqlDbContext.Slots.Include(s => s.AccessCards).Where(s => request.SlotNames.Contains(s.SlotName) && s.DeviceId == device.DeviceId).ToListAsync(cancellationToken);
            missingSlots = slots.Where(s => !request.SlotNames.Contains(s.SlotName)).ToList();
        }
        else if (request.SlotNumbers is not null)
        {
            slots = await sqlDbContext.Slots.Include(s => s.AccessCards).Where(s => request.SlotNumbers.Contains((SlotNumbersEnum)s.SlotNumber) && s.DeviceId == device.DeviceId).ToListAsync(cancellationToken);
            missingSlots = slots.Where(s => !request.SlotNumbers.Contains((SlotNumbersEnum)s.SlotNumber)).ToList();
        }

        var slotsToRemove = new List<Slot>();
        var removedSlots = new List<SimplifiedSlot>();
        var slotsWithAccesses = new List<SimplifiedSlot>();
        foreach (var slot in slots)
        {
            if (slot.AccessCards?.Count > 0)
            {
                slotsWithAccesses.Add(new SimplifiedSlot()
                {
                    SlotName = slot.SlotName,
                    SlotNumber = slot.SlotNumber
                });
            }
            else
            {
                slotsToRemove.Add(slot);
                removedSlots.Add(new SimplifiedSlot()
                {
                    SlotName = slot.SlotName,
                    SlotNumber = slot.SlotNumber
                });
            }
        }

        if (!slotsWithAccesses.Any() && !slotsToRemove.Any())
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.SlotsNotFound), ValidationMessages.SlotsNotFound)
                .Failure<RemoveSlotsFromDeviceResponse>();
        }

        sqlDbContext.Slots.RemoveRange(slotsToRemove);
        await sqlDbContext.SaveChangesAsync(cancellationToken);

        if (slotsWithAccesses.Any() && !removedSlots.Any())
        {
            return new ServiceError(ErrorType.ConflictError, StatusCodes.Status409Conflict, nameof(ValidationMessages.SlotsHaveAccesses), ValidationMessages.SlotsHaveAccesses)
                .Failure<RemoveSlotsFromDeviceResponse>();
        }

        if (slotsWithAccesses.Any() && removedSlots.Any())
        {
            var removedAndWithAccessesResponse = new RemovedAndWithAccessesSlots()
            {
                SlotsWithAccesses = slotsWithAccesses,
                RemovedSlots = removedSlots
            };

            return new RemoveSlotsFromDeviceResponse()
            {
                StatusCode = StatusCodes.Status207MultiStatus,
                Data = removedAndWithAccessesResponse
            };
        }

        return new RemoveSlotsFromDeviceResponse() { StatusCode = StatusCodes.Status204NoContent };
    }
}
