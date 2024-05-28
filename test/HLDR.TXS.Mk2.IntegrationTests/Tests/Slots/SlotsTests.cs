using AccessControlSystem.IntegrationTests.Core;
using System.Net;

namespace AccessControlSystem.IntegrationTests.Tests.Slot;

[Collection(nameof(AssemblyCollection))]
public class SlotsTests : TestsBase<SlotsContext, AssemblySharedFixture>
{
    public SlotsTests(ITestOutputHelper outputHelper, AssemblySharedFixture fixture)
        : base(outputHelper, fixture)
    {
    }

    [Fact]
    public async Task WithExistingDevice_WhenGetAllSlotsInDeviceQueried_ShouldReturnOkResponseWithListOfDevices()
    {
        await Context.WhenGetSlotsQueried();

        await Context.ThenListOfSlotsShouldBeReturned();
    }

    [Fact]
    public async Task WithExistingDevice_WhenGetSpecificSlotQueriedWithNameFilter_ShouldReturnOkResponseWithSpecificDevices()
    {
        await Context.WhenGetSlotsQueriedWithNameFilter();

        await Context.ThenFilteredSlotsAreReturned();
    }

    [Fact]
    public async Task WithExistingDevice_WhenGetSpecificSlotQueriedWithSlotNumberFilter_ShouldReturnOkResponseWithSpecificDevices()
    {
        await Context.WhenGetSlotsQueriedWithSlotNumberFilter();

        await Context.ThenFilteredSlotsAreReturned();
    }

    [Fact]
    public async Task WhenPutSlotQueried_ShouldReturnCreatedResponse()
    {
        // creation in context initialization
        await Context.ThenStatusCodeIsReturned(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task WithExistingSlotInDevice_WhenDeleteSpecificSlotQueriedWithFilter_ShouldReturnResponseWithNoContent()
    {
        await Context.WhenDeleteSlotQueried();

        await Context.ThenStatusCodeIsReturned(HttpStatusCode.NoContent);
    }
}
