
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.Handlers.Accesses;

public class GetAccessesHandler(SqlDbContext sqlDbContext) : IRequestHandler<GetAccessesRequest, GetAccessesResponse>
{
    public async Task<GetAccessesResponse> Handle(GetAccessesRequest request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var device = await sqlDbContext.Devices.FirstOrDefaultAsync(d => d.DeviceName == request.DeviceId, cancellationToken);

        if (device is null)
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.DeviceNotFound), ValidationMessages.DeviceNotFound)
                .Failure<GetAccessesResponse>();
        }

        if (request.SlotNames is null && request.SlotNumbers is null)
        {
            device = await sqlDbContext.Devices.Include(d => d.Slots!).ThenInclude(s => s.AccessCards)
                .FirstOrDefaultAsync(d => d.DeviceName == request.DeviceId, cancellationToken);
        }
        else if (request.SlotNames is not null)
        {
            device = await sqlDbContext.Devices.Include(d => d.Slots!.Where(s => request.SlotNames.Contains(s.SlotName))).ThenInclude(s => s.AccessCards)
                .FirstOrDefaultAsync(d => d.DeviceName == request.DeviceId, cancellationToken);
        }
        else if (request.SlotNumbers is not null)
        {
            device = await sqlDbContext.Devices.Include(d => d.Slots!.Where(s => request.SlotNumbers.Contains((SlotNumbersEnum)s.SlotNumber))).ThenInclude(s => s.AccessCards)
                .FirstOrDefaultAsync(d => d.DeviceName == request.DeviceId, cancellationToken);
        }

        if (device!.Slots is null || !device.Slots.Any())
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.AccessesNotFound), ValidationMessages.AccessesNotFound)
                .Failure<GetAccessesResponse>();
        }

        return new GetAccessesResponse() { Data = device };
    }
}
