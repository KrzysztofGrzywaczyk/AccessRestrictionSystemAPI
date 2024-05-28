
using AccessControlSystem.Api.Models.Bindings;
using AccessControlSystem.Api.Models.Responses.Accesses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace AccessControlSystem.Api.Models.Requests.Accesses;

public class GetAccessesRequest : IRequest<GetAccessesResponse>
{
    [FromRoute]
    [JsonIgnore]
    [SwaggerParameter(Description = "The device identifier.")]
    public string? DeviceId { get; set; }

    [SwaggerParameter(Description = "Optional slot names to further restrict scope within a specified device. Requires device Id. Provide as a comma-separated list for multiple values.")]
    [CommaSeparated]
    public string[]? SlotNames { get; set; }

    [SwaggerParameter(Description = "Optional slot number to further restrict scope within a specified device. Requires device Id. Provide as a comma-separated list for multiple values.")]
    [CommaSeparated]
    public SlotNumbersEnum[]? SlotNumbers { get; set; }
}
