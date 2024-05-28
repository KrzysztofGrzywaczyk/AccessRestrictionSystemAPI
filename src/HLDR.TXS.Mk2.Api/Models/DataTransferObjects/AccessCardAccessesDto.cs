
using System.Collections.Generic;

namespace AccessControlSystem.Api.Models.DataTransferObjects;

public class AccessCardAccessesDto
{
    public int? AccessCardId { get; set; }

    public string? AccessCardValue { get; set; }

    public List<SlotAccesses>? SlotAccesses { get; set; }
}
