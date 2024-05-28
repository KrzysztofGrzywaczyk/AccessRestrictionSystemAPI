
using AccessControlSystem.Api.Models.Responses.AccessControlSystems;
using MediatR;
using System.Text.Json.Serialization;

namespace AccessControlSystem.Api.Models.Requests.AccessControlSystems;

public class PutDeviceRequest : IRequest<PutDeviceResponse>
{
    [JsonIgnore]
    public string? DeviceId { get; set; }

    public string? DeviceName { get; set; }
}
