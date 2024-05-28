
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models.Requests.AccessControlSystems;
using AccessControlSystem.Api.UnitTests.Handlers.AccessControlSystemsHandler.Contexts;
using AccessControlSystem.SharedKernel.ApiModels;
using AccessControlSystem.UnitTests;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.AccessControlSystems;

public class RemoveDeviceHandlerTests : UnitTestsBase<RemoveDeviceHandlerTestsContext>
{
    [Theory]
    [AutoSubstituteData]
    public async Task WhenDeviceExists_ShouldRemoveDevice(List<string> deviceIds)
    {
        // arrange
        var request = new RemoveDeviceRequest() { DeviceId = deviceIds[0] };
        var devicesEntity = deviceIds.Select(x => new AccessControlDevice()
        {
            DeviceName = x,
            Description = $"UnitTests-{x}",
        }).ToList();

        await Context.WithAccessControlSystems(devicesEntity);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        response.Error.Should().BeNull();
        await Context.AccessControlSytemShouldNotExistInDatabase(deviceIds[0]);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenAccessControlSystemNotExists_ShouldReturnNotFound(List<string> deviceIds, string wrongDeviceId)
    {
        // arrange
        var request = new RemoveDeviceRequest() { DeviceId = wrongDeviceId };
        var devicesEntity = deviceIds.Select(x => new AccessControlDevice()
        {
            DeviceName = x,
            Description = $"UnitTests-{x}",
        }).ToList();

        await Context.WithAccessControlSystems(devicesEntity);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeFalse();
        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        response.Error.Should().NotBeNull();
        response.Error!.Type.Should().Be(ErrorType.ResourceNotFound);
    }
}
