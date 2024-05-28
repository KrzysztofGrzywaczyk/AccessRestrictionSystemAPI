
using AccessControlSystem.Api.Models.DataTransferObjects;
using System.Collections.Generic;

namespace AccessControlSystem.Api.Models.Responses.AccessControlSystems;

public class ExistingSlotsResponse
{
    public List<SimplifiedSlot>? ExistingSlots { get; set; }
}
