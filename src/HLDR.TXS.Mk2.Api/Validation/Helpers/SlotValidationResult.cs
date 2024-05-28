
namespace AccessControlSystem.Api.Validation.Helpers;

public class SlotValidationResult
{
    private SlotValidationResult(bool isValid, string name, string reason)
    {
        IsValid = isValid;
        Name = name;
        Reason = reason;
    }

    public bool IsValid { get; }

    public string Name { get; }

    public string Reason { get; }

    public static SlotValidationResult Valid()
    {
        return new SlotValidationResult(true, string.Empty, string.Empty);
    }

    public static SlotValidationResult BothMissing()
    {
        return new SlotValidationResult(false, nameof(ValidationMessages.SlotInfoNotGiven), ValidationMessages.SlotInfoNotGiven);
    }

    public static SlotValidationResult BothPresent()
    {
        return new SlotValidationResult(false, nameof(ValidationMessages.SlotNamesAndSlotNumbersGivenSimultaneously), ValidationMessages.SlotNamesAndSlotNumbersGivenSimultaneously);
    }

    public static SlotValidationResult DeviceIdMissing()
    {
        return new SlotValidationResult(false, nameof(ValidationMessages.SlotNamesOrSlotNumbersGivenWithoutDevice), ValidationMessages.SlotNamesOrSlotNumbersGivenWithoutDevice);
    }
}
