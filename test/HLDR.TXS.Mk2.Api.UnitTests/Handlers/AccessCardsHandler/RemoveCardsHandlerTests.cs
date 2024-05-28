
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models.Requests.AccessCards;
using AccessControlSystem.Api.UnitTests.Handlers.AccessCardsHandler.Contexts;
using AccessControlSystem.SharedKernel.ApiModels;
using AccessControlSystem.UnitTests;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.AccessCards;

public class RemoveCardsHandlerTests : UnitTestsBase<RemoveCardsHandlerTestsContext>
{
    [Theory]
    [AutoSubstituteData]
    public async Task WhenAccessCardExist_ShouldRemovedAccessCard(List<string> accessCardValues)
    {
        // arrange
        var request = new RemoveAccessCardsRequest() { AccessCardValues = accessCardValues.ToArray() };
        var AccessCards = accessCardValues.Select(x => new AccessCard()
        {
            Value = x
        }).ToList();

        await Context.WithAccessCards(AccessCards);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        response.Error.Should().BeNull();
        await Context.AccessCardsShouldNotExistsInDatabase(accessCardValues);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenAccessCardNotExist_ShouldReturnNotFound(List<string> accessCardValues, string wrongAccessCardValue)
    {
        // arrange
        string[] accessCardToRemove = { wrongAccessCardValue };
        var request = new RemoveAccessCardsRequest() { AccessCardValues = accessCardToRemove };
        var accessCards = accessCardValues.Select(x => new AccessCard()
        {
            Value = x
        }).ToList();

        await Context.WithAccessCards(accessCards);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeFalse();
        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        response.Error.Should().NotBeNull();
        response.Data.Should().BeNull();
        response.Error?.Type.Should().Be(ErrorType.ResourceNotFound);
    }
}
