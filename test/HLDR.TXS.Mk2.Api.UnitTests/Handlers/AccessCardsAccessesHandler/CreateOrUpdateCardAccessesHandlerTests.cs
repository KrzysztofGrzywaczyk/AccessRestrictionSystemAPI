
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models;
using AccessControlSystem.Api.Models.DataTransferObjects;
using AccessControlSystem.Api.Models.Requests.AccessCardAccesses;
using AccessControlSystem.Api.UnitTests.Handlers.SlotsHandler.Contexts;
using AccessControlSystem.UnitTests;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.AccessCardAccessesHandler;

public class CreateOrUpdateCardAccessesHandlerTests : UnitTestsBase<CreateOrUpdateCardAccessesHandlerTestsContext>
{
    [Theory]
    [AutoSubstituteData]
    public async Task GivenAccessCardWithoutAccesses_WhenRecieveRequestWithSlotNames_ShouldAddAccessesBySlotName(string accessCardValue, string deviceId, string testSlotName)
    {
        // arrange
        var accessCard = new AccessCard() { Value = accessCardValue };
        var slotAccess = new List<ProperSlot> { new ProperSlot() { SlotId = deviceId, SlotName = testSlotName } };
        var request = new PutCardAccessesRequest() { AccessCardValue = accessCardValue, SlotAccesses = slotAccess };

        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest-Device",
            Slots = new List<Slot>() { new Slot() { SlotName = testSlotName, SlotNumber = 1 } }
        };

        await Context.WithDeviceAndSlotCreated(device);
        await Context.WithAccessCardCreatedInDatabase(accessCard);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        response.Error.Should().BeNull();
        await Context.AccessShouldBeCratedInDatabase(device, accessCard);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task GivenAccessCardWithoutAccesses_WhenRecieveRequestWithSlotslotNumbers_ShouldAddAccessesBySlotslotNumber(string accessCardValue, string deviceId, string testSlotName)
    {
        // arrange
        var testSlotNumber = SlotNumbersEnum.Value1;
        var accessCard = new AccessCard() { Value = accessCardValue };
        var slotAccess = new List<ProperSlot> { new ProperSlot() { SlotId = deviceId, SlotNumber = (int)testSlotNumber } };
        var request = new PutCardAccessesRequest() { AccessCardValue = accessCardValue, SlotAccesses = slotAccess };

        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest-Device",
            Slots = new List<Slot>() { new Slot() { SlotName = testSlotName, SlotNumber = (int)testSlotNumber } }
        };

        await Context.WithDeviceAndSlotCreated(device);
        await Context.WithAccessCardCreatedInDatabase(accessCard);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        response.Error.Should().BeNull();
    }

    [Theory]
    [AutoSubstituteData]
    public async Task GivenAccessCardWithoutAccesses_ShouldReturnNoContent(string accessCardValue, string deviceId, string testSlotName)
    {
        // arrange
        var testSlotNumber = SlotNumbersEnum.Value1;
        var accessCard = new AccessCard() { Value = accessCardValue };
        var slotAccess = new List<ProperSlot> { new ProperSlot() { SlotId = deviceId, SlotNumber = (int)testSlotNumber } };
        var request = new PutCardAccessesRequest() { AccessCardValue = accessCardValue, SlotAccesses = slotAccess };

        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest-Device",
            Slots = new List<Slot>() { new Slot() { SlotName = testSlotName, SlotNumber = (int)testSlotNumber } }
        };

        await Context.WithDeviceAndSlotCreated(device);
        await Context.WithAccessCardWithAccessCreated(device, accessCard);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        response.Error.Should().BeNull();
    }
}
