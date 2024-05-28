using AccessControlSystem.IntegrationTests.Core;
using System.Net;

namespace AccessControlSystem.IntegrationTests.Tests.Device;

[Collection(nameof(AssemblyCollection))]
public class DevicesTests : TestsBase<DevicesContext, AssemblySharedFixture>
{
    public DevicesTests(ITestOutputHelper outputHelper, AssemblySharedFixture fixture)
        : base(outputHelper, fixture)
    {
    }

    [Fact]
    public async Task WithExistingDevice_WhenGetAllDevicesQueried_ShouldReturnOkResponseWithListOfDevices()
    {
        await Context.WhenGetDevicesQueried();

        await Context.ThenListOfDevicesShouldBeReturned();
    }

    [Fact]
    public async Task WithExistingDevice_WhenGetSpecificDeviceQueriedWithFilter_ShouldReturnOkResponseWithSpecificDevice()
    {
        await Context.WhenGetDevicesQueriedWithFilter();

        await Context.ThenFilteredDevicesShouldBeReturned();
    }

    [Fact]
    public async Task WhenPutDeviceQueried_ShouldReturnCreatedResponse()
    {
        // creation in initialize context
        await Context.ThenStatusCodeIsReturned(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task WithExistingDevice_WhenDeleteSpecificDevicesQueriedWithFilter_ShouldReturnResponseWithNoContent()
    {
        await Context.WhenDeleteDeviceIsQueried();

        await Context.ThenStatusCodeIsReturned(HttpStatusCode.NoContent);
    }
}
