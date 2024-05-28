
using System.Collections.Generic;

namespace AccessControlSystem.Api.Models.DataTransferObjects;

public class NonExistentSlotsOrCardsResponse
{
    public List<ProperSlot>? NonExistentSlots { get; set; }

    public List<string?>? NonExistentAccessCards { get; set; }
}
