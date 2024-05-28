using AccessControlSystem.IntegrationTests.Core;
using System.Net;

namespace AccessControlSystem.IntegrationTests.Tests;

[Collection(nameof(AssemblyCollection))]
public class CardAccessesTests(ITestOutputHelper outputHelper, AssemblySharedFixture fixture) : TestsBase<CardAccessesContext, AssemblySharedFixture>(outputHelper, fixture)
{
    [Fact]
    public async Task WithExsistingTestAccessCardAndSlotOnDevice_WhenGetAllAccessCardsAccessesQueried_ShouldReturnOkResponseWithListOfAccessCardAccesses()
    {
        await Context.WhenGetAllAccessesQueried();
        await Context.ThenListOfAccessCardAccessesIsReturned();
    }

    [Fact]
    public async Task WithExsistingTestAccessCardAndSlotOnDevice_WhenGetAllAccessCardsWithAccessesInDeviceFilterQueried_ShouldReturnOkResponseWithListOfAccessCardAccesses()
    {
        await Context.WhenGetAllAccessesInDeviceQueried();
        await Context.ThenListOfAccessCardAccessesIsReturned();
    }

    [Fact]
    public async Task WithExsistingTestAccessCardAndSlotOnDevice_WhenGetAllAccessCardsAccessesQueriedWithNamesFilter_ShouldReturnOkResponseWithListOfAccessCardAccesses()
    {
        await Context.WhenGetAccessesQueriedWithNamesFilter();
        await Context.ThenListOfAccessCardAccessesIsReturned();
    }

    [Fact]
    public async Task WithExsistingTestAccessCardAndSlotOnDevice_WhenGetAllAccessCardsAccessesQueriedWithSlotNumbersFilter_ShouldReturnOkResponseWithListOfAccessCardAccesses()
    {
        await Context.WhenGetAccessesQueriedWithSlotNumbersFilter();
        await Context.ThenListOfAccessCardAccessesIsReturned();
    }

    [Fact]
    public async Task WithExsistingTestAccessCardAndSlotOnDevice_WhenPutAccessCardAccessQueriedWithSlotName_ShouldReturnNoContentResponse()
    {
        await Context.WhenPutAccessQueriedWithSlotNames();
        await Context.ThenStatusCodeIsReturned(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task WithExsistingTestAccessCardAndSlotOnDevice_WhenPutAccessCardAccessQueriedWithSlotslotNumber_ShouldReturnNoContentResponse()
    {
        await Context.WhenPutAccessQueriedWithSlotNumbers();
        await Context.ThenStatusCodeIsReturned(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task WithExsistingTestAccessCardAndSlotOnDevice_WhenPutAccessCardAccessesIsQueridWithCorrectAndIncorrectSlotAccesses_ShouldReturnMultistatusResponseWithBody()
    {
        await Context.WhenPutAccessWithAdditionalIncorrectAccessIsQueried();
        await Context.ThenMultistatusResponseIsReturned();
    }

    [Fact]
    public async Task WithExsistingTestAccessCardAndSlotOnDevice_WhenDeleteAccessCardAccessesIsQuerid_ShouldDeleteAccess()
    {
        await Context.WhenDeleteAllAccessesQueried();
        await Context.ThenStatusCodeIsReturned(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task WithExsistingTestAccessCardAndSlotOnDevice_WhenDeleteAccessCardAccessesIsQueridWithDeviceFilter_ShouldDeleteDeviceAccesses()
    {
        await Context.WhenDeleteAllAccessesInDeviceQueried();
        await Context.ThenStatusCodeIsReturned(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task WithExsistingTestAccessCardAndSlotOnDevice_WhenDeleteAccessCardAccessesIsQueridWithSlotNameFilter_ShouldDeleteDeviceAccesses()
    {
        await Context.WhenDeleteAccessesQueriedWithSlotNamesFilter();
        await Context.ThenStatusCodeIsReturned(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task WithExsistingTestAccessCardAndSlotOnDevice_WhenDeleteAccessCardAccessesIsQueridWithSlotslotNumberFilter_ShouldDeleteDeviceAccesses()
    {
        await Context.WhenDeleteAccessesQueriedWithSlotslotNumbersFilter();
        await Context.ThenStatusCodeIsReturned(HttpStatusCode.NoContent);
    }
}
