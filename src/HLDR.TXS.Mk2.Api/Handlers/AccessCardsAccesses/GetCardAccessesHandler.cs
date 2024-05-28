
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models;
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

public class GetCardAccessesHandler(SqlDbContext sqlDbContext) : IRequestHandler<GetCardAccessesRequest, GetCardAccessesResponse>
{
    public async Task<GetCardAccessesResponse> Handle(GetCardAccessesRequest request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var accessCard = await sqlDbContext.AccessCards.FirstOrDefaultAsync(t => t.Value == request.AccessCardValue, cancellationToken);

        if (accessCard is null)
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.AccessCardsNotFound), ValidationMessages.AccessCardsNotFound)
                .Failure<GetCardAccessesResponse>();
        }

        if (request.DeviceId is null)
        {
            if (await sqlDbContext.Devices.Include(d => d.Slots).ToListAsync() is not null)
            {
                if (await sqlDbContext.Devices.Include(d => d.Slots!).ThenInclude(s => s.AccessCards).ToListAsync() is null)
                {
                    return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.AccessesNotFound), ValidationMessages.AccessesNotFound)
                        .Failure<GetCardAccessesResponse>();
                }
            }
            else
            {
                return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.AccessesNotFound), ValidationMessages.AccessesNotFound)
                    .Failure<GetCardAccessesResponse>();
            }
        }

        var devices = request.DeviceId is not null ?
            await sqlDbContext.Devices.Include(d => d.Slots!).ThenInclude(s => s.AccessCards!.Where(ac => ac.Value == request.AccessCardValue)).
                Where(d => d.DeviceName == request.DeviceId)
                .ToListAsync(cancellationToken) :
            await sqlDbContext.Devices.Include(d => d.Slots!).ThenInclude(s => s.AccessCards!.Where(ac => ac.Value == request.AccessCardValue))
                .Where(d => d.Slots!.Any(s => s.AccessCards!.Any(t => t.Value == request.AccessCardValue)))
                .ToListAsync(cancellationToken);

        var accessCardAccesses = new Models.DataTransferObjects.AccessCardAccessesDto()
        {
            AccessCardId = accessCard.AccessCardId,
            AccessCardValue = accessCard.Value,
            SlotAccesses = new List<SlotAccesses>()
        };

        foreach (var device in devices)
        {
            var slots = device.Slots!.Where(s => s.AccessCards!.Any(ac => ac.Value == accessCard.Value)).Select(s =>
                        {
                            s.AccessCards = null;
                            return s;
                        }).ToList();

            if (request.SlotNames is not null)
            {
                slots = slots.Where(s => request.SlotNames.Contains(s.SlotName)).ToList();
            }
            else if (request.SlotNumbers is not null)
            {
                slots = slots.Where(s => request.SlotNumbers.Contains((SlotNumbersEnum)s.SlotNumber)).ToList();
            }

            var slotAccesses = new List<GetSlotsDto>();
            foreach (var slot in slots)
            {
                slotAccesses.Add(new GetSlotsDto()
                {
                    SlotId = slot.SlotId,
                    SlotName = slot.SlotName,
                    SlotNumber = slot.SlotNumber
                });
            }

            accessCardAccesses.SlotAccesses.Add(new SlotAccesses
            {
                DeviceId = device.DeviceId,
                DeviceName = device.DeviceName,
                Description = device.Description,
                Slots = slotAccesses
            });
        }

        if (accessCardAccesses.SlotAccesses.All(slotAccess => slotAccess.Slots != null && !slotAccess.Slots.Any()))
        {
            return new ServiceError(ErrorType.ResourceNotFound, StatusCodes.Status404NotFound, nameof(ValidationMessages.AccessesNotFound), ValidationMessages.AccessesNotFound)
                .Failure<GetCardAccessesResponse>();
        }

        return new GetCardAccessesResponse() { StatusCode = StatusCodes.Status200OK, Data = accessCardAccesses };
    }
}
