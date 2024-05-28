
using AccessControlSystem.Api.Entities;
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

public class CopyAccessesHandler(SqlDbContext sqlDbContext) : IRequestHandler<CopyAccessesRequest, CopyAccessesResponse>
{
    public async Task<CopyAccessesResponse> Handle(CopyAccessesRequest request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var sourceDevice = await sqlDbContext.Devices.Include(d => d.Slots!).ThenInclude(s => s.AccessCards).FirstOrDefaultAsync(d => d.DeviceName == request.SourceDeviceId, cancellationToken);

        if (sourceDevice is null)
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.DeviceNotFound), $"Source {ValidationMessages.DeviceNotFound.ToLower()}")
                .Failure<CopyAccessesResponse>();
        }

        if (sourceDevice!.Slots is null)
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.AccessesNotFoundOnSourceDevice), string.Format(ValidationMessages.AccessesNotFoundOnSourceDevice, "copy"))
                .Failure<CopyAccessesResponse>();
        }

        sourceDevice.Slots = sourceDevice.Slots!.Where(s => s.AccessCards != null && s.AccessCards.Any()).ToList();

        var targetDevice = await sqlDbContext.Devices.Include(d => d.Slots).FirstOrDefaultAsync(d => d.DeviceName == request.TargetDeviceId, cancellationToken);

        if (targetDevice is null)
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.DeviceNotFound), $"Target {ValidationMessages.DeviceNotFound.ToLower()}")
                .Failure<CopyAccessesResponse>();
        }

        var slotNumberToAccessCardId = sourceDevice!.Slots
            !.Where(slot => slot.AccessCards != null)
            .SelectMany(slot => slot.AccessCards!.Select(accessCard => (slot.SlotNumber, accessCard.AccessCardId)))
            .ToList();

        if (slotNumberToAccessCardId.Count == 0)
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.AccessesNotFoundOnSourceDevice), string.Format(ValidationMessages.AccessesNotFoundOnSourceDevice, "copy"))
                .Failure<CopyAccessesResponse>();
        }

        var existingMappings = targetDevice?.Slots
            ?.SelectMany(slot => sqlDbContext.Mappings
            .AsNoTracking()
            .Where(m => m.SlotId == slot.SlotId))
            .ToList();

        var targetMappings = new List<AccessMapping>();
        foreach (var slotNumberAccessCardMap in slotNumberToAccessCardId)
        {
            var slot = await sqlDbContext.Slots.FirstOrDefaultAsync(s => s.DeviceId == targetDevice!.DeviceId && s.SlotNumber == slotNumberAccessCardMap.SlotNumber, cancellationToken);
            if (slot is null)
            {
                slot = new Slot
                {
                    DeviceId = targetDevice!.DeviceId,
                    SlotNumber = slotNumberAccessCardMap.SlotNumber,
                    SlotName = string.Empty
                };
                await sqlDbContext.Slots.AddAsync(slot, cancellationToken);
                await sqlDbContext.SaveChangesAsync(cancellationToken);
            }

            var newMapping = new AccessMapping()
            {
                SlotId = slot.SlotId,
                AccessCardId = slotNumberAccessCardMap.AccessCardId
            };

            if (existingMappings is null || !existingMappings.Contains(newMapping))
            {
                targetMappings.Add(newMapping);
            }
        }

        await sqlDbContext.AddRangeAsync(targetMappings, cancellationToken);
        await sqlDbContext.SaveChangesAsync(cancellationToken);

        return new CopyAccessesResponse() { StatusCode = StatusCodes.Status204NoContent };
    }
}
