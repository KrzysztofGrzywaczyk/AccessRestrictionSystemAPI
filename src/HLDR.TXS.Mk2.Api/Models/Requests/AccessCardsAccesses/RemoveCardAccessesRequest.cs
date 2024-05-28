
using AccessControlSystem.Api.Models.Bindings;
using AccessControlSystem.Api.Models.Responses.AccessCardAccesses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace AccessControlSystem.Api.Models.Requests.AccessCardAccesses;

public class RemoveCardAccessesRequest : IRequest<RemoveCardAccessesResponse>
{
    [FromRoute]
    [JsonIgnore]
    [SwaggerParameter(Description = "A AccessCard value that is presented to the device.")]
    public string? AccessCardValue { get; set; }

    [SwaggerParameter(Description = "Optional device identifier. If provided, the response will be filtered to only include details for the specified device.")]
    public string? DeviceId { get; set; }

    [SwaggerParameter(Description = "Optional slot names to further restrict scope within a specified device. Requires device Id to be specified. Provide as a comma-separated list for multiple values.")]
    [CommaSeparated]
    public string[]? SlotNames { get; set; }

    [SwaggerParameter(Description = "Optional slot number to further restrict scope within a specified device. Requires deviceId to be specified. Provide as a comma-separated list for multiple values.")]
    [CommaSeparated]
    public SlotNumbersEnum[]? SlotNumbers { get; set; }
}
