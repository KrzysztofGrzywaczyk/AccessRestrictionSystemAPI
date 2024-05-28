
using AccessControlSystem.Api.Models;
using AccessControlSystem.Api.Validation.Helpers;
using FluentValidation;
using System;

namespace AccessControlSystem.Api.Validation.Extensions;

public static class CustomValidationExtensions
{
    public static IRuleBuilder<T, object> ValidateSlot<T>(
            this IRuleBuilder<T, object> ruleBuilder,
            Func<T, string[]?> slotNamesSelector,
            Func<T, SlotNumbersEnum[]?> slotNumbersSelector,
            Func<T, string?>? deviceIdSelector = null,
            ValidateSlotMode mode = ValidateSlotMode.Optional)
    {
        ruleBuilder.Custom((_, context) =>
        {
            var instance = context.InstanceToValidate;
            var slotNames = slotNamesSelector(instance);
            var slotNumbers = slotNumbersSelector(instance);
            var deviceId = deviceIdSelector != null ? deviceIdSelector(instance) : null;

            var validationResult = ValidationHelpers.ValidateSlotsOrSlotNumbers(slotNames, slotNumbers, deviceId, mode);
            if (!validationResult.IsValid)
            {
                context.AddFailure($"{nameof(slotNames)}, {nameof(slotNumbers)}", validationResult.Reason);
            }
        });

        return ruleBuilder;
    }
}
