
using AccessControlSystem.Api.Models.Requests.Accesses;
using AccessControlSystem.Api.Validation.Extensions;
using FluentValidation;

namespace AccessControlSystem.Api.Validation.Accesses;

public class GetAccessesRequestValidator : AbstractValidator<GetAccessesRequest>
{
    public GetAccessesRequestValidator()
    {
        RuleFor(request => request)
            .ValidateSlot(
                slotNamesSelector: request => request.SlotNames,
                slotNumbersSelector: request => request.SlotNumbers);
    }
}
