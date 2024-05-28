
using AccessControlSystem.Api.Models.Requests.Accesses;
using AccessControlSystem.Api.Validation.Extensions;
using AccessControlSystem.Api.Validation.Helpers;
using FluentValidation;

namespace AccessControlSystem.Api.Validation.Accesses;

public class PutAccessesRequestValidator : AbstractValidator<PutAccessesRequest>
{
    public PutAccessesRequestValidator()
    {
        RuleFor(request => request)
            .ValidateSlot(
                slotNamesSelector: request => request.SlotNames,
                slotNumbersSelector: request => request.SlotNumbers,
                mode: ValidateSlotMode.Required);

        RuleFor(request => request.AccessCardValues!.AccessCardValues)
            .NotEmpty()
            .WithMessage(ValidationMessages.AccessCardValuesRequiredAndCannotBeEmpty)
        .DependentRules(() =>
            {
                RuleForEach(request => request.AccessCardValues!.AccessCardValues)
                    .NotEmpty()
                    .WithMessage(ValidationMessages.AccessCardValueMustBeANonEmptyString);
            });
    }
}
