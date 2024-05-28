
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models;
using AccessControlSystem.Api.Models.Requests.Accesses;
using AccessControlSystem.Api.UnitTests.Handlers.AccessesHandler.Contexts;
using AccessControlSystem.SharedKernel.ApiModels;
using AccessControlSystem.UnitTests;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.AccessesHandler;

public class RemoveAccessesHandlerTests : UnitTestsBase<RemoveAccessesHandlerTestsContext>
{
    [Theory]
    [AutoSubstituteData]
    public async Task WhenAccessesExist_ShouldRemovedAllAccessesInDevice(string deviceId, string accessCardValue)
    {
        // arrange
        var accessCard = new AccessCard() { Value = accessCardValue };

        var request = new RemoveAccessesRequest() { DeviceId = deviceId };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest",
            Slots = new List<Slot>() { new Slot() { SlotName = $"UnitTest-Slot", SlotNumber = 1 } }
        };

        await Context.WithDeviceAndSlotCreated(device);
        await Context.WithAccessCardCreated(accessCard);
        await Context.WithAccessCreated(device, accessCard);

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
    public async Task WhenProvidedWrongDevice_ShouldReturnNotFound(string deviceId, string wrongDeviceId, string accessCardValue)
    {
        // arrange
        var accessCard = new AccessCard() { Value = accessCardValue };

        var request = new RemoveAccessesRequest() { DeviceId = wrongDeviceId };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest",
            Slots = new List<Slot>() { new Slot() { SlotName = $"UnitTest-Slot", SlotNumber = 1 } }
        };

        await Context.WithDeviceAndSlotCreated(device);
        await Context.WithAccessCardCreated(accessCard);
        await Context.WithAccessCreated(device, accessCard);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeFalse();
        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        response.Error?.Type.Should().Be(ErrorType.ResourceNotFound);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenSlotNamesProvided_ShouldRemoveAllAccessesWithProvidedSlotNames(string deviceId, string testSlotName, string accessCardValue)
    {
        // arrange
        var accessCard = new AccessCard() { Value = accessCardValue };

        var request = new RemoveAccessesRequest() { DeviceId = deviceId, SlotNames = new[] { testSlotName } };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest",
            Slots = new List<Slot>()
            {
                new Slot() { SlotName = testSlotName, SlotNumber = 1 }, new Slot()
                {
                    SlotName = $"UnitTest-SecondSlot" +
                    $"", SlotNumber = 2
                }
            }
        };

        await Context.WithDeviceAndSlotCreated(device);
        await Context.WithAccessCardCreated(accessCard);
        await Context.WithAccessCreated(device, accessCard);

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

        var request = new RemoveAccessesRequest() { DeviceId = deviceId, SlotNumbers = new[] { testSlotNumber } };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest",
            Slots = new List<Slot>() { new Slot() { SlotName = $"UnitTest-Slot", SlotNumber = (int)testSlotNumber }, new Slot() { SlotName = $"UnitTest-Second-Slot", SlotNumber = 2 } }
        };

        await Context.WithDeviceAndSlotCreated(device);
        await Context.WithAccessCardCreated(accessCard);
        await Context.WithAccessCreated(device, accessCard);

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
    public async Task WhenAccessCardValuesProvided_ShouldRemoveAccessesWithProvidedAccessCardValues(string deviceId, string accessCardValue, string accessCardValueToLeave)
    {
        // arrange
        var accessCard = new AccessCard() { Value = accessCardValue };
        var accessCardToLeave = new AccessCard() { Value = accessCardValueToLeave };

        var request = new RemoveAccessesRequest() { DeviceId = deviceId, AccessCardValues = new[] { accessCardValue } };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest",
            Slots = new List<Slot>() { new Slot() { SlotName = $"UnitTest-Slot", SlotNumber = 1 } }
        };

        await Context.WithDeviceAndSlotCreated(device);
        await Context.WithAccessCardCreated(accessCard);
        await Context.WithAccessCardCreated(accessCardToLeave);
        await Context.WithAccessCreated(device, accessCard);
        await Context.WithAccessCreated(device, accessCardToLeave);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        response.Error.Should().BeNull();
        await Context.AccessesShouldNotExistInDatabase(device, accessCard);
        await Context.AccessToLeaveShouldStillExistsInDatabase(device, accessCardToLeave);
    }
}
