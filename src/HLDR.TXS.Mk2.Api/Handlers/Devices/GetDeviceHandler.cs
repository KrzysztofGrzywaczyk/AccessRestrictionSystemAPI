
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models.DataTransferObjects;
using AccessControlSystem.Api.Models.Requests.AccessControlSystems;
using AccessControlSystem.Api.Models.Responses.AccessControlSystems;
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

namespace HLDR.TXS.Mk2.Api.Handlers.AccessControlSystems;

public class GetDeviceHandler(SqlDbContext sqlDbContext) : IRequestHandler<GetDeviceRequest, GetDeviceResponse>
{
    public async Task<GetDeviceResponse> Handle(GetDeviceRequest request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var device = request.DeviceIds == null ?
            await sqlDbContext.Devices.ToListAsync(cancellationToken) :
            await sqlDbContext.Devices.Where(d => request.DeviceIds.Contains(d.DeviceName)).ToListAsync(cancellationToken);

        if (device.Count == 0)
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.DeviceNotFound), ValidationMessages.DeviceNotFound)
                .Failure<GetDeviceResponse>();
        }

        var simplifiedDevices = new List<GetDeviceDto>();
        foreach (var dev in device)
        {
            simplifiedDevices.Add(new GetDeviceDto()
            {
                DeviceId = dev.DeviceId,
                DeviceName = dev.DeviceName,
                Description = dev.Description
            });
        }

        var responseDevice = new DevicesList()
        {
            AccessControlDevices = simplifiedDevices
        };

        return new GetDeviceResponse()
        {
            StatusCode = StatusCodes.Status200OK,
            Data = responseDevice
        };
    }
}
