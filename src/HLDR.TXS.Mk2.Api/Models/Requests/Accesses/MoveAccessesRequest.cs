
using AccessControlSystem.Api.Models.Responses.Accesses;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace AccessControlSystem.Api.Models.Requests.Accesses;

public class MoveAccessesRequest : IRequest<MoveAccessesResponse>
{
    [SwaggerParameter(Description = "The device Id of the source device from which accesses will be moved.")]
    public string? SourceDeviceId { get; set; }

    [SwaggerParameter(Description = "The device Id of the source device to which accesses will be moved.")]
    public string? TargetDeviceId { get; set; }
}
