
using AccessControlSystem.Api.Models.DataTransferObjects;
using AccessControlSystem.Api.Models.Responses.Slots;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AccessControlSystem.Api.Models.Requests.Slots;

public class PutSlotsInDeviceRequest : IRequest<PutSlotsInDeviceResponse>
{
    [FromRoute]
    [JsonIgnore]
    public string? DeviceId { get; set; }

    public List<SimplifiedSlot>? Slots { get; set; }
}
