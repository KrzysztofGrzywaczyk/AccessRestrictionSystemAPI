
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models.Requests.AccessControlSystems;
using AccessControlSystem.Api.UnitTests.Handlers.AccessControlSystemsHandler.Contexts;
using AccessControlSystem.UnitTests;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.AccessControlSystems;

public class CreateOrModifyDeviceHandlerTests : UnitTestsBase<CreateOrModifyDeviceHandlerTestsContext>
{
    [Theory]
    [AutoSubstituteData]
    public async Task WhenDeviceNotExistInDatabase_ShouldAddNewDevice(List<string> devices, string deviceId)
    {
        // arrange
        var request = new PutDeviceRequest() { DeviceId = deviceId, DeviceName = deviceId };
        var devicesEntity = devices.Select(x => new AccessControlDevice()
        {
            DeviceName = x,
            Description = $"UnitTest-{x}",
        }).ToList();

        await Context.WithDevice(devicesEntity);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        response.Error.Should().BeNull();
        await Context.DeviceShouldBeAddedToDatabase(deviceId);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenDeviceAlreadyExsistsInDatabase_ShouldReturnNoContent(List<string> devices)
    {
        // arrange`
        var request = new PutDeviceRequest() { DeviceId = devices[0], DeviceName = $"New-UnitTest-{devices[0]}" };
        var devicesEntity = devices.Select(x => new AccessControlDevice()
        {
            DeviceName = x,
            Description = $"UnitTest-{x}",
        }).ToList();

        await Context.WithDevice(devicesEntity);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        response.Error.Should().BeNull();
    }
}
