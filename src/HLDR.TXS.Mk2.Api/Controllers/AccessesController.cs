
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models.DataTransferObjects;
using AccessControlSystem.Api.Models.Requests.Accesses;
using AccessControlSystem.Api.Models.Responses.Accesses;
using AccessControlSystem.Api.Models.SwaggerExamples;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/v{api-version:apiVersion}/devices/")]
[ApiController]
public class AccessesController(IMediator mediator) : BaseController(mediator)
{
    [HttpGet("{deviceId}/accesses")]
    [SwaggerOperation(Tags = new[] { "Access Control Device Accesses" }, Summary = "Retrieves AccessCards with access to the device's Slots.", Description = "Retrieves the list of AccessCards that have been granted access to slots of a specific device in the system. The device is identified by its device Id and and is able to be optionally filtered by list of Slots expressed either by slot names or slot numbers.")]
    [Produces("application/json")]
    [SwaggerResponse(200, "OK: Accesses retrieved.", typeof(DataResponseModel<AccessControlDevice>))]
    [SwaggerResponse(400, "Bad Request: A list of slots expressed as slot names AND slot numbers; only one OR the other is allowed.")]
    [SwaggerResponse(404, "Not Found: \n - Access control device not found. \n - No accesses were found.")]
    public async Task<IActionResult> GetSlotAccesses([FromQuery] GetAccessesRequest request, CancellationToken cancellationToken = default)
        => await Send<GetAccessesRequest, GetAccessesResponse>(request, cancellationToken);

    [HttpPut("{deviceId}/accesses")]
    [SwaggerOperation(Tags = new[] { "Access Control Device Accesses" }, Summary = "Assigns access to AccessCards for the specified Slots if not exists.", Description = "Assigns access to AccessCards for a specific slots on given device. Allows to provide a list of AccessCard values that should be granted access to the device slots identified by device Id and a list of Slots expressed either by slot names OR slot numbers.")]
    [Produces("application/json")]
    [SwaggerResponse(204, "No Content: The access for AccessCards was assigned to the specified Slots.")]
    [SwaggerResponse(207, "Multi-Status: The access for AccessCards was assigned to the specified Slots but some non-existent Slots and/or non-existent AccessCards has been found.", typeof(DataResponseModel<NonExistentSlotsOrCardsResponse>))]
    [SwaggerResponse(400, "Bad Request: \n - Missing list of 'Slots': A list of slot names OR slot number numbers is required. \n - A list of Slots expressed as slot names AND slot numbers; only one or the other is allowed. \n - Invalid AccessCard access request.")]
    [SwaggerResponse(404, "Not Found: \n - Access control device not found. \n - No slots exists for the specified access control device matching provided slots criteria.")]
    public async Task<IActionResult> CreateOrUpdateSlotAccesses([FromQuery] PutAccessesRequest request, CancellationToken cancellationToken = default)
    {
        return await Send<PutAccessesRequest, PutAccessesResponse>(request, cancellationToken);
    }

    [HttpDelete("{deviceId}/accesses")]
    [SwaggerOperation(Tags = new[] { "Access Control Device Accesses" }, Summary = "Removes access of AccessCards to the specified Slots.", Description = "Removes access of AccessCards from the specific Slots within the system. Possible scenarios: 1. Removes access of all AccessCards to all Slots on a device (passed: deviceId). 2. Removes access of all AccessCards to specified Slots on a device (passed : deviceId + SlotNames OR SlotNumbers). 3. Removes accesses of specified AccessCards to all Slots on a device (passed: deviceId + AccessCards). 4. Removes access of specified AccessCards to specific Slots on a device (passed : deviceId + SlotNames OR SlotNumbers + AccessCards).")]
    [Produces("application/json")]
    [SwaggerResponse(204, "No Content: The access for AccessCards was removed from the specified Slots.")]
    [SwaggerResponse(400, "Bad Request: A list of slots expressed as slot names AND SlotNumber numbers; only one or the other is allowed.")]
    [SwaggerResponse(404, "Not Found: \n - Device not found. \n - Accesses not found.")]

    public async Task<IActionResult> RemoveSlotAccesses([FromQuery] RemoveAccessesRequest request, CancellationToken cancellationToken = default)
        => await Send<RemoveAccessesRequest, RemoveAccessesResponse>(request, cancellationToken);

    [HttpPut("{sourceDeviceId}/copyaccessesto/{targetDeviceId}")]
    [SwaggerOperation(Tags = new[] { "Access Control Device Accesses" }, Summary = "Copies all accesses from one device to another", Description = "Copies all accesses from the source device (not modifying them) identified by sourceDeviceId to the target device identified by targetDevice Id.")]
    [Produces("application/json")]
    [SwaggerResponse(204, "No Content: Accesses  successfully copied from source device to target device.")]
    [SwaggerResponse(404, "Not Found: Some resources were not found. \n - Source device not found. \n - No accesses on source device to copy. \n - Target device not found.")]
    public async Task<IActionResult> CopyAccessToTargetDevice([FromRoute] CopyAccessesRequest request, CancellationToken cancellationToken = default)
        => await Send<CopyAccessesRequest, CopyAccessesResponse>(request, cancellationToken);

    [HttpPut("{sourceDeviceId}/moveaccessesto/{targetDeviceId}")]
    [SwaggerOperation(Tags = new[] { "Access Control Device Accesses" }, Summary = "Moves all accesses from one device to another", Description = "Moves all accesses from the source device (deleting them from source device) identified by sourceDeviceId to the target device identified by targetDevice Id.")]
    [Produces("application/json")]
    [SwaggerResponse(204, "No Content: Accesses moved from source device to target device.")]
    [SwaggerResponse(404, "Not Found: Some resources not found. \n - Source device not found. \n - No accesses on source device to move. \n - Target device not found.")]
    public async Task<IActionResult> MoveAccessToTargetDevice([FromRoute] MoveAccessesRequest request, CancellationToken cancellationToken = default)
        => await Send<MoveAccessesRequest, MoveAccessesResponse>(request, cancellationToken);
}
