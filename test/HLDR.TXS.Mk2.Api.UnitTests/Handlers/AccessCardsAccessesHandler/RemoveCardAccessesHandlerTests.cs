
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models;
using AccessControlSystem.Api.Models.Requests.AccessCardAccesses;
using AccessControlSystem.Api.UnitTests.Handlers.SlotsHandler.Contexts;
using AccessControlSystem.SharedKernel.ApiModels;
using AccessControlSystem.UnitTests;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.AccessCardAccessesHandler;

public class RemoveCardAccessesHandlerTests : UnitTestsBase<RemoveCardAccessesHandlerTestsContext>
{
    [Theory]
    [AutoSubstituteData]
    public async Task WhenAccessesExist_ShouldRemovedAccesses(string deviceId, string accessCardValue)
    {
        // arrange
        var accessCard = new AccessCard() { Value = accessCardValue };
        var testSlots = new List<Slot>() { new Slot() { SlotName = $"UnitTest-Slot", SlotNumber = 1 } };

        var request = new RemoveCardAccessesRequest() { AccessCardValue = accessCardValue };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest",
            Slots = testSlots
        };

        await Context.WithDeviceWithSlotCreated(device);
        await Context.WithAccessCardWithAccessCreated(device, accessCard);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        response.Error.Should().BeNull();
        await Context.AccessesShouldNotExistInDatabase(device, accessCard);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenProvidedWrongAccessCardValue_ShouldReturnNotFound(string deviceId, string wrongAccessCardValue, string accessCardValue)
    {
        // arrange
        var accessCard = new AccessCard() { Value = accessCardValue };
        var testSlots = new List<Slot>() { new Slot() { SlotName = $"UnitTest-Slot", SlotNumber = 1 } };

        var request = new RemoveCardAccessesRequest() { AccessCardValue = wrongAccessCardValue };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest",
            Slots = testSlots
        };

        await Context.WithDeviceWithSlotCreated(device);
        await Context.WithAccessCardWithAccessCreated(device, accessCard);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeFalse();
        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        response.Error?.Type.Should().Be(ErrorType.ResourceNotFound);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenProvidedWrongDevice_ShouldReturnNotFound(string deviceId, string wrongDeviceId, string accessCardValue)
    {
        // arrange
        var accessCard = new AccessCard() { Value = accessCardValue };
        var testSlots = new List<Slot>() { new Slot() { SlotName = $"UnitTest-Slot", SlotNumber = 1 } };

        var request = new RemoveCardAccessesRequest() { AccessCardValue = accessCardValue, DeviceId = wrongDeviceId };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest",
            Slots = testSlots
        };

        await Context.WithDeviceWithSlotCreated(device);
        await Context.WithAccessCardWithAccessCreated(device, accessCard);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeFalse();
        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        response.Error?.Type.Should().Be(ErrorType.ResourceNotFound);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenSlotNamesProvided_ShouldRemoveAccessesWithProvidedSlotNames(string deviceId, string testSlotName, string accessCardValue)
    {
        // arrange
        var accessCard = new AccessCard() { Value = accessCardValue };

        var request = new RemoveCardAccessesRequest() { AccessCardValue = accessCardValue, DeviceId = deviceId, SlotNames = new[] { testSlotName } };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest",
            Slots = new List<Slot>() { new Slot() { SlotName = testSlotName, SlotNumber = 1 }, new Slot() { SlotName = $"UnitTest-SecondSlot", SlotNumber = 2 } }
        };

        await Context.WithDeviceWithSlotCreated(device);
        await Context.WithAccessCardWithAccessCreated(device, accessCard);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        response.Error.Should().BeNull();
        await Context.AccessesShouldNotExistInDatabase(device, accessCard);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenSlotslotNumbersProvided_ShouldRemoveAccessesWithProvidedSlotslotNumbers(string deviceId, string accessCardValue)
    {
        // arrange
        var testSlotNumber = SlotNumbersEnum.Value1;
        var accessCard = new AccessCard() { Value = accessCardValue };

        var request = new RemoveCardAccessesRequest() { AccessCardValue = accessCardValue, DeviceId = deviceId, SlotNumbers = new[] { testSlotNumber } };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest",
            Slots = new List<Slot>() { new Slot() { SlotName = $"UnitTest-Slot", SlotNumber = (int)testSlotNumber }, new Slot() { SlotName = $"UnitTest-SecondSlot", SlotNumber = 2 } }
        };

        await Context.WithDeviceWithSlotCreated(device);
        await Context.WithAccessCardWithAccessCreated(device, accessCard);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        response.Error.Should().BeNull();
        await Context.AccessesShouldNotExistInDatabase(device, accessCard);
    }
}
