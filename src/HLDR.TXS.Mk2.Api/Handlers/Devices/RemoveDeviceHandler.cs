
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models.Requests.AccessControlSystems;
using AccessControlSystem.Api.Models.Responses.AccessControlSystems;
using AccessControlSystem.Api.Validation.Helpers;
using AccessControlSystem.SharedKernel.ApiModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.Handlers.AccessControlSystems;

public class RemoveDeviceHandler(SqlDbContext sqlDbContext) : IRequestHandler<RemoveDeviceRequest, RemoveDeviceResponse>
{
    public async Task<RemoveDeviceResponse> Handle(RemoveDeviceRequest request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var device = await sqlDbContext.Devices.Include(d => d.Slots).FirstOrDefaultAsync(d => d.DeviceName == request.DeviceId, cancellationToken);

        if (device is null)
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.DeviceNotFound), ValidationMessages.DeviceNotFound)
                .Failure<RemoveDeviceResponse>();
        }

        if (device!.Slots?.Count > 0)
        {
            var invalidParameters = device.Slots.Select(slot =>
                new InvalidParameter("existingSlotName", slot.SlotName!))
                .ToList();

            return new ServiceError(ErrorType.ConflictError, StatusCodes.Status409Conflict, nameof(ValidationMessages.DeviceHasSlots), ValidationMessages.DeviceHasSlots, null, invalidParameters)
                .Failure<RemoveDeviceResponse>();
        }

        sqlDbContext.Devices.Remove(device);
        await sqlDbContext.SaveChangesAsync(cancellationToken);

        return new RemoveDeviceResponse() { StatusCode = StatusCodes.Status204NoContent };
    }
}
