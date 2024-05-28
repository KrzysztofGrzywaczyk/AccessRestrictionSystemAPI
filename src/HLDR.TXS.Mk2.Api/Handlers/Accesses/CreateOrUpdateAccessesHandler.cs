
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models;
using AccessControlSystem.Api.Models.DataTransferObjects;
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

public class CreateOrUpdateAccessesHandler(SqlDbContext sqlDbContext) : IRequestHandler<PutAccessesRequest, PutAccessesResponse>
{
    public async Task<PutAccessesResponse> Handle(PutAccessesRequest request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var device = await sqlDbContext.Devices.FirstOrDefaultAsync(d => d.DeviceName == request.DeviceId, cancellationToken);

        if (device is null)
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.DeviceNotFound), ValidationMessages.DeviceNotFound)
                .Failure<PutAccessesResponse>();
        }

        var existingSlots = request.SlotNames is not null ?
            await sqlDbContext.Slots.Where(s => request.SlotNames.Contains(s.SlotName) && s.DeviceId == device.DeviceId).Include(s => s.AccessCards).ToListAsync(cancellationToken) :
            await sqlDbContext.Slots.Where(s => request.SlotNumbers!.Contains((SlotNumbersEnum)s.SlotNumber) && s.DeviceId == device.DeviceId).Include(s => s.AccessCards).ToListAsync(cancellationToken);

        if (existingSlots is null || !existingSlots.Any())
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.SlotsNotFound), ValidationMessages.SlotsNotFound)
                .Failure<PutAccessesResponse>();
        }

        var nonExistentSlots = new List<ProperSlot>();
        var nonExistentSlotNumbers = new List<ProperSlot>();

        if (request.SlotNames != null)
        {
            nonExistentSlots.AddRange(request.SlotNames
                .Where(slotName => !existingSlots.Any(s => s.SlotName == slotName))
                .Select(name => new ProperSlot { SlotId = request.DeviceId, SlotName = name }));
        }

        if (request.SlotNumbers != null)
        {
            nonExistentSlotNumbers.AddRange(request.SlotNumbers
                .Where(slotNumber => !existingSlots.Any(s => s.SlotNumber == (int)slotNumber))
                .Select(number => new ProperSlot { SlotId = request.DeviceId, SlotNumber = (int)number }));
        }

        List<ProperSlot> nonExistentSlotsOrSlotNumbers = nonExistentSlots.Concat(nonExistentSlotNumbers).ToList();

        // Requested AccessCards is validated and at least one AccessCard is in the list
        var requestedAccessCards = request.AccessCardValues!.AccessCardValues;

        var existingAccessCards = await sqlDbContext.AccessCards
            .Where(accessCard => requestedAccessCards!.Contains(accessCard.Value!))
            .Select(accessCard => accessCard.Value)
            .ToListAsync(cancellationToken);

        if (existingAccessCards is null || !existingAccessCards.Any())
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.AccessCardsNotFound), ValidationMessages.AccessCardsNotFound)
                .Failure<PutAccessesResponse>();
        }

        var nonExistentAccessCards = requestedAccessCards!.Except(existingAccessCards).ToList();

        var presentMappings = new List<AccessMapping>();
        foreach (var slot in existingSlots)
        {
            presentMappings.AddRange(await sqlDbContext.Mappings.Where(m => m.SlotId == slot.SlotId).ToListAsync(cancellationToken));
        }

        var accessCards = new List<AccessCard>();
        foreach (var accessCardValue in existingAccessCards)
        {
            accessCards.AddRange(await sqlDbContext.AccessCards.Where(t => t.Value == accessCardValue).ToListAsync(cancellationToken));
        }

        var newMappings = new List<AccessMapping>();
        foreach (var slot in existingSlots)
        {
            foreach (var accessCard in accessCards)
            {
                var newMapping = new AccessMapping()
                {
                    SlotId = slot.SlotId,
                    AccessCardId = accessCard.AccessCardId
                };

                if (!presentMappings.Contains(newMapping))
                {
                    newMappings.Add(newMapping);
                }
            }
        }

        await sqlDbContext.Mappings.AddRangeAsync(newMappings, cancellationToken);

        await sqlDbContext.SaveChangesAsync(cancellationToken);

        if ((nonExistentSlotsOrSlotNumbers is not null && nonExistentSlotsOrSlotNumbers.Any())
            || (nonExistentAccessCards is not null && nonExistentAccessCards.Any()))
        {
            var putAccessesWithNonExistentSlotsOrNonExistentAccessCardsResponse = new NonExistentSlotsOrCardsResponse()
            {
                NonExistentSlots = nonExistentSlotsOrSlotNumbers,
                NonExistentAccessCards = nonExistentAccessCards
            };

            return new PutAccessesResponse()
            {
                StatusCode = StatusCodes.Status207MultiStatus,
                Data = putAccessesWithNonExistentSlotsOrNonExistentAccessCardsResponse
            };
        }

        return new PutAccessesResponse() { StatusCode = StatusCodes.Status204NoContent };
    }
}
