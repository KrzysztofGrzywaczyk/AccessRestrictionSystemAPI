
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

public class GetSlotsHandler(SqlDbContext sqlDbContext) : IRequestHandler<GetSlotsInDeviceRequest, GetSlotsInDeviceResponse>
{
    public async Task<GetSlotsInDeviceResponse> Handle(GetSlotsInDeviceRequest request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var device = await sqlDbContext.Devices.FirstOrDefaultAsync(d => d.DeviceName == request.DeviceId, cancellationToken);

        if (device is null)
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.DeviceNotFound), ValidationMessages.DeviceNotFound)
                .Failure<GetSlotsInDeviceResponse>();
        }

        if (request.SlotNames is null && request.SlotNumbers is null)
        {
            device = await sqlDbContext.Devices.Include(d => d.Slots)
                .FirstOrDefaultAsync(d => d.DeviceName == request.DeviceId, cancellationToken);
        }
        else if (request.SlotNames is not null)
        {
            device = await sqlDbContext.Devices.Include(d => d.Slots!.Where(s => request.SlotNames.Contains(s.SlotName)))
                .FirstOrDefaultAsync(d => d.DeviceName == request.DeviceId, cancellationToken);
        }
        else if (request.SlotNumbers is not null)
        {
            device = await sqlDbContext.Devices.Include(d => d.Slots!.Where(s => request.SlotNumbers.Contains((SlotNumbersEnum)s.SlotNumber)))
                .FirstOrDefaultAsync(d => d.DeviceName == request.DeviceId, cancellationToken);
        }

        if (device!.Slots is null || !device.Slots.Any())
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.SlotsNotFound), ValidationMessages.SlotsNotFound)
                .Failure<GetSlotsInDeviceResponse>();
        }

        var simplifiedDevice = new SlotAccesses()
        {
            DeviceId = device!.DeviceId,
            DeviceName = device.DeviceName,
            Description = device.Description,
            Slots = device?.Slots?.Select(slot => new GetSlotsDto
            {
                SlotId = slot.SlotId,
                SlotName = slot.SlotName,
                SlotNumber = slot.SlotNumber
            }).ToList() ?? new List<GetSlotsDto>()
        };

        return new GetSlotsInDeviceResponse()
        {
            StatusCode = StatusCodes.Status200OK,
            Data = simplifiedDevice
        };
    }
}
