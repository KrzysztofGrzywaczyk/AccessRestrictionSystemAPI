
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace AccessControlSystem.SharedKernel.ApiModels;

[ExcludeFromCodeCoverage]
public class ServiceError(string type, int status = StatusCodes.Status400BadRequest, string? title = null, string? detail = null, string? traceId = null, IEnumerable<InvalidParameter>? invalidParameters = null)
{
    /// <summary>
    /// Gets or sets URI or relative path that defines what the problem is.
    /// </summary>
    public string Type { get; set; } = type;

    /// <summary>
    /// Gets or sets the status member represents the same HTTP status code.
    /// </summary>
    public int Status { get; set; } = status;

    /// <summary>
    /// Gets or sets a short human-readable message of the problem type. It should NOT change from occurrence to occurrence.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Title { get; set; } = title;

    /// <summary>
    /// Gets or sets human-readable explanation of the exact issue that occurred. This can differ from occurrence to occurrence.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Detail { get; set; } = detail;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TraceId { get; set; } = traceId;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<InvalidParameter>? InvalidParameters { get; set; } = invalidParameters;
}
