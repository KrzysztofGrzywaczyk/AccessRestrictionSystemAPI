using AccessControlSystem.Api.Models.Requests.AccessCards;
using AccessControlSystem.Api.Models.Responses.AccessCards;
using AccessControlSystem.IntegrationTests.Components;
using AccessControlSystem.IntegrationTests.Core.Configuration;
using AccessControlSystem.IntegrationTests.Core.Logging;
using AccessControlSystem.SharedKernel.JsonSerialization;
using System.Net;
using System.Text;
using System.Text.Json;

namespace AccessControlSystem.IntegrationTests.Tests.AccessCard;

public class AccessCardsContext(AssemblySharedFixture assemblySharedFixture, Logger logger) : BaseContext(assemblySharedFixture, logger)
{
    private readonly string _testAccessCardValue = $"IntegrationTests-AccessCard-{Guid.NewGuid().ToString("N").Substring(0, 16)}";

    private readonly AccessCardsApiClient _apiClient = new(
        assemblySharedFixture.HttpClient,
        assemblySharedFixture.Configuration.GetProperty(ConfigKey.TestApiAddress),
        assemblySharedFixture.Configuration.GetProperty(ConfigKey.ApiKey));

    public override async Task InitializeAsync()
    {
        await WhenPutAccessCardQueried();
        Logger.Info(this, "Initialized");
    }

    public override async Task DisposeAsync()
    {
        Logger.Info(this, "Dispose:");
        await WhenDeleteAccessCardQueried();
    }

    public async Task WhenGetAllAccessCardsQueried(bool withAuthToken = true)
    {
        Logger.Info(this, "GET AccessCards sent ...");
        LastApiResponse = await _apiClient.GetAccessCards(withAuthToken);

        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenGetAccessCardQueriedWithFilter(bool withAuthToken = true)
    {
        Logger.Info(this, "GET AccessCards sent with Ids an filter...");
        LastApiResponse = await _apiClient.GetAccessCards(new List<string> { _testAccessCardValue }, withAuthToken);

        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenPutAccessCardQueried(bool withApiKey = true)
    {
        Logger.Info(this, $"PUT AccessCards sent...");

        var content = SerializeCorrectPutAccessCardRequest();

        LastApiResponse = await _apiClient.PutAccessCards(content, withApiKey);

        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task WhenDeleteAccessCardQueried(bool withApiKey = true)
    {
        Logger.Info(this, $"Delete AccessCard sent...");

        LastApiResponse = await _apiClient.DeleteAccessCards(new List<string>() { _testAccessCardValue }, withApiKey);

        Logger.Info(this, $"Received response: {LastApiResponse.StatusCode}");
    }

    public async Task ThenListOfAccessCardsShouldBeReturned()
    {
        await ThenStatusCodeIsReturned(HttpStatusCode.OK);

        var content = await LastApiResponse!.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<GetAccessCardsResponse>(content, JsonSerializerOptionsProvider.Default);

        responseContent.Should().NotBeNull();
        responseContent!.Data?.Should().NotBeNull();
        responseContent.Data!.AccessCardValues.Should().NotBeNull();
        responseContent.Data.AccessCardValues.Should().BeOfType<List<string>>();
        responseContent.Data.AccessCardValues.Should().Contain(_testAccessCardValue);
    }

    public async Task ThenFilteredAccessCardsShouldBeReturned()
    {
        await ThenStatusCodeIsReturned(HttpStatusCode.OK);

        var content = await LastApiResponse!.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<GetAccessCardsResponse>(content, JsonSerializerOptionsProvider.Default);

        responseContent.Should().NotBeNull();
        responseContent!.Data?.Should().NotBeNull();
        responseContent.Data!.AccessCardValues.Should().NotBeNull();
        responseContent.Data.AccessCardValues.Should().BeOfType<List<string>>();
        responseContent.Data.AccessCardValues!.FirstOrDefault().Should().NotBeNull();
        responseContent.Data.AccessCardValues.Should().Contain(_testAccessCardValue);
    }

    private static StringContent SerializeRequest(object request)
    {
        var jsonRequest = JsonSerializer.Serialize(request, JsonSerializerOptionsProvider.Default);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        return content;
    }

    private StringContent SerializeCorrectPutAccessCardRequest()
    {
        var correctPutRequest = new PutAccessCardsRequest()
        {
            AccessCardValues = new List<string>() { _testAccessCardValue }
        };

        return SerializeRequest(correctPutRequest);
    }
}
