using Microsoft.JSInterop;

namespace Tournament.Client.Services
{
    public abstract class BaseService
    {
        protected readonly HttpClient _client;
        public BaseService(HttpClient client)
        {
            _client = client;
        }
    }
}
