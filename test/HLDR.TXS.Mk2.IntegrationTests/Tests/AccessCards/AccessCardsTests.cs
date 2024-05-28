using AccessControlSystem.IntegrationTests.Core;
using System.Net;

namespace AccessControlSystem.IntegrationTests.Tests.AccessCard;

[Collection(nameof(AssemblyCollection))]
public class AccessCardsTests(ITestOutputHelper outputHelper, AssemblySharedFixture fixture) : TestsBase<AccessCardsContext, AssemblySharedFixture>(outputHelper, fixture)
{
    [Fact]
    public async Task WithExistingAccessCard_WhenGetAllAccessCardsQueried_ShouldReturnOkResponseWithListOfAccessCards()
    {
        await Context.WhenGetAllAccessCardsQueried();
        await Context.ThenListOfAccessCardsShouldBeReturned();
    }

    [Fact]
    public async Task WithExistingAccessCard_WhenGetSpecificAccessCardQueriedWithFilter_ShouldReturnOkResponseWithSpecificAccessCard()
    {
        await Context.WhenGetAccessCardQueriedWithFilter();

        await Context.ThenFilteredAccessCardsShouldBeReturned();
    }

    [Fact]
    public async Task WhenPutAccessCardsQueried_ShouldReturnCreatedResponse()
    {
        // creation in initialize context
        await Context.ThenStatusCodeIsReturned(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task WithExistingAccessCard_WhenDeleteSpecificAccessCardQueriedWithFilter_ShouldReturnResponseWithNoContent()
    {
        await Context.WhenDeleteAccessCardQueried();

        await Context.ThenStatusCodeIsReturned(HttpStatusCode.NoContent);
    }
}
