
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AccessControlSystem.Api.Entities;

public class AccessCard
{
    [Key]
    public int AccessCardId { get; set; }

    public string? Value { get; set; }

    [JsonIgnore]
    public virtual ICollection<Slot>? Slots { get; set; }
}