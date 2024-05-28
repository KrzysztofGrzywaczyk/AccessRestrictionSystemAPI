
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models.DataTransferObjects;
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

public class GetCardsHandlerTests : UnitTestsBase<GetCardsHandlerTestsContext>
{
    [Theory]
    [AutoSubstituteData]
    public async Task WhenAccessCardsExistsInDatabase_ShouldReturnAllAccessCards(List<string> AccessCardValues)
    {
        // arrange
        var request = new GetAccessCardsRequest();
        var AccessCardsEntity = AccessCardValues.Select(x => new AccessCard()
        {
            Value = x
        }).ToList();

        await Context.WithAccessCards(AccessCardsEntity);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        response.Error.Should().BeNull();
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<AccessCardValueList>();
        response.Data!.AccessCardValues.Should().NotBeNull();
        response.Data.AccessCardValues!.Count().Should().Be(3);
        response.Data.AccessCardValues!.First().Should().Be(AccessCardValues[0]);
    }

    [Fact]
    public async Task WhenAccessCardsNotExistsInDatabase_ShouldReturnNotFound()
    {
        // arrange
        var request = new GetAccessCardsRequest();

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeFalse();
        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        response.Data.Should().BeNull();
        response.Error.Should().NotBeNull();
        response.Error!.Type.Should().Be(ErrorType.ResourceNotFound);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenAdditionalAccessCardValuesProvidedExists_ShouldReturnFilteredAccessCards(string AccessCardValue)
    {
        // arrange
        var request = new GetAccessCardsRequest() { AccessCardValues = new string[] { AccessCardValue } };
        var AccessCard = new AccessCard()
        {
            Value = AccessCardValue
        };

        await Context.WithAccessCards(new List<AccessCard> { AccessCard });

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        response.Error.Should().BeNull();
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<AccessCardValueList>();
        response.Data!.AccessCardValues.Should().NotBeNull();
        response.Data.AccessCardValues!.Count().Should().Be(1);
        response.Data.AccessCardValues!.First().Should().Be(AccessCardValue);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenProvidedWrongAdditionalAccessCardValues_ShouldReturnNotFound(List<string> AccessCardValues, string wrongAccessCardValue)
    {
        // arrange
        var request = new GetAccessCardsRequest() { AccessCardValues = new string[] { wrongAccessCardValue } };
        var AccessCards = AccessCardValues.Select(x => new AccessCard()
        {
            Value = x
        }).ToList();

        await Context.WithAccessCards(AccessCards);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeFalse();
        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        response.Data.Should().BeNull();
        response.Error.Should().NotBeNull();
        response.Error!.Type.Should().Be(ErrorType.ResourceNotFound);
    }
}
