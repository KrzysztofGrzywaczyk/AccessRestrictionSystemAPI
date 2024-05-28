
using AccessControlSystem.Api.Models.Requests.AccessControlSystems;
using AccessControlSystem.Api.Validation.Helpers;
using FluentValidation;

namespace AccessControlSystem.Api.Validation.AccessControlSystems;

public class PutAccessControlSystemsRequestValidator : AbstractValidator<PutDeviceRequest>
{
    public PutAccessControlSystemsRequestValidator()
    {
        RuleFor(request => request.DeviceName)
                .NotEmpty()
                .WithMessage(ValidationMessages.DeviceNameIsRequiredAndCannotBeEmpty);
    }
}
