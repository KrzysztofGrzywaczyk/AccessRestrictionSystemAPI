
using System.Collections.Generic;

namespace AccessControlSystem.Api.Models.DataTransferObjects;

public class RemovedAndWithAccessesSlots
{
    public List<SimplifiedSlot>? RemovedSlots { get; set; }

    public List<SimplifiedSlot>? SlotsWithAccesses { get; set; }
}
