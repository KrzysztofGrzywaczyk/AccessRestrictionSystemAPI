
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Models.DataTransferObjects;
using AccessControlSystem.Api.Models.Requests.Slots;
using AccessControlSystem.Api.UnitTests.Handlers.SlotsHandler.Contexts;
using AccessControlSystem.UnitTests;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.SlotsHandler;

public class CreateOrUpdateSlotsHandlerTests : UnitTestsBase<CreateOrUpdateSlotsHandlerTestsContext>
{
    [Theory]
    [AutoSubstituteData]
    public async Task WhenSlotNotExistInDatabase_ShouldAddNewSlot(string testSlotName, string deviceId)
    {
        // arrange
        var request = new PutSlotsInDeviceRequest()
        {
            DeviceId = deviceId,
            Slots = new List<SimplifiedSlot>
                {
                    new SimplifiedSlot() { SlotName = testSlotName, SlotNumber = 1 }
                }
        };

        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest-Device",
        };

        await Context.WithDeviceAndSlot(device);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        response.Error.Should().BeNull();
    }

    [Theory]
    [AutoSubstituteData]
    public async Task WhenSlotAlreadyExists_ShouldReturnNoContent(string testSlotName, string deviceId)
    {
        // arrange
        var request = new PutSlotsInDeviceRequest()
            {
                DeviceId = deviceId,
                Slots = new List<SimplifiedSlot>
                {
                    new SimplifiedSlot() { SlotName = testSlotName, SlotNumber = 1 }
                }
            };

        var device = new AccessControlDevice()
        {
            DeviceName = deviceId,
            Description = $"UnitTest-Device",
            Slots = new List<Slot>() { new Slot() { SlotName = testSlotName, SlotNumber = 1 }, new Slot() { SlotName = $"UnitTest-SecondSlot", SlotNumber = 2 } }
        };

        await Context.WithDeviceAndSlot(device);

        // act
        var response = await Context.Handler.Handle(request, CancellationToken.None);

        // assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        response.Error.Should().BeNull();
    }
}
