
using AccessControlSystem.Api.Models.Bindings;
using AccessControlSystem.Api.Models.Responses.Slots;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace AccessControlSystem.Api.Models.Requests.Slots;

public class RemoveSlotsFromDeviceRequest : IRequest<RemoveSlotsFromDeviceResponse>
{
    [FromRoute]
    [JsonIgnore]
    [SwaggerParameter(Description = "The device identifier.")]
    public string? DeviceId { get; set; }

    [SwaggerParameter(Description = "Optional slot names to further restrict scope within a specified device. Requires device Id.")]
    [CommaSeparated]
    public string[]? SlotNames { get; set; }

    [SwaggerParameter(Description = "Optional slot numbers to further restrict scope within a specified device. Requires device Id.")]
    [CommaSeparated]
    public SlotNumbersEnum[]? SlotNumbers { get; set; }
}
