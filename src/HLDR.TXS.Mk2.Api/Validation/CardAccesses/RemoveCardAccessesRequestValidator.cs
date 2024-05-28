
using AccessControlSystem.Api.Models.Requests.AccessCardAccesses;
using AccessControlSystem.Api.Validation.Extensions;
using AccessControlSystem.Api.Validation.Helpers;
using FluentValidation;

namespace AccessControlSystem.Api.Validation.Accesses;

public class RemoveCardAccessesRequestValidator : AbstractValidator<RemoveCardAccessesRequest>
{
    public RemoveCardAccessesRequestValidator()
    {
        RuleFor(request => request)
            .ValidateSlot(
                slotNamesSelector: request => request.SlotNames,
                slotNumbersSelector: request => request.SlotNumbers,
                deviceIdSelector: request => request.DeviceId,
                ValidateSlotMode.RequiredWithDeviceId);
    }
}
