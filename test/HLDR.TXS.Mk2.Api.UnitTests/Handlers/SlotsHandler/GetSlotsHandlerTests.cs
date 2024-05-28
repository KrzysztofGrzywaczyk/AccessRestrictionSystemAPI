
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models;
using AccessControlSystem.Api.Models.DataTransferObjects;
using AccessControlSystem.Api.Models.Requests.Slots;
using AccessControlSystem.Api.UnitTests.Handlers.SlotsHandler.Contexts;
using AccessControlSystem.SharedKernel.ApiModels;
using AccessControlSystem.UnitTests;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.SlotsHandler;

public class GetSlotsHandlerTests : UnitTestsBase<GetSlotsHandlerTestsContext>
{
    [Theory]
    [AutoSubstituteData]
    public async Task WhenAccessControlSystemWithSlotsExistsInDatabase_ShouldReturnDeviceWithSlots(string deviceName, string testSlotName)
    {
        // arrange
        var request = new GetSlotsInDeviceRequest() { DeviceId = deviceName };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceName,
            Description = $"UnitTest",
            Slots = new List<Slot>() { new Slot() { SlotName = testSlotName, SlotNumber = 1 } }
        };

        await Context.WithDeviceAndSlotCreated(device);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        response.Error.Should().BeNull();
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<SlotAccesses>();
        response.Data!.Slots!.Count().Should().Be(1);
        response.Data!.Slots!.First().SlotName.Should().Be(testSlotName);
    }

    [Fact]
    public async Task WhenNoSlotsExistsCorrespondingToGivenDevice_ShouldReturnSlotsNotFound()
    {
        // arrange
        var request = new GetSlotsInDeviceRequest();

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeFalse();
        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        response.Error.Should().NotBeNull();
        response.Error!.Type.Should().Be(ErrorType.ResourceNotFound);
        response.Data.Should().BeNull();
    }

    [Fact]
    public async Task WhenNotProvidedDevice_ShouldReturnNotFound()
    {
        // arrange
        var request = new GetSlotsInDeviceRequest();

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeFalse();
        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        response.Error.Should().NotBeNull();
        response.Data.Should().BeNull();
        response.Error?.Type.Should().Be(ErrorType.ResourceNotFound);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenProvidedWrongDevice_ShouldReturnNotFound(string deviceId, string wrongDeviceId)
    {
        // arrange
        var request = new GetSlotsInDeviceRequest() { DeviceId = wrongDeviceId };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest",
            Slots = new List<Slot>() { new Slot() { SlotName = $"UnitTest-Device", SlotNumber = 1 } }
        };

        await Context.WithDeviceAndSlotCreated(device);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeFalse();
        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        response.Error.Should().NotBeNull();
        response.Error!.Type.Should().Be(ErrorType.ResourceNotFound);
        response.Data.Should().BeNull();
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenSlotNamesProvided_ShouldRemoveSlotsWithProvidedNames(string deviceId, string testSlotName)
    {
        // arrange
        var request = new GetSlotsInDeviceRequest() { DeviceId = deviceId, SlotNames = new[] { testSlotName } };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest",
            Slots = new List<Slot>() { new Slot() { SlotName = testSlotName, SlotNumber = 1 }, new Slot() { SlotName = $"UnitTest-SecondSlot", SlotNumber = 2 } }
        };

        await Context.WithDeviceAndSlotCreated(device);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        response.Error.Should().BeNull();
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<SlotAccesses>();
        response.Data!.Slots.Should().NotBeNullOrEmpty();
        response.Data!.Slots.Should().Contain(s => s.SlotName == testSlotName);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenSlotslotNumbersProvided_ShouldRemoveSlotsWithProvidedSlotNumbers(string deviceId)
    {
        // arrange
        var testSlotNumber = SlotNumbersEnum.Value1;
        var request = new GetSlotsInDeviceRequest() { DeviceId = deviceId, SlotNumbers = new[] { testSlotNumber } };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest",
            Slots = new List<Slot>() { new Slot() { SlotName = $"UnitTest-Slot", SlotNumber = (int)testSlotNumber }, new Slot() { SlotName = $"UnitTest-Second-Slot", SlotNumber = 2 } }
        };

        await Context.WithDeviceAndSlotCreated(device);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        response.Error.Should().BeNull();
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<SlotAccesses>();
        response.Data!.Slots.Should().NotBeNullOrEmpty();
        response.Data!.Slots.Should().Contain(s => s.SlotNumber == (int)testSlotNumber);
    }
}
