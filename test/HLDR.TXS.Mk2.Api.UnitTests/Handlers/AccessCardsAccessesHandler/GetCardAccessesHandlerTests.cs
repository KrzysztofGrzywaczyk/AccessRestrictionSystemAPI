
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models;
using AccessControlSystem.Api.Models.DataTransferObjects;
using AccessControlSystem.Api.Models.Requests.AccessCardAccesses;
using AccessControlSystem.Api.UnitTests.Handlers.SlotsHandler.Contexts;
using AccessControlSystem.SharedKernel.ApiModels;
using AccessControlSystem.UnitTests;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.AccessCardAccessesHandler;

public class GetCardAccessesHandlerTests : UnitTestsBase<GetCardAccessesHandlerTestsContext>
{
    [Theory]
    [AutoSubstituteData]
    public async Task WhenDeviceWithSlotsAndAccessCardWithAccessesExistsInDatabase_ShouldReturnAccessCardWithAccesses(string accessCardValue, string deviceId, string testSlotName)
    {
        // arrange
        var accessCard = new AccessCard() { Value = accessCardValue };
        var request = new GetCardAccessesRequest() { AccessCardValue = accessCard.Value };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest",
            Slots = new List<Slot>() { new Slot() { SlotName = testSlotName, SlotNumber = 1 } }
        };

        await Context.WithDeviceAndSlotCreated(device);
        await Context.WithAccessCardWithAccessCreated(device, accessCard);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        response.Error.Should().BeNull();
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<AccessCardAccessesDto>();
        response.Data!.SlotAccesses.Should().NotBeNullOrEmpty();
        response.Data.SlotAccesses.Should().BeOfType<List<SlotAccesses>>();
        response.Data.SlotAccesses!.First().Slots.Should().NotBeNull();
        response.Data.SlotAccesses!.First().Slots!.First().SlotName.Should().Be(testSlotName);
    }

    [Fact]
    public async Task WhenAccessCardValueNotProvided_ShouldReturnNotFound()
    {
        // arrange
        var request = new GetCardAccessesRequest();

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
    public async Task WhenProvidedWrongAccessCardValue_ShouldReturnNotFound(string accessCardValue, string deviceIdwayId, string wrongDeviceId)
    {
        // arrange
        var accessCard = new AccessCard() { Value = accessCardValue };
        var request = new GetCardAccessesRequest() { DeviceId = wrongDeviceId };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceIdwayId,
            Description = $"UnitTest",
            Slots = new List<Slot>() { new Slot() { SlotName = $"UnitTest-Device", SlotNumber = 1 } }
        };

        await Context.WithDeviceAndSlotCreated(device);
        await Context.WithAccessCardWithAccessCreated(device, accessCard);

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
    public async Task WhenSlotNamesProvided_ShouldReturnAcessesOfSlotsWithProvidedNames(string accessCardValue, string deviceId, string testSlotName)
    {
        // arrange
        var accessCard = new AccessCard() { Value = accessCardValue };
        var request = new GetCardAccessesRequest() { AccessCardValue = accessCard.Value, DeviceId = deviceId, SlotNames = new[] { testSlotName } };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest",
            Slots = new List<Slot>() { new Slot() { SlotName = testSlotName, SlotNumber = 1 } }
        };

        await Context.WithDeviceAndSlotCreated(device);
        await Context.WithAccessCardWithAccessCreated(device, accessCard);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        response.Error.Should().BeNull();
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<AccessCardAccessesDto>();
        response.Data!.SlotAccesses.Should().NotBeNullOrEmpty();
        response.Data.SlotAccesses.Should().BeOfType<List<SlotAccesses>>();
        response.Data.SlotAccesses!.First().Slots.Should().NotBeNull();
        response.Data.SlotAccesses!.First().Slots!.First().SlotName.Should().Be(testSlotName);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenSlotslotNumbersProvided_ShouldRemoveSlotsWithProvidedSlotNumbers(string accessCardValue, string deviceId, string testSlotName)
    {
        // arrange
        var accessCard = new AccessCard() { Value = accessCardValue };
        var testSlotNumber = SlotNumbersEnum.Value1;
        var request = new GetCardAccessesRequest() { AccessCardValue = accessCard.Value, DeviceId = deviceId, SlotNumbers = new[] { testSlotNumber } };
        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest",
            Slots = new List<Slot>() { new Slot() { SlotName = testSlotName, SlotNumber = (int)testSlotNumber } }
        };

        await Context.WithDeviceAndSlotCreated(device);
        await Context.WithAccessCardWithAccessCreated(device, accessCard);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        response.Error.Should().BeNull();
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<AccessCardAccessesDto>();
        response.Data!.SlotAccesses.Should().NotBeNullOrEmpty();
        response.Data.SlotAccesses.Should().BeOfType<List<SlotAccesses>>();
        response.Data.SlotAccesses!.First().Slots.Should().NotBeNull();
        response.Data.SlotAccesses!.First().Slots!.First().SlotNumber.Should().Be((int)testSlotNumber);
    }
}
