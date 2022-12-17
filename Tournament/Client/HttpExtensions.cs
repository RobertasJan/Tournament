using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Tournament.Domain;
using Tournament.Shared;

namespace Tournament.Client
{
    public static class HttpExtensions
    {
        public static async Task ReadResponseAsync(this HttpResponseMessage message)
        {
            if (!message.IsSuccessStatusCode)
            {
                var responseEx = await message.Content.ReadAsStringAsync();
                throw JsonConvert.DeserializeObject<Exception>(responseEx, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full
                });
            }
        }

        public static async Task<T> ReadResponseAsync<T>(this HttpResponseMessage message, CancellationToken cancellationToken) where T : ResponseModel
        {
            if (message.IsSuccessStatusCode)
            {   
                var model = await message.Content.ReadAsAsync<T>(cancellationToken).ConfigureAwait(false);
                return model;
            }
            var responseEx = await message.Content.ReadAsStringAsync();
            throw JsonConvert.DeserializeObject<Exception>(responseEx, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full
            });
        }
    }
}
