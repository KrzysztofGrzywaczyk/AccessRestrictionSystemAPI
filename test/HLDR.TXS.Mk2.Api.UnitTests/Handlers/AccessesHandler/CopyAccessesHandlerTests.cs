
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models.Requests.Accesses;
using AccessControlSystem.Api.UnitTests.Handlers.AccessesHandler.Contexts;
using AccessControlSystem.SharedKernel.ApiModels;
using AccessControlSystem.UnitTests;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.AccessesHandler;

public class CopyAccessesHandlerTests : UnitTestsBase<CopyAccessesHandlerTestsContext>
{
    [Theory]
    [AutoSubstituteData]
    public async Task WhenSourceAccessesAndTargetDeviceExsists_ShouldMovedAccessesToTargetDevice(string sourceDeviceId, string targetDeviceId, string accessCardValue)
    {
        // arrange
        var accessCard = new AccessCard() { Value = accessCardValue };

        var request = new CopyAccessesRequest() { SourceDeviceId = sourceDeviceId, TargetDeviceId = targetDeviceId };
        var sourceDevice = new AccessControlDevice()
        {
            DeviceName = sourceDeviceId,
            Description = $"Source-UnitTest-Device",
            Slots = new List<Slot>() { new Slot() { SlotName = $"UnitTest-Slot", SlotNumber = 1 } }
        };
        var targetDevice = new AccessControlDevice()
        {
            DeviceName = targetDeviceId,
            Description = $"Target-UnitTest-Device",
        };

        await Context.WithDeviceAndSlotCreated(sourceDevice);
        await Context.WithDeviceAndSlotCreated(targetDevice);
        await Context.WithAccessCardCreated(accessCard);
        await Context.WithAccessCreated(sourceDevice, accessCard);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        response.Error.Should().BeNull();
        await Context.TargetAccessShouldBeCreatedInDatabase(targetDevice, accessCard);
        await Context.SourceAccessesShouldStillExistInDatabase(sourceDevice, accessCard);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenThereAreNoAccessesToCopyOnSourceSystem_ShouldReturnNotFound(string sourceDeviceId, string targetDeviceId)
    {
        // arrange
        var request = new CopyAccessesRequest() { SourceDeviceId = sourceDeviceId, TargetDeviceId = targetDeviceId };
        var sourceDevice = new AccessControlDevice()
        {
            DeviceName = sourceDeviceId,
            Description = $"Source-UnitTest-Device",
            Slots = new List<Slot>() { new Slot() { SlotName = $"UnitTest-Slot", SlotNumber = 1 } }
        };
        var targetDevice = new AccessControlDevice()
        {
            DeviceName = targetDeviceId,
            Description = $"Target-UnitTest-Device"
        };

        await Context.WithDeviceAndSlotCreated(sourceDevice);
        await Context.WithDeviceAndSlotCreated(targetDevice);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeFalse();
        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        response.Error?.Type.Should().Be(ErrorType.ResourceNotFound);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenProvidedWrongSourceDevice_ShouldReturnNotFound(string sourceDeviceId, string targetDeviceId, string wrongDeviceId, string accessCardValue)
    {
        // arrange
        var accessCard = new AccessCard() { Value = accessCardValue };

        var request = new CopyAccessesRequest() { SourceDeviceId = wrongDeviceId, TargetDeviceId = targetDeviceId };
        var sourceDevice = new AccessControlDevice()
        {
            DeviceName = sourceDeviceId,
            Description = $"Source-UnitTest-Device",
            Slots = new List<Slot>() { new Slot() { SlotName = $"UnitTest-Slot", SlotNumber = 1 } }
        };
        var targetDevice = new AccessControlDevice()
        {
            DeviceName = targetDeviceId,
            Description = $"Target-UnitTest-Device"
        };

        await Context.WithDeviceAndSlotCreated(sourceDevice);
        await Context.WithDeviceAndSlotCreated(targetDevice);
        await Context.WithAccessCardCreated(accessCard);
        await Context.WithAccessCreated(sourceDevice, accessCard);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeFalse();
        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        response.Error?.Type.Should().Be(ErrorType.ResourceNotFound);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenProvidedWrongTargetDevice_ShouldReturnNotFound(string sourceDeviceId, string targetDeviceId, string wrongDeviceId, string accessCardValue)
    {
        // arrange
        var accessCard = new AccessCard() { Value = accessCardValue };

        var request = new CopyAccessesRequest() { SourceDeviceId = sourceDeviceId, TargetDeviceId = wrongDeviceId };
        var sourceDevice = new AccessControlDevice()
        {
            DeviceName = sourceDeviceId,
            Description = $"Source-UnitTest-Device",
            Slots = new List<Slot>() { new Slot() { SlotName = $"UnitTest-Slot", SlotNumber = 1 } }
        };
        var targetDevice = new AccessControlDevice()
        {
            DeviceName = targetDeviceId,
            Description = $"Target-UnitTest-Device"
        };

        await Context.WithDeviceAndSlotCreated(sourceDevice);
        await Context.WithDeviceAndSlotCreated(targetDevice);
        await Context.WithAccessCardCreated(accessCard);
        await Context.WithAccessCreated(sourceDevice, accessCard);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeFalse();
        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        response.Error?.Type.Should().Be(ErrorType.ResourceNotFound);
    }
}
