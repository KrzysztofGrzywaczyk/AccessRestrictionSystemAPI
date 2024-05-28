
using AccessControlSystem.Api.Models.Requests.Slots;
using AccessControlSystem.Api.Validation.Helpers;
using FluentValidation;

namespace AccessControlSystem.Api.Validation.Slots;

public class PutSlotsInDeviceRequestValidator : AbstractValidator<PutSlotsInDeviceRequest>
{
    public PutSlotsInDeviceRequestValidator()
    {
        RuleFor(request => request.Slots)
            .NotNull().WithMessage(ValidationMessages.SlotsListIsRequired);

        RuleForEach(request => request.Slots)
            .ChildRules(slot =>
            {
                slot.RuleFor(s => s.SlotName)
                    .NotEmpty()
                    .WithMessage(ValidationMessages.SlotNameIsRequiredAndCannotBeEmpty);
            });

        RuleForEach(request => request.Slots)
            .Must(slot => slot.SlotNumber.HasValue && slot.SlotNumber >= 1 && slot.SlotNumber <= 4)
            .WithMessage(ValidationMessages.SlotNumberIsRequiredAndMustBeAnIntegerBetween1And8);
    }
}
