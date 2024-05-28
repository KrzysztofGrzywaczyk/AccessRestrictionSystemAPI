using AccessControlSystem.Api.Models.DataTransferObjects;
using AccessControlSystem.Api.Models.Requests.AccessControlSystems;
using AccessControlSystem.Api.Models.Responses.AccessControlSystems;
using AccessControlSystem.IntegrationTests.Components;
using AccessControlSystem.IntegrationTests.Core.Configuration;
using AccessControlSystem.IntegrationTests.Core.Logging;
using AccessControlSystem.SharedKernel.JsonSerialization;
using System.Net;
using System.Text;
using System.Text.Json;

namespace AccessControlSystem.IntegrationTests.Tests.Device;

public class DevicesContext(AssemblySharedFixture assemblySharedFixture, Logger logger) : BaseContext(assemblySharedFixture, logger)
{
    private readonly string _testDeviceId = $"IntegrationTests-Device-{Guid.NewGuid().ToString("N").Substring(0, 16)}";

    private readonly string _testDeviceName = $"IntegrationTests-Device-{Guid.NewGuid().ToString("N").Substring(0, 16)}";

    private readonly DevicesApiClient _apiClient = new(
        assemblySharedFixture.HttpClient,
        assemblySharedFixture.Configuration.GetProperty(ConfigKey.TestApiAddress),
        assemblySharedFixture.Configuration.GetProperty(ConfigKey.ApiKey));

    public override Task InitializeAsync()
    {
        WhenPutDeviceQueried();
        Logger.Info(this, "Initialized");

        return base.InitializeAsync();
    }

    public override Task DisposeAsync()
    {
        Logger.Info(this, "Dispose:");
        WhenDeleteDeviceIsQueried();

        return base.DisposeAsync();
    }

    public Task WhenGetDevicesQueried(bool withApiKey = true)
    {
        Logger.Info(this, "GET devices sent ...");
        var response = _apiClient.GetAccessControlDevices(withApiKey);

        LastApiResponse = response.Result;
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");

        return Task.CompletedTask;
    }

    public Task WhenGetDevicesQueriedWithFilter(bool withApiKey = true)
    {
        var deviceBaseIds = new List<string>() { _testDeviceId };

        Logger.Info(this, "GET devices with {0} filter device Ids sent ...", deviceBaseIds.Count);
        var response = _apiClient.GetAccessControlDevices(deviceBaseIds, withApiKey);

        LastApiResponse = response.Result;
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");

        return Task.CompletedTask;
    }

    public Task WhenPutDeviceQueried(bool withApiKey = true)
    {
        Logger.Info(this, $"PUT devices sent...");

        var content = SerializeCorrectPutDeviceRequest();

        var response = _apiClient.PutAccessControlDevice(_testDeviceId, content, withApiKey);

        LastApiResponse = response.Result;
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");

        return Task.CompletedTask;
    }

    public Task WhenDeleteDeviceIsQueried(bool withApiKey = true)
    {
        Logger.Info(this, $"Delete devices sent...");

        var response = _apiClient.DeleteAccessControlDevice(_testDeviceId, withApiKey);
        LastApiResponse = response.Result;
        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");

        return Task.CompletedTask;
    }

    public async Task ThenListOfDevicesShouldBeReturned()
    {
        await ThenStatusCodeIsReturned(HttpStatusCode.OK);

        var content = await LastApiResponse!.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<GetDeviceResponse>(content, JsonSerializerOptionsProvider.Default);

        responseContent.Should().NotBeNull();
        responseContent!.Data?.Should().NotBeNull();
        responseContent.Data!.AccessControlDevices.Should().NotBeNull();
        responseContent.Data.AccessControlDevices!.First().Should().NotBeNull();
        responseContent.Data.AccessControlDevices!.First().Should().BeOfType<GetDeviceDto>();
        responseContent.Data.AccessControlDevices!.Count.Should().BeGreaterThanOrEqualTo(1);
    }

    public async Task ThenFilteredDevicesShouldBeReturned()
    {
        await ThenStatusCodeIsReturned(HttpStatusCode.OK);

        var content = await LastApiResponse!.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<GetDeviceResponse>(content, JsonSerializerOptionsProvider.Default);

        responseContent.Should().NotBeNull();
        responseContent!.Data?.Should().NotBeNull();
        responseContent.Data!.AccessControlDevices.Should().NotBeNull();
        responseContent.Data.AccessControlDevices!.First().Should().NotBeNull();
        responseContent.Data.AccessControlDevices!.Count.Should().BeGreaterThanOrEqualTo(1);
        responseContent.Data.AccessControlDevices.First(a => a.DeviceName == _testDeviceId).Should().NotBeNull();
        responseContent.Data.AccessControlDevices.First(a => a.DeviceName == _testDeviceId).Should().BeOfType<GetDeviceDto>();
        responseContent.Data.AccessControlDevices.First(a => a.Description == _testDeviceName).Should().NotBeNull();
        responseContent.Data.AccessControlDevices.First(a => a.DeviceName == _testDeviceId).Should().BeOfType<GetDeviceDto>();
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
            DeviceName = _testDeviceName
        };

        return SerializeRequest(correctPutRequest);
    }
}
