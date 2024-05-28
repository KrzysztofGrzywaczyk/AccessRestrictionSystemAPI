namespace AccessControlSystem.IntegrationTests.Components;

public class AccessCardsApiClient(HttpClient httpClient, string apiAddress, string apiKey) : BaseApiClient(httpClient, apiAddress, apiKey)
{
    private string _endpointAddress = $"{apiAddress}/v2.0/accesscards";

    public async Task<HttpResponseMessage> GetAccessCards(bool withApiKey)
    {
        var uri = new Uri(_endpointAddress);

        var httpRequest = CreateRequest(HttpMethod.Get, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> GetAccessCards(List<string> accessCardValues, bool withApiKey)
    {
        var uri = CreateUriWithQueryParams(_endpointAddress, "accessCardValues", accessCardValues);
        var httpRequest = CreateRequest(HttpMethod.Get, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> PutAccessCards(StringContent content, bool withApiKey)
    {
        var uri = new Uri(_endpointAddress);

        var httpRequest = CreateRequest(HttpMethod.Put, uri, withApiKey);
        httpRequest.Content = content;

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> DeleteAccessCards(List<string> accessCardValues, bool withApiKey)
    {
        var uri = CreateUriWithQueryParams(_endpointAddress, "accessCardValues", accessCardValues);

        var httpRequest = CreateRequest(HttpMethod.Delete, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }
}
