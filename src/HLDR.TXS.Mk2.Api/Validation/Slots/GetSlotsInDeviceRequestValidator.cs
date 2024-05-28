
using AccessControlSystem.Api.Models.Requests.Slots;
using AccessControlSystem.Api.Validation.Extensions;
using FluentValidation;

namespace AccessControlSystem.Api.Validation.Accesses;

public class GetSlotsInDeviceRequestValidator : AbstractValidator<GetSlotsInDeviceRequest>
{
    public GetSlotsInDeviceRequestValidator()
    {
        RuleFor(request => request)
            .ValidateSlot(
                slotNamesSelector: request => request.SlotNames,
                slotNumbersSelector: request => request.SlotNumbers);
    }
}
