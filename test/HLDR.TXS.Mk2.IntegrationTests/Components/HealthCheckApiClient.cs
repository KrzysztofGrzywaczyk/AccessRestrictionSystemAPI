namespace AccessControlSystem.IntegrationTests.Components;

public class HealthCheckApiClient(HttpClient httpClient, string apiAddress, string apiKey) : BaseApiClient(httpClient, apiAddress, apiKey)
{
    public async Task<HttpResponseMessage> GetHealthcheck()
    {
        var uri = new Uri($"{ApiAddress.Replace("/api", string.Empty)}/health");

        var httpRequest = CreateRequest(HttpMethod.Get, uri, false);

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> GetHealthcheckWithNoDependencies()
    {
        var uri = new Uri($"{ApiAddress.Replace("/api", string.Empty)}/health/no-dependencies");

        var httpRequest = CreateRequest(HttpMethod.Get, uri, false);

        return await HttpClient.SendAsync(httpRequest);
    }
}
