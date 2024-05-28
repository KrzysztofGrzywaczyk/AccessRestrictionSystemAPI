
using System.Text.Json.Serialization;

namespace AccessControlSystem.SharedKernel.ApiModels.Paging;

public abstract class CursorResultBase<T, TCursor> : ServiceResultBase<T>
    where T : class
{
    /// <summary>
    /// Gets or sets a cursor value.
    /// </summary>
    /// <remarks>
    /// JsonPropertyOrder is used to make sure that the cursor appears as the last deserialized property on the client side.
    /// </remarks>
    [JsonPropertyOrder(9999)]
    public CursorModel<TCursor> Cursor { get; set; } = new();
}