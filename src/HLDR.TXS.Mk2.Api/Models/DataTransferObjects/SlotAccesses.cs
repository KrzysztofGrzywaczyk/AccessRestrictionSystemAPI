
using System.Collections.Generic;

namespace AccessControlSystem.Api.Models.DataTransferObjects;

public class SlotAccesses
{
    public int? DeviceId { get; set; }

    public string? DeviceName { get; set; }

    public string? Description { get; set; }

    public List<GetSlotsDto>? Slots { get; set; }
}
