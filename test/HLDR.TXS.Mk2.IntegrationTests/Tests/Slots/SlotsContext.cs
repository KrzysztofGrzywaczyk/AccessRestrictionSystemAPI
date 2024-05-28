using AccessControlSystem.Api.Models.DataTransferObjects;
using AccessControlSystem.Api.Models.Requests.AccessControlSystems;
using AccessControlSystem.Api.Models.Requests.Slots;
using AccessControlSystem.Api.Models.Responses.Slots;
using AccessControlSystem.IntegrationTests.Components;
using AccessControlSystem.IntegrationTests.Core.Configuration;
using AccessControlSystem.IntegrationTests.Core.Logging;
using AccessControlSystem.SharedKernel.JsonSerialization;
using System.Net;
using System.Text;
using System.Text.Json;

namespace AccessControlSystem.IntegrationTests.Tests.Slot;

public class SlotsContext(AssemblySharedFixture assemblySharedFixture, Logger logger) : BaseContext(assemblySharedFixture, logger)
{
    private readonly string _testDeviceId = $"IntegrationTests-Device-{Guid.NewGuid().ToString("N").Substring(0, 16)}";

    private readonly string _testSlotName = $"IntegrationTests-Slot-{Guid.NewGuid().ToString("N").Substring(0, 16)}";

    private readonly int _testSlotslotNumber = 1;

    private readonly SlotsApiClient _apiClient = new(
        assemblySharedFixture.HttpClient,
        assemblySharedFixture.Configuration.GetProperty(ConfigKey.TestApiAddress),
        assemblySharedFixture.Configuration.GetProperty(ConfigKey.ApiKey));

    private readonly DevicesApiClient _deviceApiClient = new(
        assemblySharedFixture.HttpClient,
        assemblySharedFixture.Configuration.GetProperty(ConfigKey.TestApiAddress),
        assemblySharedFixture.Configuration.GetProperty(ConfigKey.ApiKey));

    public override async Task InitializeAsync()
    {
        await WithDeviceCreated();
        await WhenPutSlotQueried();
        Logger.Info(this, "Initialized");
    }

    public override async Task DisposeAsync()
    {
        Logger.Info(this, "Dispose :");
        await WhenDeleteSlotQueried();
        await FinallyDeviceIsDeleted();
    }

    public async Task WhenGetSlotsQueried(bool withApiKey = true)
    {
        Logger.Info(this, "GET devices sent ...");
        LastApiResponse = await _apiClient.GetSlots(_testDeviceId, withApiKey);

        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenGetSlotsQueriedWithNameFilter(bool withApiKey = true)
    {
        var slotNames = new List<string>() { _testSlotName };

        Logger.Info(this, "GET slots with {0} slotNames filter sent ...", slotNames.Count);
        LastApiResponse = await _apiClient.GetSlots(_testDeviceId, slotNames, withApiKey);

        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenGetSlotsQueriedWithSlotNumberFilter(bool withApiKey = true)
    {
        var slotNumbers = new List<int>() { _testSlotslotNumber };

        Logger.Info(this, "GET slots with {0} slotNumbers filter sent ...", slotNumbers.Count);
        LastApiResponse = await _apiClient.GetSlots(_testDeviceId, slotNumbers, withApiKey);

        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenPutSlotQueried(bool withApiKey = true)
    {
        Logger.Info(this, $"PUT slot sent...");

        var content = SerializeCorrectPutSlotRequest();

        LastApiResponse = await _apiClient.PutSlots(_testDeviceId, content, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenDeleteSlotQueried(bool withApiKey = true)
    {
        Logger.Info(this, $"DELETE slot sent...");

        LastApiResponse = await _apiClient.DeleteSlots(_testDeviceId, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WithDeviceCreated(bool withApiKey = true)
    {
        Logger.Info(this, $"PUT devices sent...");

        var content = SerializeCorrectPutDeviceRequest();

        LastApiResponse = await _deviceApiClient.PutAccessControlDevice(_testDeviceId, content, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task FinallyDeviceIsDeleted(bool withApiKey = true)
    {
        Logger.Info(this, $"Delete devices sent...");

        LastApiResponse = await _deviceApiClient.DeleteAccessControlDevice(_testDeviceId, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task ThenListOfSlotsShouldBeReturned()
    {
        await ThenStatusCodeIsReturned(HttpStatusCode.OK);

        var content = await LastApiResponse!.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<GetSlotsInDeviceResponse>(content, JsonSerializerOptionsProvider.Default);

        responseContent.Should().NotBeNull();
        responseContent!.Data?.Should().NotBeNull();

        var responseDevice = responseContent.Data;

        responseDevice!.Should().BeOfType<SlotAccesses>();
        responseDevice!.Slots.Should().NotBeNullOrEmpty();
        responseDevice.Slots!.First().Should().NotBeNull();
        responseDevice.Slots!.First().Should().BeOfType<GetSlotsDto>();
        responseDevice.Slots!.Count.Should().BeGreaterThanOrEqualTo(1);
    }

    public async Task ThenFilteredSlotsAreReturned()
    {
        await ThenStatusCodeIsReturned(HttpStatusCode.OK);

        var content = await LastApiResponse!.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<GetSlotsInDeviceResponse>(content, JsonSerializerOptionsProvider.Default);

        responseContent.Should().NotBeNull();
        responseContent!.Data?.Should().NotBeNull();

        var responseDevice = responseContent.Data;

        responseDevice!.Should().BeOfType<SlotAccesses>();
        responseDevice!.Slots.Should().NotBeNullOrEmpty();
        responseDevice.Slots!.First().Should().NotBeNull();
        responseDevice.Slots!.First().Should().BeOfType<GetSlotsDto>();
        responseDevice.Slots!.Count.Should().BeGreaterThanOrEqualTo(1);
        responseDevice.Slots!.First(s => s.SlotName == _testSlotName).Should().NotBeNull();
        responseDevice.Slots!.First(s => s.SlotName == _testSlotName).Should().BeOfType<GetSlotsDto>();
        responseDevice.Slots!.First(s => s.SlotNumber == _testSlotslotNumber).Should().NotBeNull();
        responseDevice.Slots!.First(s => s.SlotNumber == _testSlotslotNumber).Should().BeOfType<GetSlotsDto>();
    }

    private static StringContent SerializeRequest(object request)
    {
        var jsonRequest = JsonSerializer.Serialize(request, JsonSerializerOptionsProvider.Default);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        return content;
    }

    private StringContent SerializeCorrectPutDeviceRequest()
    {
        var correctPutRequest = new PutDeviceRequest()
        {
            DeviceName = Guid.NewGuid().ToString("N").Substring(0, 16)
        };

        return SerializeRequest(correctPutRequest);
    }

    private StringContent SerializeCorrectPutSlotRequest()
    {
        var correctPutRequest = new PutSlotsInDeviceRequest()
        {
            Slots = new List<SimplifiedSlot>
            {
                new SimplifiedSlot
                {
                    SlotName = _testSlotName,
                    SlotNumber = _testSlotslotNumber
                }
            }
        };

        return SerializeRequest(correctPutRequest);
    }
}
