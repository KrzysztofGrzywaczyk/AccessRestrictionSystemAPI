
namespace AccessControlSystem.Api.Validation.Helpers;

public static class ValidationMessages
{
    public const string AccessCardsHaveAccesses = "AccessCards can not be removed because they have associated accesses.";
    public const string AccessCardsNotFound = "AccessCards not found.";
    public const string AccessCardValueMustBeANonEmptyString = "Each item in accessCard value must be a non-empty string.";
    public const string AccessCardValuesRequiredAndCannotBeEmpty = "At least one non-empty accessCard value is required.";
    public const string AccessesNotAssigned = "No slot accesses could be assigned to accessCard.";
    public const string AccessesNotFound = "Accesses not found.";
    public const string AccessesNotFoundOnSourceDevice = "No accesses on source device to {0}.";
    public const string DeviceHasSlots = "Device can not be removed because it has associated slots.";
    public const string DeviceIdIsRequiredAndCannotBeEmpty = "Device Id is required and cannot be empty or whitespace.";
    public const string DeviceNameIsRequiredAndCannotBeEmpty = "Device name is required and cannot be empty or whitespace.";
    public const string DeviceNotFound = "Device not found.";
    public const string SlotInfoNotGiven = "Missing list of slots, a list of slot names OR slot numbers is required.";
    public const string SlotNameIsRequiredAndCannotBeEmpty = "Slot name is required and cannot be empty or whitespace.";
    public const string SlotNamesAndSlotNumbersGivenSimultaneously = "The list of slots was expressed as slot names AND slot numbers; only one OR the other is allowed.";
    public const string SlotNamesOrSlotNumbersGivenWithoutDevice = "To scope by slot names or slot numbers a device Id is mandatory.";
    public const string SlotNumberIsRequiredAndMustBeAnIntegerBetween1And8 = "SlotNumber is required and must be an integer between 1 and 8.";
    public const string SlotsHaveAccesses = "Slots can not be removed because they have associated accesses.";
    public const string SlotsListIsRequired = "A list of slots is required.";
    public const string SlotsNotFound = "Slots not found.";
}
