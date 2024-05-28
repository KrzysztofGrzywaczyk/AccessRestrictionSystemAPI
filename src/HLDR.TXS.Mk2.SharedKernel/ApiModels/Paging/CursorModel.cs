
using System.Text.Json.Serialization;

namespace AccessControlSystem.SharedKernel.ApiModels.Paging;

public sealed class CursorModel<TCursor>
{
    public CursorModel()
    {
        if (default(TCursor) != null)
        {
            throw new System.ArgumentException("Cursor type has to be nullable");
        }
    }

    public string Next => CursorValue.ConvertFrom(NextDecoded);

    [JsonIgnore]
    public TCursor? NextDecoded { get; set; }
}