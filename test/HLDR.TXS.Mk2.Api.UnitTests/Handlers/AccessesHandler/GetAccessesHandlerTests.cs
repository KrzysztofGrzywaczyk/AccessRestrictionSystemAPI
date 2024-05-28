
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models;
using AccessControlSystem.Api.Models.Requests.Accesses;
using AccessControlSystem.Api.UnitTests.Handlers.AccessesHandler.Contexts;
using AccessControlSystem.SharedKernel.ApiModels;
using AccessControlSystem.UnitTests;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.AccessesHandler;

public class GetAccessesHandlerTests : UnitTestsBase<GetAccessesHandlerTestsContext>
{
    [Theory]
    [AutoSubstituteData]
    public async Task WhenAccessesExistsInDatabase_ShouldReturnAccessesOnGivenDevice(string deviceId, string deviceDescription, string testSlotName, string accessCardValue)
    {
        // arrange
        var accessCard = new AccessCard() { Value = accessCardValue };
        var request = new GetAccessesRequest() { DeviceId = deviceId };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = deviceDescription,
            Slots = new List<Slot>() { new Slot() { SlotName = testSlotName, SlotNumber = 1 } }
        };

        await Context.WithDeviceAndSlotCreated(device);
        await Context.WithAccessCardCreated(accessCard);
        await Context.WithAccessesCreated(device, accessCard);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        response.Error.Should().BeNull();
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<AccessControlDevice>();
        response.Data!.DeviceName.Should().Be(deviceId);
        response.Data.Description.Should().Be(deviceDescription);
        response.Data.Slots.Should().NotBeNullOrEmpty();
        response.Data.Slots.Should().BeOfType<List<Slot>>();
        response.Data.Slots!.FirstOrDefault().Should().NotBeNull();
        response.Data.Slots!.FirstOrDefault()!.AccessCards.Should().NotBeNullOrEmpty();
        response.Data.Slots!.FirstOrDefault()!.
            AccessCards!.FirstOrDefault().Should().NotBeNull();
        response.Data.Slots!.FirstOrDefault()!.
            AccessCards!.FirstOrDefault()!.Value.Should().Be(accessCardValue);
    }

    [Fact]
    public async Task WhenDeviceIdNotProvided_ShouldReturnNotFound()
    {
        // arrange
        var request = new GetAccessesRequest();

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
    public async Task WhenProvidedWrongDeviceId_ShouldReturnNotFound(string deviceId, string wrongDeviceId, string accessCardValue)
    {
        // arrange
        var accessCard = new AccessCard() { Value = accessCardValue };
        var request = new GetAccessesRequest() { DeviceId = wrongDeviceId };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest",
            Slots = new List<Slot>() { new Slot() { SlotName = $"UnitTest-Device", SlotNumber = 1 } }
        };

        await Context.WithDeviceAndSlotCreated(device);
        await Context.WithAccessCardCreated(accessCard);
        await Context.WithAccessesCreated(device, accessCard);

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
    public async Task WhenSlotNamesProvided_ShouldReturnAcessesOfSlotsWithProvidedNames(string deviceId, string deviceDescription, string testSlotName, string accessCardValue)
    {
        // arrange
        var accessCard = new AccessCard() { Value = accessCardValue };
        var request = new GetAccessesRequest() { DeviceId = deviceId, SlotNames = new[] { testSlotName } };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = deviceDescription,
            Slots = new List<Slot>() { new Slot() { SlotName = testSlotName, SlotNumber = 1 } }
        };

        await Context.WithDeviceAndSlotCreated(device);
        await Context.WithAccessCardCreated(accessCard);
        await Context.WithAccessesCreated(device, accessCard);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        response.Error.Should().BeNull();
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<AccessControlDevice>();
        response.Data!.DeviceName.Should().Be(deviceId);
        response.Data.Description.Should().Be(deviceDescription);
        response.Data.Slots.Should().NotBeNullOrEmpty();
        response.Data.Slots.Should().BeOfType<List<Slot>>();
        response.Data.Slots!.FirstOrDefault().Should().NotBeNull();
        response.Data.Slots!.FirstOrDefault()!.AccessCards.Should().NotBeNullOrEmpty();
        response.Data.Slots!.FirstOrDefault()!.
            AccessCards!.FirstOrDefault().Should().NotBeNull();
        response.Data.Slots!.FirstOrDefault()!.
            AccessCards!.FirstOrDefault()!.Value.Should().Be(accessCardValue);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenSlotslotNumbersProvided_ShouldRemoveSlotsWithProvidedSlotNumbers(string deviceId, string deviceDescription, string testSlotName, string accessCardValue)
    {
        // arrange
        var accessCard = new AccessCard() { Value = accessCardValue };
        var testSlotNumber = SlotNumbersEnum.Value1;
        var request = new GetAccessesRequest() { DeviceId = deviceId, SlotNumbers = new[] { testSlotNumber } };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = deviceDescription,
            Slots = new List<Slot>() { new Slot() { SlotName = testSlotName, SlotNumber = (int)testSlotNumber } }
        };

        await Context.WithDeviceAndSlotCreated(device);
        await Context.WithAccessCardCreated(accessCard);
        await Context.WithAccessesCreated(device, accessCard);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        response.Error.Should().BeNull();
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<AccessControlDevice>();
        response.Data!.DeviceName.Should().Be(deviceId);
        response.Data.Description.Should().Be(deviceDescription);
        response.Data.Slots.Should().NotBeNullOrEmpty();
        response.Data.Slots.Should().BeOfType<List<Slot>>();
        response.Data.Slots!.FirstOrDefault().Should().NotBeNull();
        response.Data.Slots!.FirstOrDefault()!.AccessCards.Should().NotBeNullOrEmpty();
        response.Data.Slots!.FirstOrDefault()!.
            AccessCards!.FirstOrDefault().Should().NotBeNull();
        response.Data.Slots!.FirstOrDefault()!.
            AccessCards!.FirstOrDefault()!.Value.Should().Be(accessCardValue);
    }
}
