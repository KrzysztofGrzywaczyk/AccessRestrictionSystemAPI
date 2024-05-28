
using AccessControlSystem.Api.Models.Requests.AccessCards;
using AccessControlSystem.Api.Validation.Helpers;
using FluentValidation;
using System.Linq;

namespace AccessControlSystem.Api.Validation.AccessCards;

public class RemoveAccessCardsRequestValidator : AbstractValidator<RemoveAccessCardsRequest>
{
    public RemoveAccessCardsRequestValidator()
    {
        RuleFor(request => request.AccessCardValues)
            .Must(AccessCardValues => AccessCardValues != null && AccessCardValues.Any(AccessCard => !string.IsNullOrWhiteSpace(AccessCard)))
            .WithMessage(ValidationMessages.AccessCardValuesRequiredAndCannotBeEmpty);
    }
}
