using System.Web;

namespace AccessControlSystem.IntegrationTests.Components
{
    public class BaseApiClient(HttpClient httpClient, string apiAddress, string apiKey)
    {
        private readonly string _apiKeyName = "api-key";

        protected HttpClient HttpClient { get; private set; } = httpClient;

        protected string ApiAddress { get; private set; } = apiAddress;

        protected HttpRequestMessage CreateRequest(HttpMethod httpMethod, Uri uri, bool withApiKey = true)
        {
            var result = new HttpRequestMessage(httpMethod, uri);
            if (!withApiKey)
            {
                return result;
            }

            var builder = new UriBuilder(uri.AbsoluteUri);
            var query = HttpUtility.ParseQueryString(uri.Query);
            query[_apiKeyName] = apiKey;

            builder.Query = query.ToString();

            uri = builder.Uri;
            result.RequestUri = uri;

            return result;
        }

        protected Uri BuildUriWithQueryParam(string baseUri, string paramName, string queryParam)
        {
            var builder = new UriBuilder(baseUri);
            var query = HttpUtility.ParseQueryString(builder.Query);

            query.Add($"{paramName}", queryParam);

            builder.Query = query.ToString();

            return builder.Uri;
        }

        protected Uri CreateUriWithQueryParams(string baseUri, string paramName, List<string> queryParams)
        {
            var builder = new UriBuilder(baseUri);
            var query = HttpUtility.ParseQueryString(builder.Query);

            foreach (var param in queryParams)
            {
                query.Add($"{paramName}", param);
            }

            builder.Query = query.ToString();

            return builder.Uri;
        }
    }
}
