using Microsoft.JSInterop;

namespace Tournament.Client.Services
{
    public abstract class BaseService
    {
        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("Tournament.AnonymousAPI");
            _authorizedClient = httpClientFactory.CreateClient("Tournament.ServerAPI");
        }
        protected HttpClient _client;
        protected HttpClient _authorizedClient;

    }
}
