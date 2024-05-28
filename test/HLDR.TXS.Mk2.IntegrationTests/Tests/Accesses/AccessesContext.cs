using AccessControlSystem.Api.Models.DataTransferObjects;
using AccessControlSystem.Api.Models.Requests.AccessCards;
using AccessControlSystem.Api.Models.Requests.AccessControlSystems;
using AccessControlSystem.Api.Models.Requests.Slots;
using AccessControlSystem.Api.Models.Responses.Accesses;
using AccessControlSystem.Api.Models.Responses.Slots;
using AccessControlSystem.IntegrationTests.Components;
using AccessControlSystem.IntegrationTests.Core.Configuration;
using AccessControlSystem.IntegrationTests.Core.Logging;
using AccessControlSystem.SharedKernel.JsonSerialization;
using System.Net;
using System.Text;
using System.Text.Json;

namespace AccessControlSystem.IntegrationTests.Tests.Accesses;

public class AccessesContext(AssemblySharedFixture assemblySharedFixture, Logger logger) : BaseContext(assemblySharedFixture, logger)
{
    private readonly string _testAccessCardValue = $"IntegrationTests-AccessCard-{Guid.NewGuid().ToString("N").Substring(0, 16)}";

    private readonly string _testDeviceId = $"IntegrationTests-Device-{Guid.NewGuid().ToString("N").Substring(0, 16)}";

    private readonly string _secondDeviceId = $"IntegrationTests-SecondDevice-{Guid.NewGuid().ToString("N").Substring(0, 16)}";

    private readonly string _testSlotName = $"IntegrationTests-Slot-{Guid.NewGuid().ToString("N").Substring(0, 16)}";

    private readonly int _testSlotslotNumber = 1;

    private readonly DevicesApiClient _deviceApiClient = new(
        assemblySharedFixture.HttpClient,
        assemblySharedFixture.Configuration.GetProperty(ConfigKey.TestApiAddress),
        assemblySharedFixture.Configuration.GetProperty(ConfigKey.ApiKey));

    private readonly AccessesApiClient _apiClient = new(
        assemblySharedFixture.HttpClient,
        assemblySharedFixture.Configuration.GetProperty(ConfigKey.TestApiAddress),
        assemblySharedFixture.Configuration.GetProperty(ConfigKey.ApiKey));

    private readonly SlotsApiClient _slotsApiClient = new(
        assemblySharedFixture.HttpClient,
        assemblySharedFixture.Configuration.GetProperty(ConfigKey.TestApiAddress),
        assemblySharedFixture.Configuration.GetProperty(ConfigKey.ApiKey));

    private readonly AccessCardsApiClient _accessCardsApiClient = new(
        assemblySharedFixture.HttpClient,
        assemblySharedFixture.Configuration.GetProperty(ConfigKey.TestApiAddress),
        assemblySharedFixture.Configuration.GetProperty(ConfigKey.ApiKey));

    public override async Task InitializeAsync()
    {
        await WithTestDeviceCreated();
        await WithTestSlotInDeviceCreated();
        await WithTestAccessCardCreated();
        await WhenPutAccessQueriedWithSlotNames();
        Logger.Info(this, "Initialized");
    }

    public override async Task DisposeAsync()
    {
        Logger.Info(this, "Dispose :");
        await WhenDeleteAllAccessesQueried();
        await FinallySecondDeviceAccessesAreDeleted();
        await FinallyTestAccessCardIsDeleted();
        await FinallyTestSlotsAreDeleted();
        await FinallyTestDevicesAreDeleted();
    }

    public async Task FinallyTestDevicesAreDeleted(bool withApiKey = true)
    {
        Logger.Info(this, $"Delete devices sent...");

        var response = await _deviceApiClient.DeleteAccessControlDevice(_testDeviceId, withApiKey);
        Logger.Info(this, $"Received response: {response.StatusCode}");

        response = await _deviceApiClient.DeleteAccessControlDevice(_secondDeviceId, withApiKey);
        Logger.Info(this, $"(Second device: {response.StatusCode})");
    }

    public async Task FinallyTestSlotsAreDeleted(bool withApiKey = true)
    {
        Logger.Info(this, $"DELETE Slots sent...");

        var response = await _slotsApiClient.DeleteSlots(_testDeviceId, withApiKey);
        Logger.Info(this, $"Received response: {response.StatusCode}");

        response = await _slotsApiClient.DeleteSlots(_secondDeviceId, withApiKey);
        Logger.Info(this, $"(Second slot: {response.StatusCode})");
    }

    public async Task FinallyTestAccessCardIsDeleted(bool withApiKey = true)
    {
        Logger.Info(this, $"Delete AccessCard sent...");

        LastApiResponse = await _accessCardsApiClient.DeleteAccessCards(new List<string>() { _testAccessCardValue }, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task FinallySecondDeviceAccessesAreDeleted(bool withApiKey = true)
    {
        var response = await _apiClient.DeleteAccesses(_secondDeviceId, null, withApiKey);
        Logger.Info(this, $"(Second device: {response.StatusCode})");
    }

    public async Task WithSecondTestDeviceCreated(bool withApiKey = true)
    {
        Logger.Info(this, $"PUT second device sent...");

        var content = SerializeCorrectPutDeviceRequest();

        LastApiResponse = await _deviceApiClient.PutAccessControlDevice(_secondDeviceId, content, withApiKey);

        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WithTestAccessCardCreated(bool withApiKey = true)
    {
        Logger.Info(this, $"PUT AccessCard sent ...");

        var content = SerializeCorrectPutAccessCardRequest();

        LastApiResponse = await _accessCardsApiClient.PutAccessCards(content, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WithTestDeviceCreated(bool withApiKey = true)
    {
        Logger.Info(this, $"PUT device sent...");

        var content = SerializeCorrectPutDeviceRequest();

        LastApiResponse = await _deviceApiClient.PutAccessControlDevice(_testDeviceId, content, withApiKey);

        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WithTestSlotInDeviceCreated(bool withApiKey = true)
    {
        Logger.Info(this, $"PUT Slot sent...");

        var content = SerializeCorrectPutSlotRequest();

        LastApiResponse = await _slotsApiClient.PutSlots(_testDeviceId, content, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenCopyAccessesToSecondControlSystemQueried(bool withApiKey = true)
    {
        Logger.Info(this, $"COPY accesses sent...");

        LastApiResponse = await _apiClient.CopyAccesses(_testDeviceId, _secondDeviceId, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenMoveAccessesToSecondControlSystemQueried(bool withApiKey = true)
    {
        Logger.Info(this, $"MOVE accesses sent...");

        LastApiResponse = await _apiClient.MoveAccesses(_testDeviceId, _secondDeviceId, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenDeleteAllAccessesQueried(bool withApiKey = true)
    {
        Logger.Info(this, "DELETE access sent...");

        LastApiResponse = await _apiClient.DeleteAccesses(_testDeviceId, null, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenDeleteAllAccessesInDeviceQueried(bool withApiKey = true)
    {
        Logger.Info(this, "DELETE access in device sent...");

        LastApiResponse = await _apiClient.DeleteAccesses(_testDeviceId, new List<string> { _testAccessCardValue }, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenDeleteAccessesQueriedWithSlotNameFilter(bool withApiKey = true)
    {
        Logger.Info(this, "DELETE access with names filter sent...");

        LastApiResponse = await _apiClient.DeleteAccesses(_testDeviceId, new List<string> { _testSlotName }, new List<string> { _testAccessCardValue }, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenDeleteAccessesQueriedWithSlotslotNumberFilter(bool withApiKey = true)
    {
        Logger.Info(this, "DELETE access with SlotNumbers filter sent...");

        LastApiResponse = await _apiClient.DeleteAccesses(_testDeviceId, new List<int> { _testSlotslotNumber }, new List<string> { _testAccessCardValue }, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenGetAllAccessesInDeviceQueried(bool withApiKey = true)
    {
        Logger.Info(this, "GET accesses sent...");

        LastApiResponse = await _apiClient.GetAccesses(_testDeviceId, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenGetAccessesWithSlotsNamesFilterQueried(bool withApiKey = true)
    {
        Logger.Info(this, "GET accesses sent with names filter...");

        LastApiResponse = await _apiClient.GetAccesses(_testDeviceId, new List<string>() { _testSlotName }, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenGetAccessesWithSlotsSlotNumbersFilterQueried(bool withApiKey = true)
    {
        Logger.Info(this, "GET accesses sent with SlotNumbers filter...");

        LastApiResponse = await _apiClient.GetAccesses(_testDeviceId, new List<int>() { _testSlotslotNumber }, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenPutAccessQueriedWithSlotNames(bool withApiKey = true)
    {
        Logger.Info(this, "PUT access sent with slot names...");

        var content = SerializePutAccessRequest();

        LastApiResponse = await _apiClient.PutAccesses(_testDeviceId, content, new List<string> { _testSlotName },  withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenPutAccessQueriedWithSlotslotNumbers(bool withApiKey = true)
    {
        Logger.Info(this, "PUT access sent with slot numbers...");

        var content = SerializePutAccessRequest();

        LastApiResponse = await _apiClient.PutAccesses(_testDeviceId, content, new List<int> { _testSlotslotNumber }, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task ThenAccessesAreCopied()
    {
        var response = await _apiClient.GetAccesses(_secondDeviceId, withApiKey: true);
        var content = await response!.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<GetAccessesResponse>(content, JsonSerializerOptionsProvider.Default);

        responseContent.Should().NotBeNull();
        responseContent!.Data?.Should().NotBeNull();
        responseContent!.Data.Should().BeOfType<Api.Entities.AccessControlDevice>();

        var device = responseContent.Data;
        device!.DeviceName.Should().NotBeNullOrEmpty();
        device!.Description.Should().NotBeNullOrEmpty();
        device.Slots.Should().NotBeNullOrEmpty();
        device.Slots!.FirstOrDefault(s => s.SlotNumber == _testSlotslotNumber).Should().NotBeNull();
        device.Slots!.FirstOrDefault(s => s.SlotNumber == _testSlotslotNumber)!.AccessCards.Should().NotBeNull();
        device.Slots!.FirstOrDefault(s => s.SlotNumber == _testSlotslotNumber)!.AccessCards!.
            FirstOrDefault(t => t.Value == _testAccessCardValue).Should().NotBeNull();
    }

    public async Task ThenSlotIsCopiedIfNotExists()
    {
        var response = await _slotsApiClient.GetSlots(_secondDeviceId, withApiKey: true);
        var content = await response!.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<GetSlotsInDeviceResponse>(content, JsonSerializerOptionsProvider.Default);

        responseContent.Should().NotBeNull();
        responseContent!.Data?.Should().NotBeNull();

        var responseDevice = responseContent.Data;

        responseDevice!.Should().BeOfType<SlotAccesses>();
        responseDevice!.Slots.Should().NotBeNullOrEmpty();
        responseDevice.Slots!.First().Should().NotBeNull();
        responseDevice.Slots!.First().Should().BeOfType<GetSlotsDto>();
        responseDevice.Slots!.Count.Should().BeGreaterThanOrEqualTo(1);
        responseDevice.Slots!.FirstOrDefault(s => s.SlotNumber == _testSlotslotNumber).Should().NotBeNull();
    }

    public async Task ThenListOfAccessCardAccessesIsReturned()
    {
        await ThenStatusCodeIsReturned(HttpStatusCode.OK);

        var content = await LastApiResponse!.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<GetAccessesResponse>(content, JsonSerializerOptionsProvider.Default);

        responseContent.Should().NotBeNull();
        responseContent!.Data?.Should().NotBeNull();
        responseContent!.Data.Should().BeOfType<Api.Entities.AccessControlDevice>();

        var device = responseContent.Data;
        device!.DeviceName.Should().NotBeNullOrEmpty();
        device!.Description.Should().NotBeNullOrEmpty();
        device.Slots.Should().NotBeNullOrEmpty();
        device.Slots!.FirstOrDefault(s => s.SlotName == _testSlotName).Should().NotBeNull();
        device.Slots!.FirstOrDefault(s => s.SlotNumber == _testSlotslotNumber).Should().NotBeNull();
        device.Slots!.FirstOrDefault(s => s.SlotNumber == _testSlotslotNumber)!.AccessCards.Should().NotBeNull();
        device.Slots!.FirstOrDefault(s => s.SlotNumber == _testSlotslotNumber)!.AccessCards!.
            FirstOrDefault(t => t.Value == _testAccessCardValue).Should().NotBeNull();
    }

    public async Task ThenAccessesAreMoved()
    {
        await ThenAccessesAreCopied();

        var sourceAccessesResponse = await _apiClient.GetAccesses(_testDeviceId, withApiKey: true);

        var content = await sourceAccessesResponse!.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<GetAccessesResponse>(content, JsonSerializerOptionsProvider.Default);

        responseContent.Should().NotBeNull();
        responseContent!.Data?.Should().NotBeNull();
        responseContent!.Data.Should().BeOfType<Api.Entities.AccessControlDevice>();

        var device = responseContent.Data;
        device!.DeviceName.Should().NotBeNullOrEmpty();
        device!.Description.Should().NotBeNullOrEmpty();

        if (device.Slots is not null)
        {
            device.Slots!.FirstOrDefault(s => s.SlotName == _testSlotName).Should().NotBeNull();
            device.Slots!.FirstOrDefault(s => s.SlotNumber == _testSlotslotNumber).Should().NotBeNull();
            device.Slots!.FirstOrDefault(s => s.SlotNumber == _testSlotslotNumber)!.AccessCards.Should().BeNullOrEmpty();
        }
    }

    private static StringContent SerializeRequest(object request)
    {
        var jsonRequest = JsonSerializer.Serialize(request, JsonSerializerOptionsProvider.Default);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        return content;
    }

    private StringContent SerializePutAccessRequest()
    {
        var request = new AccessCardValueList()
            {
                AccessCardValues = new List<string> { _testAccessCardValue }
            };
        return SerializeRequest(request);
    }

    private StringContent SerializeCorrectPutAccessCardRequest()
    {
        var correctPutRequest = new PutAccessCardsRequest()
        {
            AccessCardValues = new List<string>() { _testAccessCardValue }
        };

        return SerializeRequest(correctPutRequest);
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
