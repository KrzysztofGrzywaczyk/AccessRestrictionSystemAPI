
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace AccessControlSystem.SharedKernel.ApiModels;

[ExcludeFromCodeCoverage]
public abstract class ServiceResultBase
{
    [JsonIgnore]
    public int StatusCode { get; set; } = StatusCodes.Status200OK;

    [JsonIgnore]
    public bool IsSuccess => Error is null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ServiceError? Error { get; set; }
}

[ExcludeFromCodeCoverage]
#pragma warning disable SA1402 // File may only contain a single type
public abstract class ServiceResultBase<T> : ServiceResultBase
#pragma warning restore SA1402 // File may only contain a single type
    where T : class
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data { get; set; }
}