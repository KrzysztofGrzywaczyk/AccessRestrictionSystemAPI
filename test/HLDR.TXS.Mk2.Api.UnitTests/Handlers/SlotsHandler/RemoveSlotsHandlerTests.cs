
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models;
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

public class RemoveSlotsHandlerTests : UnitTestsBase<RemoveSlotsHandlerTestsContext>
{
    [Theory]
    [AutoSubstituteData]
    public async Task WhenDeviceWithSlotExist_ShouldRemovedDevice(string deviceId)
    {
        // arrange
        var testSlots = new List<Slot>() { new Slot() { SlotName = $"UnitTest-Slot", SlotNumber = 1 } };

        var request = new RemoveSlotsFromDeviceRequest() { DeviceId = deviceId };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest",
            Slots = testSlots
        };

        await Context.WithDeviceWithSlotCreated(device);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        response.Error.Should().BeNull();
        await Context.SlotsShouldNotExistInDatabase(testSlots.Select(s => s.SlotName!).ToList());
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenProvidedWrongDevice_ShouldReturnNotFound(string deviceId, string wrongDeviceId)
    {
        // arrange
        var request = new RemoveSlotsFromDeviceRequest() { DeviceId = wrongDeviceId };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest",
            Slots = new List<Slot>() { new Slot() { SlotName = $"UnitTest-Slot", SlotNumber = 1 } }
        };

        await Context.WithDeviceWithSlotCreated(device);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeFalse();
        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        response.Error?.Type.Should().Be(ErrorType.ResourceNotFound);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenSlotNamesProvided_ShouldRemoveSlotsWithProvidedNames(string deviceId, string testSlotName)
    {
        // arrange
        var request = new RemoveSlotsFromDeviceRequest() { DeviceId = deviceId, SlotNames = new[] { testSlotName } };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest",
            Slots = new List<Slot>() { new Slot() { SlotName = testSlotName, SlotNumber = 1 }, new Slot() { SlotName = $"UnitTest-SecondSlot", SlotNumber = 2 } }
        };

        await Context.WithDeviceWithSlotCreated(device);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        response.Error.Should().BeNull();
        await Context.SlotsShouldNotExistInDatabase(device.Slots.Where(s => s.SlotName == testSlotName).Select(s => s.SlotName!).ToList());
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenSlotslotNumbersProvided_ShouldRemoveSlotsWithProvidedSlotNumbers(string deviceId)
    {
        // arrange
        var testSlotNumber = SlotNumbersEnum.Value1;
        var request = new RemoveSlotsFromDeviceRequest() { DeviceId = deviceId, SlotNumbers = new[] { testSlotNumber } };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest",
            Slots = new List<Slot>() { new Slot() { SlotName = $"UnitTest-Slot", SlotNumber = (int)testSlotNumber }, new Slot() { SlotName = $"UnitTest-Second-Slot", SlotNumber = 2 } }
        };

        await Context.WithDeviceWithSlotCreated(device);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        response.Error.Should().BeNull();
        await Context.SlotsShouldNotExistInDatabase(device.Slots.Where(s => s.SlotNumber == (int)testSlotNumber).Select(s => s.SlotName!).ToList());
    }
}
