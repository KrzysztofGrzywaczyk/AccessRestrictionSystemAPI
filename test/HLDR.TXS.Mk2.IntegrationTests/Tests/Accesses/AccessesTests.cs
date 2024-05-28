using AccessControlSystem.IntegrationTests.Core;
using System.Net;

namespace AccessControlSystem.IntegrationTests.Tests.Accesses;

[Collection(nameof(AssemblyCollection))]
public class AccessesTests : TestsBase<AccessesContext, AssemblySharedFixture>
{
    public AccessesTests(ITestOutputHelper outputHelper, AssemblySharedFixture fixture)
            : base(outputHelper, fixture)
    {
    }

    [Fact]
    public async Task WithExistingTestAccessCardAndSlotOnDevice_WhenGetAllAccessesOfDeviceQueried_ShouldReturnOkResponseWithListOfAccessCardAccesses()
    {
        await Context.WhenGetAllAccessesInDeviceQueried();
        await Context.ThenListOfAccessCardAccessesIsReturned();
    }

    [Fact]
    public async Task WithExistingTestAccessCardAndSlotOnDevice_WhenGetAccessesFilteredWithSlotNamesQueried_ShouldReturnOkResponseWithListOfAccessCardAccesses()
    {
        await Context.WhenGetAccessesWithSlotsNamesFilterQueried();
        await Context.ThenListOfAccessCardAccessesIsReturned();
    }

    [Fact]
    public async Task WithExistingTestAccessCardAndSlotOnDevice_WhenGetAccessesFilteredWithSlotslotNumbersQueried_ShouldReturnOkResponseWithListOfAccessCardAccesses()
    {
        await Context.WhenGetAccessesWithSlotsSlotNumbersFilterQueried();
        await Context.ThenListOfAccessCardAccessesIsReturned();
    }

    [Fact]
    public async Task WithExistingTestAccessCardAndSlotOnDevice_WhenPutAccessQueriedWithSlotName_ShouldReturnNoContentResponse()
    {
        await Context.WhenPutAccessQueriedWithSlotNames();
        await Context.ThenStatusCodeIsReturned(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task WithExistingTestAccessCardAndSlotOnDevice_WhenPutAccessQueriedWithSlotslotNumber_ShouldReturnNoContentResponse()
    {
        await Context.WhenPutAccessQueriedWithSlotslotNumbers();
        await Context.ThenStatusCodeIsReturned(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task WithExistingTestAccessCardAndSlotOnDevice_WhenDeleteAccessesIsQueried_ShouldDeleteAccess()
    {
        await Context.WhenDeleteAllAccessesQueried();
        await Context.ThenStatusCodeIsReturned(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task WithExistingTestAccessCardAndSlotOnDevice_WhenDeleteAccessesIsQueriedWithDeviceFilter_ShouldDeleteDeviceAccesses()
    {
        await Context.WhenDeleteAllAccessesInDeviceQueried();
        await Context.ThenStatusCodeIsReturned(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task WithExistingTestAccessCardAndSlotOnDevice_WhenDeleteAccessesIsQueriedWithSlotNameFilter_ShouldDeleteDeviceAccesses()
    {
        await Context.WhenDeleteAccessesQueriedWithSlotNameFilter();
        await Context.ThenStatusCodeIsReturned(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task WithExistingTestAccessCardAndSlotOnDevice_WhenDeleteAccessesIsQueriedWithSlotslotNumberFilter_ShouldDeleteDeviceAccesses()
    {
        await Context.WhenDeleteAccessesQueriedWithSlotslotNumberFilter();
        await Context.ThenStatusCodeIsReturned(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task WithExistingTestAccessCardAndSlotOnDevice_WhenCopyAccessesIsQueried_ShouldReturnNoContentAndCopyAccesses()
    {
        await Context.WithSecondTestDeviceCreated();

        await Context.WhenCopyAccessesToSecondControlSystemQueried();

        await Context.ThenStatusCodeIsReturned(HttpStatusCode.NoContent);
        await Context.ThenSlotIsCopiedIfNotExists();
        await Context.ThenAccessesAreCopied();
    }

    [Fact]
    public async Task WithExistingTestAccessCardAndSlotOnDevice_WhenMoveAccessesIsQueried_ShouldReturnNoContentAndMoveSourceAccesses()
    {
        await Context.WithSecondTestDeviceCreated();

        await Context.WhenMoveAccessesToSecondControlSystemQueried();
        await Context.ThenSlotIsCopiedIfNotExists();
        await Context.ThenAccessesAreMoved();
    }
}
