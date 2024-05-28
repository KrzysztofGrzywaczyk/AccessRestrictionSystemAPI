
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models.Requests.AccessControlSystems;
using AccessControlSystem.Api.Models.Responses.AccessControlSystems;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.Handlers.AccessControlSystems;

public class CreateOrModifyDeviceHandler(SqlDbContext sqlDbContext) : IRequestHandler<PutDeviceRequest, PutDeviceResponse>
{
    public async Task<PutDeviceResponse> Handle(PutDeviceRequest request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var device = await sqlDbContext.Devices.FirstOrDefaultAsync(d => d.DeviceName == request.DeviceId, cancellationToken);

        if (device == null)
        {
            device = new AccessControlDevice()
            {
                DeviceName = request.DeviceId,
                Description = request.DeviceName
            };

            await sqlDbContext.AddAsync(device, cancellationToken);
            await sqlDbContext.SaveChangesAsync(cancellationToken);

            return new PutDeviceResponse() { StatusCode = StatusCodes.Status204NoContent };
        }

        device.Description = request.DeviceName;
        await sqlDbContext.SaveChangesAsync(cancellationToken);

        return new PutDeviceResponse() { StatusCode = StatusCodes.Status204NoContent };
    }
}