
using AccessControlSystem.Api.Models.DataTransferObjects;
using AccessControlSystem.Api.Models.Requests.AccessControlSystems;
using AccessControlSystem.Api.Models.Responses.AccessControlSystems;
using AccessControlSystem.Api.Models.SwaggerExamples;
using AccessControlSystem.Api.SwaggerGen.Filters;
using AccessControlSystem.SharedKernel.ApiModels;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/v{api-version:apiVersion}/devices/")]
[ApiController]
public class AccessControlDevicesController(IMediator mediator) : BaseController(mediator)
{
    [HttpGet]
    [SwaggerOperation(Tags = new[] { "Access Control Devices" }, Summary = "Retrieves all access control devices in the system.", Description = "Retrieves a list of access control devices in the system.")]
    [Produces("application/json")]
    [SwaggerResponse(200, "OK: Access control devices retrieved.", typeof(DataResponseModel<DevicesList>))]
    [SwaggerResponse(404, "Not Found: Access control device not found.")]
    public async Task<IActionResult> GetDevices([FromQuery]GetDeviceRequest request, CancellationToken cancellationToken = default)
        => await Send<GetDeviceRequest, GetDeviceResponse>(request, cancellationToken);

    [HttpPut("{deviceId}")]
    [SwaggerOperation(Tags = new[] { "Access Control Devices" }, Summary = "Creates or updates an access control device in the system.", Description = "Creates a new device or updates an existing one based on the provided deviceId.")]
    [Produces("application/json")]
    [SwaggerResponse(204, "No Content: Device upserted.")]
    [SwaggerResponse(400, "Bad Request:  Invalid device request.")]
    public async Task<IActionResult> CreateOrModifyDevices([FromRoute][SwaggerParameter("The device identifier.")] string deviceId, [FromBody] PutDeviceRequest request, CancellationToken cancellationToken = default)
    {
        request.DeviceId = deviceId;
        return await Send<PutDeviceRequest, PutDeviceResponse>(request, cancellationToken);
    }

    [HttpDelete("{deviceId}")]
    [SwaggerOperation(Tags = new[] { "Access Control Devices" }, Summary = "Removes a specific access control device from the system.", Description = "Removes a specific access control device from the system identified by its device Id.")]
    [Produces("application/json")]
    [SwaggerResponse(204, "No Content: Access control device removed.")]
    [SwaggerResponse(404, "Not Found: Access control device not found.")]
    [SwaggerResponse(409, "Conflict: Access control device cannot be removed because it has associated Slots.", typeof(ServiceError))]
    [SwaggerResponseExample(409, typeof(DeviceConflictErrorResponseExample))]
    public async Task<IActionResult> RemoveDevices([FromRoute] RemoveDeviceRequest request, CancellationToken cancellationToken = default)
        => await Send<RemoveDeviceRequest, RemoveDeviceResponse>(request, cancellationToken);
}
