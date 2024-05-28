
using AccessControlSystem.Api.Models;

namespace AccessControlSystem.Api.Validation.Helpers;

public enum ValidateSlotMode
{
    Optional,
    Required,
    RequiredWithDeviceId
}

public static class ValidationHelpers
{
    public static SlotValidationResult ValidateSlotsOrSlotNumbers(string[]? slotNames, SlotNumbersEnum[]? slotNumbers, string? deviceId = "", ValidateSlotMode slotValidationMode = ValidateSlotMode.Optional)
    {
        if (slotNames != null && slotNumbers != null)
        {
            return SlotValidationResult.BothPresent();
        }

        if (slotValidationMode == ValidateSlotMode.Required && slotNames == null && slotNumbers == null)
        {
            return SlotValidationResult.BothMissing();
        }

        if (slotValidationMode == ValidateSlotMode.RequiredWithDeviceId && (slotNames != null || slotNumbers != null) && deviceId == null)
        {
            return SlotValidationResult.DeviceIdMissing();
        }

        return SlotValidationResult.Valid();
    }
}
