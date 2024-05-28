using System.Web;

namespace AccessControlSystem.IntegrationTests.Components;

public class CardAccessesApiClient(HttpClient httpClient, string apiAddress, string apiKey) : BaseApiClient(httpClient, apiAddress, apiKey)
{
    private string _endpointAddress = $"{apiAddress}/v1.0/accesscards";

    public async Task<HttpResponseMessage> GetAccessCardAccesses(string accessCardValue, string? deviceId, bool withApiKey)
    {
        var uri = new Uri($"{_endpointAddress}/{accessCardValue}/accesses");

        var httpRequest = CreateRequest(HttpMethod.Get, uri, withApiKey);

        if (deviceId is not null)
        {
            uri = BuildUriWithQueryParam($"{_endpointAddress}/{accessCardValue}/accesses", "deviceId", deviceId);
        }

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> GetAccessCardAccesses(string accessCardValue, string deviceId, List<string> slotNames, bool withApiKey)
    {
        var uri = BuildFilterUri($"{_endpointAddress}/{accessCardValue}/accesses", deviceId, "slotNames", slotNames);
        var httpRequest = CreateRequest(HttpMethod.Get, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> GetAccessCardAccesses(string accessCardValue, string deviceId, List<int> slotslotNumbers, bool withApiKey)
    {
        var slotslotNumbersString = slotslotNumbers.Select(p => p.ToString()).ToList();
        var uri = BuildFilterUri($"{_endpointAddress}/{accessCardValue}/accesses", deviceId, "slotslotNumbers", slotslotNumbersString);

        var httpRequest = CreateRequest(HttpMethod.Get, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> PutAccessCardAccesses(string accessCardValue, StringContent content, bool withApiKey)
    {
        var uri = new Uri($"{_endpointAddress}/{accessCardValue}/accesses");

        var httpRequest = CreateRequest(HttpMethod.Put, uri, withApiKey);
        httpRequest.Content = content;

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> DeleteAccessCardAccesses(string accessCardValue, string? deviceId, bool withApiKey)
    {
        var uri = new Uri($"{_endpointAddress}/{accessCardValue}/accesses");

        var httpRequest = CreateRequest(HttpMethod.Delete, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> DeleteAccessCardAccesses(string accessCardValue, string deviceId, List<string> slotNames, bool withApiKey)
    {
        var uri = BuildFilterUri($"{_endpointAddress}/{accessCardValue}/accesses", deviceId, "slotNames", slotNames);

        var httpRequest = CreateRequest(HttpMethod.Delete, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> DeleteAccessCardAccesses(string accessCardValue, string deviceId, List<int> slotslotNumbers, bool withApiKey)
    {
        var slotslotNumbersString = slotslotNumbers.Select(p => p.ToString()).ToList();
        var uri = BuildFilterUri($"{_endpointAddress}/{accessCardValue}/accesses", deviceId, "slotslotNumbers", slotslotNumbersString);

        var httpRequest = CreateRequest(HttpMethod.Delete, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }

    private Uri BuildFilterUri(string baseUri, string deviceId, string valueName, List<string> values)
    {
        var builder = new UriBuilder(baseUri);
        var query = HttpUtility.ParseQueryString(builder.Query);

        query.Add($"deviceId", deviceId);

        foreach (var value in values)
        {
            query.Add($"{valueName}", value);
        }

        builder.Query = query.ToString();

        return builder.Uri;
    }
}
