
using System.Collections.Generic;

namespace AccessControlSystem.Api.Models.DataTransferObjects;

public class AssignedAndFailedCardAccesses
{
    public List<ProperSlot>? AssignedSlotAccesses { get; set; }

    public List<ProperSlot>? FailedSlotAccesses { get; set; }
}
