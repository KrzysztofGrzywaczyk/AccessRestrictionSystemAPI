
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AccessControlSystem.Api.Entities;

public class Slot
{
    [Key]
    public int SlotId { get; set; }

    [JsonIgnore]
    public int DeviceId { get; set; }

    public string? SlotName { get; set; }

    public int SlotNumber { get; set; }

    [JsonIgnore]
    public AccessControlDevice? Device { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public virtual ICollection<AccessCard>? AccessCards { get; set; }
}