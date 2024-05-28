
using AccessControlSystem.Api.Models.DataTransferObjects;
using AccessControlSystem.Api.Models.Responses.AccessCardAccesses;
using MediatR;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AccessControlSystem.Api.Models.Requests.AccessCardAccesses;

public class PutCardAccessesRequest : IRequest<PutCardAccessesResponse>
{
    [JsonIgnore]
    public string? AccessCardValue { get; set; }

    public List<ProperSlot>? SlotAccesses { get; set; }
}
