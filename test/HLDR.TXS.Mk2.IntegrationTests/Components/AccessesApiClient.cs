using System.Web;

namespace AccessControlSystem.IntegrationTests.Components;

public class AccessesApiClient(HttpClient httpClient, string apiAddress, string apiKey) : BaseApiClient(httpClient, apiAddress, apiKey)
{
    private string _endpointAddressPrefix = $"{apiAddress}/v2.0/devices";

    public async Task<HttpResponseMessage> CopyAccesses(string sourceDeviceId, string targetDeviceId, bool withApiKey)
    {
        var uri = new Uri($"{_endpointAddressPrefix}/{sourceDeviceId}/copyaccessesto/{targetDeviceId}");

        var httpRequest = CreateRequest(HttpMethod.Put, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> GetAccesses(string deviceId, bool withApiKey)
    {
        var uri = new Uri($"{_endpointAddressPrefix}/{deviceId}/accesses");

        var httpRequest = CreateRequest(HttpMethod.Get, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> GetAccesses(string deviceId, List<string> slotNames, bool withApiKey)
    {
        var uri = CreateUriWithQueryParams($"{_endpointAddressPrefix}/{deviceId}/accesses", nameof(slotNames), slotNames);

        var httpRequest = CreateRequest(HttpMethod.Get, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> GetAccesses(string deviceId, List<int> slotNumbers, bool withApiKey)
    {
        var slotslotNumbersString = slotNumbers.Select(p => p.ToString()).ToList();
        var uri = CreateUriWithQueryParams($"{_endpointAddressPrefix}/{deviceId}/accesses", nameof(slotNumbers), slotslotNumbersString);

        var httpRequest = CreateRequest(HttpMethod.Get, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> MoveAccesses(string sourceDeviceId, string targetDeviceId, bool withApiKey)
    {
        var uri = new Uri($"{_endpointAddressPrefix}/{sourceDeviceId}/moveaccessesto/{targetDeviceId}");

        var httpRequest = CreateRequest(HttpMethod.Put, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> PutAccesses(string deviceId, StringContent content, List<string> slotNames, bool withApiKey)
    {
        var uri = CreateUriWithQueryParams($"{_endpointAddressPrefix}/{deviceId}/accesses", nameof(slotNames), slotNames);

        var httpRequest = CreateRequest(HttpMethod.Put, uri, withApiKey);
        httpRequest.Content = content;

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> PutAccesses(string deviceId, StringContent content, List<int> slotNumbers, bool withApiKey)
    {
        var slotslotNumbersString = slotNumbers.Select(p => p.ToString()).ToList();
        var uri = CreateUriWithQueryParams($"{_endpointAddressPrefix}/{deviceId}/accesses", nameof(slotNumbers), slotslotNumbersString);

        var httpRequest = CreateRequest(HttpMethod.Put, uri, withApiKey);
        httpRequest.Content = content;

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> DeleteAccesses(string deviceId, List<string>? accessCardValues, bool withApiKey)
    {
        var uri = new Uri($"{_endpointAddressPrefix}/{deviceId}/accesses");

        if (accessCardValues is not null)
        {
            uri = BuildAccessCardValuesUri($"{_endpointAddressPrefix}/{deviceId}/accesses", accessCardValues);
        }

        var httpRequest = CreateRequest(HttpMethod.Delete, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> DeleteAccesses(string deviceId, List<string> slotNames, List<string>? accessCardValues, bool withApiKey)
    {
        var uri = CreateUriWithQueryParams($"{_endpointAddressPrefix}/{deviceId}/accesses", "SlotNames", slotNames);

        if (accessCardValues is not null)
        {
            uri = BuildAccessCardValuesUri($"{_endpointAddressPrefix}/{deviceId}/accesses", "SlotNames", slotNames, accessCardValues);
        }

        var httpRequest = CreateRequest(HttpMethod.Delete, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> DeleteAccesses(string deviceId, List<int> slotslotNumbers, List<string>? accessCardValues, bool withApiKey)
    {
        var slotslotNumbersString = slotslotNumbers.Select(p => p.ToString()).ToList();
        var uri = CreateUriWithQueryParams($"{_endpointAddressPrefix}/{deviceId}/accesses", "SlotslotNumbers", slotslotNumbersString);

        if (accessCardValues is not null)
        {
            uri = BuildAccessCardValuesUri($"{_endpointAddressPrefix}/{deviceId}/accesses", "SlotslotNumbers", slotslotNumbersString, accessCardValues);
        }

        var httpRequest = CreateRequest(HttpMethod.Delete, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }

    private Uri BuildAccessCardValuesUri(string baseUri, List<string> accessCardValues)
    {
        var builder = new UriBuilder(baseUri);
        var query = HttpUtility.ParseQueryString(builder.Query);

        foreach (var value in accessCardValues)
        {
            query.Add("AccessCardValues", value);
        }

        builder.Query = query.ToString();

        return builder.Uri;
    }

    private Uri BuildAccessCardValuesUri(string baseUri, string? paramName, List<string> queryParams, List<string> accessCardValues)
    {
        var builder = new UriBuilder(baseUri);
        var query = HttpUtility.ParseQueryString(builder.Query);

        foreach (var param in queryParams)
        {
            query.Add($"{paramName}", param);
        }

        foreach (var value in accessCardValues)
        {
            query.Add("AccessCardValues", value);
        }

        builder.Query = query.ToString();

        return builder.Uri;
    }
}
