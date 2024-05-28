using AccessControlSystem.Api.Models.DataTransferObjects;
using AccessControlSystem.Api.Models.Requests.AccessCardAccesses;
using AccessControlSystem.Api.Models.Requests.AccessCards;
using AccessControlSystem.Api.Models.Requests.AccessControlSystems;
using AccessControlSystem.Api.Models.Requests.Slots;
using AccessControlSystem.Api.Models.Responses.AccessCardAccesses;
using AccessControlSystem.Api.Models.Responses.AccessCards;
using AccessControlSystem.IntegrationTests.Components;
using AccessControlSystem.IntegrationTests.Core.Configuration;
using AccessControlSystem.IntegrationTests.Core.Logging;
using AccessControlSystem.SharedKernel.JsonSerialization;
using System.Net;
using System.Text;
using System.Text.Json;

namespace AccessControlSystem.IntegrationTests.Tests;

public class CardAccessesContext(AssemblySharedFixture assemblySharedFixture, Logger logger) : BaseContext(assemblySharedFixture, logger)
{
    private readonly string _testAccessCardValue = $"IntegrationTests-AccessCard-{Guid.NewGuid().ToString("N").Substring(0, 16)}";

    private readonly string _testDeviceId = $"IntegrationTests-Device-{Guid.NewGuid().ToString("N").Substring(0, 16)}";

    private readonly string _testSlotName = $"IntegrationTests-Slot-{Guid.NewGuid().ToString("N").Substring(0, 16)}";

    private readonly int _testSlotslotNumber = 1;

    private readonly DevicesApiClient _deviceApiClient = new(
        assemblySharedFixture.HttpClient,
        assemblySharedFixture.Configuration.GetProperty(ConfigKey.TestApiAddress),
        assemblySharedFixture.Configuration.GetProperty(ConfigKey.ApiKey));

    private readonly CardAccessesApiClient _apiClient = new(
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
        await FinallyTestAccessCardIsDeleted();
        await FinallyTestSlotIsDeleted();
        await FinallyTestDeviceIsDeleted();
    }

    public async Task FinallyTestDeviceIsDeleted(bool withApiKey = true)
    {
        Logger.Info(this, $"Delete device sent...");

        LastApiResponse = await _deviceApiClient.DeleteAccessControlDevice(_testDeviceId, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task FinallyTestSlotIsDeleted(bool withApiKey = true)
    {
        Logger.Info(this, $"DELETE slot sent...");

        LastApiResponse = await _slotsApiClient.DeleteSlots(_testDeviceId, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task FinallyTestAccessCardIsDeleted(bool withApiKey = true)
    {
        Logger.Info(this, $"Delete accessCard sent...");

        LastApiResponse = await _accessCardsApiClient.DeleteAccessCards(new List<string>() { _testAccessCardValue }, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    // ---
    public async Task WhenDeleteAllAccessesQueried(bool withApiKey = true)
    {
        Logger.Info(this, "DELETE accesses sent ...");

        LastApiResponse = await _apiClient.DeleteAccessCardAccesses(_testAccessCardValue, null, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenDeleteAllAccessesInDeviceQueried(bool withApiKey = true)
    {
        Logger.Info(this, "DELETE accesses with Device filter sent ...");

        LastApiResponse = await _apiClient.DeleteAccessCardAccesses(_testAccessCardValue, _testDeviceId, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenDeleteAccessesQueriedWithSlotNamesFilter(bool withApiKey = true)
    {
        Logger.Info(this, "DELETE accesses sent with Slots names filter...");

        LastApiResponse = await _apiClient.DeleteAccessCardAccesses(_testAccessCardValue, _testDeviceId, new List<string> { _testSlotName }, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenDeleteAccessesQueriedWithSlotslotNumbersFilter(bool withApiKey = true)
    {
        Logger.Info(this, "DELETE accesses sent with Slots SlotNumbers filter...");

        LastApiResponse = await _apiClient.DeleteAccessCardAccesses(_testAccessCardValue, _testDeviceId, new List<int> { _testSlotslotNumber }, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenGetAllAccessesQueried(bool withApiKey = true)
    {
        Logger.Info(this, "GET accesses sent ...");

        LastApiResponse = await _apiClient.GetAccessCardAccesses(_testAccessCardValue, null, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenGetAllAccessesInDeviceQueried(bool withApiKey = true)
    {
        Logger.Info(this, "GET accesses sent ...");

        LastApiResponse = await _apiClient.GetAccessCardAccesses(_testAccessCardValue, _testDeviceId, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenGetAccessesQueriedWithNamesFilter(bool withApiKey = true)
    {
        Logger.Info(this, "GET accesses sent with Ids an filter...");

        LastApiResponse = await _apiClient.GetAccessCardAccesses(_testAccessCardValue, _testDeviceId, new List<string> { _testSlotName }, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenGetAccessesQueriedWithSlotNumbersFilter(bool withApiKey = true)
    {
        Logger.Info(this, "GET accesses sent with Ids an filter...");

        LastApiResponse = await _apiClient.GetAccessCardAccesses(_testAccessCardValue, _testDeviceId, new List<int> { _testSlotslotNumber }, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenPutAccessWithAdditionalIncorrectAccessIsQueried(bool withApiKey = true)
    {
        Logger.Info(this, "PUT additional incorrect access sent...");

        var request = new PutCardAccessesRequest()
        {
            SlotAccesses = new List<ProperSlot>()
            {
                new ProperSlot()
                {
                    SlotId = _testDeviceId,
                    SlotName = _testSlotName
                },
                new ProperSlot()
                {
                    SlotId = _testDeviceId,
                    SlotName = Guid.NewGuid().ToString()
                }
            }
        };
        var content = SerializeRequest(request);

        LastApiResponse = await _apiClient.PutAccessCardAccesses(_testAccessCardValue, content, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenPutAccessQueriedWithSlotNames(bool withApiKey = true)
    {
        Logger.Info(this, "PUT access sent with slot names...");

        var content = SerializePutAccessCardAccessRequest(_testSlotName);

        LastApiResponse = await _apiClient.PutAccessCardAccesses(_testAccessCardValue, content, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenPutAccessQueriedWithSlotNumbers(bool withApiKey = true)
    {
        Logger.Info(this, "PUT access sent with slot SlotNumbers...");

        var content = SerializePutAccessCardAccessRequest(_testSlotslotNumber);

        LastApiResponse = await _apiClient.PutAccessCardAccesses(_testAccessCardValue, content, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    // --
    public async Task WithTestAccessCardCreated(bool withApiKey = true)
    {
        Logger.Info(this, $"PUT accessCard sent ...");

        var content = SerializeCorrectPutAccessCardRequest();

        LastApiResponse = await _accessCardsApiClient.PutAccessCards(content, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WithTestDeviceCreated(bool withApiKey = true)
    {
        Logger.Info(this, $"PUT devices sent...");

        var content = SerializeCorrectPutDeviceRequest();

        LastApiResponse = await _deviceApiClient.PutAccessControlDevice(_testDeviceId, content, withApiKey);

        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WithTestSlotInDeviceCreated(bool withApiKey = true)
    {
        Logger.Info(this, $"PUT slot sent...");

        var content = SerializeCorrectPutSlotRequest();

        LastApiResponse = await _slotsApiClient.PutSlots(_testDeviceId, content, withApiKey);
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task ThenFilteredAccessCardsAreReturned()
    {
        await ThenStatusCodeIsReturned(HttpStatusCode.OK);

        var content = await LastApiResponse!.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<GetAccessCardsResponse>(content, JsonSerializerOptionsProvider.Default);

        responseContent.Should().NotBeNull();
        responseContent!.Data.Should().NotBeNull();
        responseContent.Data.Should().NotBeNull();
        responseContent.Data.Should().BeOfType<AccessCardValueList>();
        responseContent.Data!.AccessCardValues.Should().NotBeNullOrEmpty();
        responseContent.Data.AccessCardValues!.First(v => v == _testAccessCardValue).Should().NotBeNull();
    }

    public async Task ThenListOfAccessCardAccessesIsReturned()
    {
        await ThenStatusCodeIsReturned(HttpStatusCode.OK);

        var content = await LastApiResponse!.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<GetCardAccessesResponse>(content, JsonSerializerOptionsProvider.Default);

        responseContent.Should().NotBeNull();
        responseContent!.Data?.Should().NotBeNull();
        responseContent!.Data.Should().BeOfType<AccessCardAccessesDto>();

        var cardAccess = responseContent.Data;
        cardAccess!.AccessCardId.Should().NotBeNull();
        cardAccess.AccessCardValue.Should().Be(_testAccessCardValue);
        cardAccess.SlotAccesses.Should().NotBeNull();
        cardAccess.SlotAccesses.Should().BeOfType<List<SlotAccesses>>();
        cardAccess.SlotAccesses!.Count.Should().BeGreaterThanOrEqualTo(1);
        cardAccess.SlotAccesses.FirstOrDefault(a => a.DeviceName == _testDeviceId).Should().NotBeNull();
        cardAccess.SlotAccesses.FirstOrDefault(a => a.DeviceName == _testDeviceId)!.Slots.Should().NotBeNull();
        cardAccess.SlotAccesses.FirstOrDefault(a => a.DeviceName == _testDeviceId)!.Slots!.Count.Should().BeGreaterThanOrEqualTo(1);
        cardAccess.SlotAccesses.FirstOrDefault(a => a.DeviceName == _testDeviceId)!.Slots!.
            FirstOrDefault(s => s.SlotName == _testSlotName).Should().NotBeNull();
    }

    public async Task ThenMultistatusResponseIsReturned()
    {
        await ThenStatusCodeIsReturned(HttpStatusCode.MultiStatus);

        var content = await LastApiResponse!.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<PutCardAccessesResponse>(content, JsonSerializerOptionsProvider.Default);

        responseContent.Should().NotBeNull();
        responseContent!.Data?.Should().NotBeNull();
        responseContent!.Data.Should().BeOfType<AssignedAndFailedCardAccesses>();

        var resultSlots = responseContent.Data;
        resultSlots.Should().NotBeNull();
        resultSlots!.AssignedSlotAccesses.Should().NotBeNull();
        resultSlots!.AssignedSlotAccesses.Should().BeOfType<List<ProperSlot>>();
        resultSlots.AssignedSlotAccesses!.Count.Should().Be(1);
        resultSlots.AssignedSlotAccesses.First(s => s.SlotName == _testSlotName);
        resultSlots!.FailedSlotAccesses.Should().NotBeNull();
        resultSlots.FailedSlotAccesses.Should().BeOfType<List<ProperSlot>>();
        resultSlots.FailedSlotAccesses!.Count.Should().Be(1);
    }

    private static StringContent SerializeRequest(object request)
    {
        var jsonRequest = JsonSerializer.Serialize(request, JsonSerializerOptionsProvider.Default);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        return content;
    }

    private StringContent SerializePutAccessCardAccessRequest(string slotName)
    {
        var request = new PutCardAccessesRequest()
        {
            SlotAccesses = new List<ProperSlot>()
            {
            new ProperSlot()
                {
                    SlotId = _testDeviceId,
                    SlotName = slotName
                }
            }
        };
        return SerializeRequest(request);
    }

    private StringContent SerializePutAccessCardAccessRequest(int slotslotNumber)
    {
        var request = new PutCardAccessesRequest()
        {
            SlotAccesses = new List<ProperSlot>()
        {
            new ProperSlot()
            {
                SlotId = _testDeviceId,
                SlotNumber = slotslotNumber
            }
        }
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
