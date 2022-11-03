using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace Tournament.Client.Services
{
    public abstract class BaseService
    {
        public BaseService(HttpClient httpClient, IJSRuntime jsr)
        {
            _client = httpClient;
            _jsr = jsr;
        }

        protected HttpClient _client;
        protected IJSRuntime _jsr;

        public async Task SetToken()
        {
            var token = await _jsr.InvokeAsync<string>("localStorage.getItem", "user").ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(token))
            {
                token = token.Split(';', 2)[1];
            }

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
