
using AccessControlSystem.Api.Models.Requests.Accesses;
using AccessControlSystem.Api.Validation.Extensions;
using FluentValidation;

namespace AccessControlSystem.Api.Validation.Accesses;

public class RemoveAccessesValidator : AbstractValidator<RemoveAccessesRequest>
{
    public RemoveAccessesValidator()
    {
        RuleFor(request => request)
            .ValidateSlot(
                slotNamesSelector: request => request.SlotNames,
                slotNumbersSelector: request => request.SlotNumbers);
    }
}
