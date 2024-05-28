
using AccessControlSystem.Api.Models.Responses.Accesses;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace AccessControlSystem.Api.Models.Requests.Accesses;

public class CopyAccessesRequest : IRequest<CopyAccessesResponse>
{
    [SwaggerParameter(Description = "The device Id of the source device from which accesses will be copied.")]
    public string? SourceDeviceId { get; set; }

    [SwaggerParameter(Description = "The device Id of the target device to which accesses will be copied.")]
    public string? TargetDeviceId { get; set; }
}
