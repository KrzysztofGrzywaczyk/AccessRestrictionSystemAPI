using AccessControlSystem.Api.Models.DataTransferObjects;
using AccessControlSystem.Api.Models.Requests.Accesses;
using AccessControlSystem.Api.Validation.Helpers;
using AccessControlSystem.UnitTests;
using FluentValidation.TestHelper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Validation;

public class PutAccessesRequestValidatorTests : UnitTestsBase<PutAccessesRequestValidatorTestsContext>
{
    [Theory]
    [AutoSubstituteData]
    public async Task WhenProvidedAccessCardsIsEmpty_ShouldHaveValidationError(PutAccessesRequest request)
    {
        request.AccessCardValues!.AccessCardValues = null;

        var validationResult = await Context.Validator.TestValidateAsync(request);

        validationResult
            .ShouldHaveValidationErrorFor(x => x.AccessCardValues!.AccessCardValues)
            .WithErrorMessage(ValidationMessages.AccessCardValuesRequiredAndCannotBeEmpty);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenProvidedAccessCardsHasEmptyString_ShouldHaveValidationError(PutAccessesRequest request)
    {
        request.AccessCardValues = new AccessCardValueList
        {
            AccessCardValues = new List<string> { "testAccessCard", string.Empty }
        };

        var validationResult = await Context.Validator.TestValidateAsync(request);

        validationResult
            .ShouldHaveValidationErrorFor(x => x.AccessCardValues!.AccessCardValues)
            .WithErrorMessage(ValidationMessages.AccessCardValueMustBeANonEmptyString);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenSlotNamesAndSlotNumbersNotProvided_ShouldHaveValidationError(PutAccessesRequest request)
    {
        request.SlotNames = null;
        request.SlotNumbers = null;

        var validationResult = await Context.Validator.TestValidateAsync(request);
        validationResult
            .ShouldHaveValidationErrorFor($"SlotNames, SlotNumbers")
            .WithErrorMessage(ValidationMessages.SlotInfoNotGiven);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenSlotNamesAndSlotNumbersBothProvided_ShouldHaveValidationError(PutAccessesRequest request)
    {
        var validationResult = await Context.Validator.TestValidateAsync(request);
        validationResult
            .ShouldHaveValidationErrorFor($"SlotNames, SlotNumbers")
            .WithErrorMessage(ValidationMessages.SlotNamesAndSlotNumbersGivenSimultaneously);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenOnlySlotNamesProvided_ShouldNotHaveValidationError(PutAccessesRequest request)
    {
        request.SlotNumbers = null;
        var validationResult = await Context.Validator.TestValidateAsync(request);
        validationResult
            .ShouldNotHaveValidationErrorFor(x => x);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenOnlySlotNumbersProvided_ShouldNotHaveValidationError(PutAccessesRequest request)
    {
        request.SlotNames = null;
        var validationResult = await Context.Validator.TestValidateAsync(request);
        validationResult
            .ShouldNotHaveValidationErrorFor(x => x);
    }
}
