
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccessControlSystem.Api.Entities;

public class AccessControlDevice
{
    [Key]
    public int DeviceId { get; set; }

    public string? DeviceName { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Slot>? Slots { get; set; }
}