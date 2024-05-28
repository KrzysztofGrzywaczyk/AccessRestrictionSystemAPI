
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models.Requests.AccessCards;
using AccessControlSystem.Api.UnitTests.Handlers.AccessCardsHandler.Contexts;
using AccessControlSystem.UnitTests;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.AccessCards;

public class CreateOrUpdateCardsHandlerTests : UnitTestsBase<CreateOrUpdateCardsHandlerTestsContext>
{
    [Theory]
    [AutoSubstituteData]
    public async Task WhenAccessCardNotExist_ShouldAddAccessCard(List<string> accessCardValues, string newAccessCardValue)
    {
        // arrange
        var request = new PutAccessCardsRequest() { AccessCardValues = new List<string> { newAccessCardValue } };
        var accessCards = accessCardValues.Select(x => new AccessCard()
        {
            Value = x
        }).ToList();

        await Context.WithAccessCards(accessCards);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        response.Error.Should().BeNull();
        await Context.AccessCardsShouldExistInDatabase(accessCards.Select(t => t.Value!).ToList());
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenAccessCardAlreadyExists_ShouldReturnNoContent(string accessCardValue)
    {
        // arrange
        var request = new PutAccessCardsRequest() { AccessCardValues = new List<string> { accessCardValue } };
        var accessCard = new AccessCard()
        {
            Value = accessCardValue
        };

        await Context.WithAccessCards(new List<AccessCard> { accessCard });

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        response.Error.Should().BeNull();
    }
}
