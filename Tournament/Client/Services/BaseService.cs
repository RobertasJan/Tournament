using Microsoft.JSInterop;

namespace Tournament.Client.Services
{
    public abstract class BaseService
    {
        public BaseService(HttpClient httpClient)
        {
            _client = httpClient;
        }
        protected HttpClient _client;

    }
}
