
using AccessControlSystem.Api.Models.DataTransferObjects;
using AccessControlSystem.Api.Models.Requests.Slots;
using AccessControlSystem.Api.Models.Responses.Slots;
using AccessControlSystem.Api.Models.SwaggerExamples;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/v{api-version:apiVersion}/devices/{deviceId}/slots")]
[ApiController]
public class SlotsController(IMediator mediator) : BaseController(mediator)
{
    [HttpGet]
    [SwaggerOperation(Tags = new[] { "Access Control Devices / Slots" }, Summary = "Retrieves Slots existing on specified access control device.", Description = "Retrieves a list of slots on a device identified by its device Id and an optionaly scoped by list of slots expressed either by sotNames OR slotNumbers.")]
    [Produces("application/json")]
    [SwaggerResponse(200, "OK: Device with slots details retrieved.", Type = typeof(DataResponseModel<SlotAccesses>))]
    [SwaggerResponse(400, "Bad Request: A list of slots expressed as slotName AND slotNumber numbers; only one OR the other is allowed.")]
    [SwaggerResponse(404, "Not Found: \n - Device not found. \n - Slots not found.")]
    public async Task<IActionResult> GetSlotsInDevice([FromQuery] GetSlotsInDeviceRequest request, CancellationToken cancellationToken = default)
        => await Send<GetSlotsInDeviceRequest, GetSlotsInDeviceResponse>(request, cancellationToken);

    [HttpPut]
    [SwaggerOperation(Tags = new[] { "Access Control Devices / Slots" }, Summary = "Creates or updates slots on a device.", Description = "Creates or updates slots on a device identified by its device Id.")]
    [Produces("application/json")]
    [SwaggerResponse(204, "No Content: Slots upserted.")]
    [SwaggerResponse(400, "Bad Request: Invalid slots request.")]
    [SwaggerResponse(404, "Not Found: Device not found.")]
    public async Task<IActionResult> CreateOrUpdateSlotsInDevice([FromRoute][SwaggerParameter("The device identifier.")] string deviceId, [FromBody] PutSlotsInDeviceRequest request, CancellationToken cancellationToken = default)
    {
        request.DeviceId = deviceId;
        return await Send<PutSlotsInDeviceRequest, PutSlotsInDeviceResponse>(request, cancellationToken);
    }

    [HttpDelete]
    [SwaggerOperation(Tags = new[] { "Access Control Devices / Slots" }, Summary = "Removes specific slots on a device", Description = "Removes slots on a device identified by its device Id and an optional list of slots expressed either by slot names OR slot numbers. All Slots on device removed when no slots specified.")]
    [Produces("application/json")]
    [SwaggerResponse(204, "No Content: Slots removed.")]
    [SwaggerResponse(207, "Multi-Status: Some slots could not be removed because they have associated accesses.", typeof(DataResponseModel<RemovedAndWithAccessesSlots>))]
    [SwaggerResponse(400, "Bad Request: A list of slots expressed as slotNames AND slotNumbers; only one or the other is allowed.")]
    [SwaggerResponse(404, "Not Found: \n - Device not found. \n - Slots not found.")]
    [SwaggerResponse(409, "Conflict: Slots cannot be removed because they have associated accesses.")]
    public async Task<IActionResult> DeleteSlotFromDevice([FromQuery] RemoveSlotsFromDeviceRequest request, CancellationToken cancellationToken = default)
        => await Send<RemoveSlotsFromDeviceRequest, RemoveSlotsFromDeviceResponse>(request, cancellationToken);
}
