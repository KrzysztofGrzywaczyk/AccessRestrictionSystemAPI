
using AccessControlSystem.Api.Models.Responses.AccessControlSystems;
using MediatR;
using Microsoft.ApplicationInsights;
using Swashbuckle.AspNetCore.Annotations;

namespace AccessControlSystem.Api.Models.Requests.AccessControlSystems;

public class RemoveDeviceRequest : IRequest<RemoveDeviceResponse>
{
    [SwaggerParameter(Description = "The device identifier.")]
    public string? DeviceId { get; set; }
}
