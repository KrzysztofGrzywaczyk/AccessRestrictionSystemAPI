namespace AccessControlSystem.IntegrationTests.Components;

public class SlotsApiClient(HttpClient httpClient, string apiAddress, string apiKey) : BaseApiClient(httpClient, apiAddress, apiKey)
{
    private string _endpointAddressPrefix = $"{apiAddress}/v1.0/devices";

    public async Task<HttpResponseMessage> GetSlots(string deviceId, bool withApiKey)
    {
        var uri = new Uri($"{_endpointAddressPrefix}/{deviceId}/slots");

        var httpRequest = CreateRequest(HttpMethod.Get, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> GetSlots(string deviceId, List<string> slotNames, bool withApiKey)
    {
        var uri = CreateUriWithQueryParams($"{_endpointAddressPrefix}/{deviceId}/slots", "SlotNames", slotNames);
        var httpRequest = CreateRequest(HttpMethod.Get, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> GetSlots(string deviceId, List<int> slotNumbers, bool withApiKey)
    {
        var slotNumbersString = slotNumbers.Select(p => p.ToString()).ToList();
        var uri = CreateUriWithQueryParams($"{_endpointAddressPrefix}/{deviceId}/slots", "SlotNumbers", slotNumbersString);
        var httpRequest = CreateRequest(HttpMethod.Get, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> PutSlots(string deviceId, StringContent content, bool withApiKey)
    {
        var uri = new Uri($"{_endpointAddressPrefix}/{deviceId}/Slots");

        var httpRequest = CreateRequest(HttpMethod.Put, uri, withApiKey);
        httpRequest.Content = content;

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> DeleteSlots(string deviceId, bool withApiKey)
    {
        var uri = new Uri($"{_endpointAddressPrefix}/{deviceId}/slots");

        var httpRequest = CreateRequest(HttpMethod.Delete, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> DeleteSlots(string deviceId, List<string> slotNames, bool withApiKey)
    {
        var uri = CreateUriWithQueryParams($"{_endpointAddressPrefix}/{deviceId}/slots", "SlotNames", slotNames);

        var httpRequest = CreateRequest(HttpMethod.Delete, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> DeleteSlots(string deviceId, List<int> slotNumbers, bool withApiKey)
    {
        var slotNumbersString = slotNumbers.Select(p => p.ToString()).ToList();
        var uri = CreateUriWithQueryParams($"{_endpointAddressPrefix}/{deviceId}/slots", "SlotNumbers", slotNumbersString);

        var httpRequest = CreateRequest(HttpMethod.Delete, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }
}
