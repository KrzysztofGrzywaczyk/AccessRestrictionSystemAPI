
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models;
using AccessControlSystem.Api.Models.DataTransferObjects;
using AccessControlSystem.Api.Models.Requests.Accesses;
using AccessControlSystem.Api.UnitTests.Handlers.AccessesHandler.Contexts;
using AccessControlSystem.UnitTests;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.AccessesHandler;

public class CreateOrUpdateAccessesHandlerTests : UnitTestsBase<CreateOrUpdateAccessesHandlerTestsContext>
{
    [Theory]
    [AutoSubstituteData]
    public async Task GivenSlotWithoutAccesses_WhenRecieveRequestWithSlotNames_ShouldAddAccessesBySlotName(string accessCardValue, string deviceId, string testSlotName)
    {
        // arrange
        var accessCard = new AccessCard() { Value = accessCardValue };
        var request = new PutAccessesRequest() { DeviceId = deviceId, SlotNames = new[] { testSlotName }, AccessCardValues = new AccessCardValueList() { AccessCardValues = new List<string> { accessCardValue } } };

        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest-Device",
            Slots = new List<Slot>() { new Slot() { SlotName = testSlotName, SlotNumber = 1 } }
        };

        await Context.WithDeviceAndSlotCreated(device);
        await Context.WithAccessCardCreated(accessCard);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        response.Error.Should().BeNull();
        await Context.AccessShouldBeCreatedInDatabase(device, accessCard);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task GivenSlotWithoutAccesses_WhenRecieveRequestWithSlotNumbers_ShouldAddAccessesBySlotNumber(string accessCardValue, string deviceId, string testDeviceName)
    {
        // arrange
        var testSlotNumber = SlotNumbersEnum.Value1;
        var accessCard = new AccessCard() { Value = accessCardValue };
        var request = new PutAccessesRequest() { DeviceId = deviceId, SlotNumbers = new[] { testSlotNumber }, AccessCardValues = new AccessCardValueList() { AccessCardValues = new List<string> { accessCardValue } } };

        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest-Device",
            Slots = new List<Slot>() { new Slot() { SlotName = testDeviceName, SlotNumber = (int)testSlotNumber } }
        };

        await Context.WithDeviceAndSlotCreated(device);
        await Context.WithAccessCardCreated(accessCard);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        response.Error.Should().BeNull();
        await Context.AccessShouldBeCreatedInDatabase(device, accessCard);
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenAccessAlreadyExists_ShouldReturnNoContent(string accessCardValue, string testDeviceName, string deviceId)
    {
        // arrange
        var testSlotNumber = SlotNumbersEnum.Value1;
        var accessCard = new AccessCard() { Value = accessCardValue };
        var request = new PutAccessesRequest() { DeviceId = deviceId, SlotNames = new[] { testDeviceName }, AccessCardValues = new AccessCardValueList() { AccessCardValues = new List<string> { accessCardValue } } };

        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest-Device",
            Slots = new List<Slot>() { new Slot() { SlotName = testDeviceName, SlotNumber = (int)testSlotNumber } }
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
    }
}
