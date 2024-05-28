
using AccessControlSystem.Api.Models.Requests.AccessCards;
using AccessControlSystem.Api.Validation.Helpers;
using FluentValidation;

namespace AccessControlSystem.Api.Validation.AccessCards;

public class PutAccessCardsRequestValidator : AbstractValidator<PutAccessCardsRequest>
{
    public PutAccessCardsRequestValidator()
    {
        RuleFor(request => request.AccessCardValues)
            .NotEmpty().WithMessage(ValidationMessages.AccessCardValuesRequiredAndCannotBeEmpty);

        RuleForEach(request => request.AccessCardValues)
            .NotEmpty().
            WithMessage(ValidationMessages.AccessCardValueMustBeANonEmptyString);
    }
}
