
using AccessControlSystem.Api.Models.Requests.AccessCardAccesses;
using AccessControlSystem.Api.Validation.Helpers;
using FluentValidation;

namespace AccessControlSystem.Api.Validation.AccessCardAccesses;

public class PutCardAccessesRequestValidator : AbstractValidator<PutCardAccessesRequest>
{
    public PutCardAccessesRequestValidator()
    {
        RuleForEach(request => request.SlotAccesses)
            .Custom((properSlot, context) =>
            {
                if (properSlot.SlotNumber is not null && properSlot.SlotName is not null)
                {
                    context.AddFailure("SlotNameAndSlotNumberGivenSimultaneously", "Only one of slotName or slotNumber should be provided.");
                }
            });

        RuleFor(request => request.SlotAccesses)
            .NotEmpty()
            .WithMessage("Slot accesses collection cannot be null.");

        RuleForEach(request => request.SlotAccesses)
            .Must(properSlot => !string.IsNullOrWhiteSpace(properSlot.SlotId))
            .WithMessage(ValidationMessages.DeviceIdIsRequiredAndCannotBeEmpty);

        RuleForEach(request => request.SlotAccesses)
            .Where(properSlot => properSlot.SlotName is not null)
            .Must(properSlot => !string.IsNullOrWhiteSpace(properSlot.SlotName))
            .WithMessage(ValidationMessages.SlotNameIsRequiredAndCannotBeEmpty);

        RuleForEach(request => request.SlotAccesses)
            .Where(properSlot => properSlot.SlotNumber is not null)
            .Must(properSlot => properSlot.SlotNumber.HasValue && properSlot.SlotNumber >= 1 && properSlot.SlotNumber <= 4)
            .WithMessage(ValidationMessages.SlotNumberIsRequiredAndMustBeAnIntegerBetween1And8);
    }
}
