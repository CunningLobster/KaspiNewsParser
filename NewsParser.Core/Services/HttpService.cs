using Microsoft.Extensions.Logging;
using NewsParser.Core.ServiceContracts;

namespace NewsParser.Core.Services
{
    public class HttpService : IHttpService
    {
        private readonly ILogger<HttpService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpService(IHttpClientFactory httpClientFactory, ILogger<HttpService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;

        }

        public async Task<string> GetHttpResponse(string url)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                _logger.LogInformation("Run Method {0} from {1}", nameof(GetHttpResponse), nameof(HttpService));

                HttpRequestMessage httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"{url}")
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                string responseString = await httpResponseMessage.Content.ReadAsStringAsync();

                return responseString;
            }
        }
    }
}