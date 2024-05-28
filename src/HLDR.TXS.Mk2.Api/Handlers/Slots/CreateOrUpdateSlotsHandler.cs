
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models.Requests.Slots;
using AccessControlSystem.Api.Models.Responses.Slots;
using AccessControlSystem.Api.Validation.Helpers;
using AccessControlSystem.SharedKernel.ApiModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.Handlers.Slots;

public class CreateOrUpdateSlotsHandler(SqlDbContext sqlDbContext) : IRequestHandler<PutSlotsInDeviceRequest, PutSlotsInDeviceResponse>
{
    public async Task<PutSlotsInDeviceResponse> Handle(PutSlotsInDeviceRequest request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var device = await sqlDbContext.Devices.FirstOrDefaultAsync(d => d.DeviceName == request.DeviceId, cancellationToken);

        if (device is null)
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.DeviceNotFound), ValidationMessages.DeviceNotFound)
                .Failure<PutSlotsInDeviceResponse>();
        }

        var deviceId = device.DeviceId;

        foreach (var slot in request.Slots!)
        {
            var slotEntity = await sqlDbContext.Slots.FirstOrDefaultAsync(s => s.DeviceId == deviceId && (s.SlotName == slot.SlotName || s.SlotNumber == slot.SlotNumber), cancellationToken);

            if (slotEntity is null)
            {
                slotEntity = new Slot
                {
                    DeviceId = deviceId,
                    SlotName = slot.SlotName,
                    SlotNumber = (int)slot.SlotNumber!
                };

                await sqlDbContext.Slots.AddAsync(slotEntity, cancellationToken);
            }
            else
            {
                slotEntity.DeviceId = deviceId;
                slotEntity.SlotName = slot.SlotName;
                slotEntity.SlotNumber = (int)slot.SlotNumber!;
            }

            await sqlDbContext.SaveChangesAsync(cancellationToken);
        }

        return new PutSlotsInDeviceResponse()
        { StatusCode = StatusCodes.Status204NoContent };
    }
}
