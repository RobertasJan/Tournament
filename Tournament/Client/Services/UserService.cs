using Microsoft.JSInterop;
using System.Net;
using Tournament.Client.Pages;
using Tournament.Shared.Tournaments;
using Tournament.Shared;
using Tournament.Shared.User;

namespace Tournament.Client.Services
{
    public class UserService : BaseService
    {
        public UserService(HttpClient client, IJSRuntime jsr) : base(client, jsr)
        {

        }

        public async Task<ResponseModel<LoginResult>> Register(RegistrationModel model)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.PostAsJsonAsync($"api/auth/register", model, cancellationToken);
            return await httpResponse.Content.ReadAsAsync<ResponseModel<LoginResult>>(cancellationToken).ConfigureAwait(false);
        }

        public async Task<ResponseModel<LoginResult>> Login(LoginModel model)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.PostAsJsonAsync($"api/auth/login", model, cancellationToken);
            return await httpResponse.Content.ReadAsAsync<ResponseModel<LoginResult>>(cancellationToken).ConfigureAwait(false);
        }

        public async Task SignOut()
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.PostAsJsonAsync($"authentication/signout", cancellationToken);
            if (httpResponse.StatusCode != HttpStatusCode.OK)
                throw new NotImplementedException("No error handling");
        }

        public async Task<bool> IsAdmin()
        {
            try
            {
                var token = await _jsr.InvokeAsync<string>("localStorage.getItem", "user").ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(token))
                {
                    token = token.Split(';', 3)[2];
                }
                return token == "Admin";
            } catch (Exception) {
                return false;
            }
        }
    }
}
