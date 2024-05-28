
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models.DataTransferObjects;
using AccessControlSystem.Api.Models.Requests.AccessControlSystems;
using AccessControlSystem.Api.UnitTests.Handlers.AccessControlSystemsHandler.Contexts;
using AccessControlSystem.UnitTests;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.AccessControlSystems;

public class GetDeviceHandlerTests : UnitTestsBase<GetDeviceHandlerTestsContext>
{
    [Theory]
    [AutoSubstituteData]
    public async Task WhenAccessControlSysyemsExistsInDatabase_ShouldReturnAllDevices(List<string> deviceIds)
    {
        // arrange
        var devicesEntity = deviceIds.Select(x => new AccessControlDevice()
        {
            DeviceName = x,
            Description = $"UnitTest-{x}",
        }).ToList();

        await Context.WithAccessControlSystem(devicesEntity);

        var request = new GetDeviceRequest();

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        response.Error.Should().BeNull();
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<DevicesList>();
        response.Data!.AccessControlDevices.Should().NotBeNullOrEmpty();
        response.Data.AccessControlDevices!.Should().BeOfType<List<GetDeviceDto>>();
        response.Data.AccessControlDevices!.Count().Should().Be(3);
        response.Data.AccessControlDevices!.First().Description.Should().Be($"UnitTest-{deviceIds[0]}");
    }

    [Fact]
    public async Task WhenAnyAccessControlSystemNotExistInDatabase_ShouldReturnNotFound()
    {
        // arrange
        var request = new GetDeviceRequest();

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeFalse();
        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        response.Error.Should().NotBeNull();
        response.Data.Should().BeNull();
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenProvidedAdditionalDeviceIds_ShouldReturnFilteredDevice(List<string> deviceIds)
    {
        // arrange
        var request = new GetDeviceRequest() { DeviceIds = new string[] { deviceIds[0] } };
        var devicesEntity = deviceIds.Select(x => new AccessControlDevice()
        {
            DeviceName = x,
            Description = $"UnitTest-{x}",
        }).ToList();

        await Context.WithAccessControlSystem(devicesEntity);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        response.Error.Should().BeNull();
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<DevicesList>();
        response.Data!.AccessControlDevices.Should().NotBeNullOrEmpty();
        response.Data.AccessControlDevices!.Should().BeOfType<List<GetDeviceDto>>();
        response.Data.AccessControlDevices!.Count().Should().Be(1);
        response.Data.AccessControlDevices!.First().Description.Should().Be($"UnitTest-{deviceIds[0]}");
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenProvidedWrongAdditionalDeviceIds_ShouldReturnNotFound(List<string> deviceIds, string wrongDeviceId)
    {
        // arrange
        var request = new GetDeviceRequest() { DeviceIds = new string[] { wrongDeviceId } };
        var devicesEntity = deviceIds.Select(x => new AccessControlDevice()
        {
            DeviceName = x,
            Description = $"UnitTest-{x}",
        }).ToList();

        await Context.WithAccessControlSystem(devicesEntity);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeFalse();
        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        response.Error.Should().NotBeNull();
        response.Data.Should().BeNull();
    }
}
