namespace AccessControlSystem.IntegrationTests.Components;

public class DevicesApiClient(HttpClient httpClient, string apiAddress, string apiKey) : BaseApiClient(httpClient, apiAddress, apiKey)
{
    private string _endpointAddress = $"{apiAddress}/v1.0/devices";

    public async Task<HttpResponseMessage> GetAccessControlDevices(bool withApiKey)
    {
        var uri = new Uri(_endpointAddress);

        var httpRequest = CreateRequest(HttpMethod.Get, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> GetAccessControlDevices(List<string> deviceIds, bool withApiKey)
    {
        var uri = CreateUriWithQueryParams(_endpointAddress, "deviceIds", deviceIds);

        var httpRequest = CreateRequest(HttpMethod.Get, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> PutAccessControlDevice(string deviceId, StringContent content, bool withApiKey)
    {
        var uri = new Uri($"{_endpointAddress}/{deviceId}");

        var httpRequest = CreateRequest(HttpMethod.Put, uri, withApiKey);
        httpRequest.Content = content;

        return await HttpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResponseMessage> DeleteAccessControlDevice(string deviceId, bool withApiKey)
    {
        var uri = new Uri($"{_endpointAddress}/{deviceId}");

        var httpRequest = CreateRequest(HttpMethod.Delete, uri, withApiKey);

        return await HttpClient.SendAsync(httpRequest);
    }
}
