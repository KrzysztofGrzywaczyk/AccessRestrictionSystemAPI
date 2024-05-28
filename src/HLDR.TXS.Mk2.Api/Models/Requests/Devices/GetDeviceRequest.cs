
using AccessControlSystem.Api.Models.Bindings;
using AccessControlSystem.Api.Models.Responses.AccessControlSystems;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace AccessControlSystem.Api.Models.Requests.AccessControlSystems;

public class GetDeviceRequest : IRequest<GetDeviceResponse>
{
    [SwaggerParameter(Description = "Optional device identifiers. If provided, the response will be filtered to only include details for the specified devices. Provide as a comma-separated list for multiple values.")]
    [CommaSeparated]
    public string[]? DeviceIds { get; set; }
}
