using System.Net;
using Tournament.Client.Pages;
using Tournament.Shared.User;

namespace Tournament.Client.Services
{
    public class UserService : BaseService
    {
        public UserService(HttpClient client) : base(client)
        {

        }

        public async Task Register(UserModel model)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.PostAsJsonAsync($"authentication/register", model, cancellationToken);
            if (httpResponse.StatusCode != HttpStatusCode.OK)
                throw new NotImplementedException("No error handling");
        }

        public async Task Login(UserModel model)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.PostAsJsonAsync($"authentication/login", model, cancellationToken);
            if (httpResponse.StatusCode != HttpStatusCode.OK)
                throw new NotImplementedException("No error handling");
        }

        public async Task SignOut()
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.PostAsJsonAsync($"authentication/signout", cancellationToken);
            if (httpResponse.StatusCode != HttpStatusCode.OK)
                throw new NotImplementedException("No error handling");
        }
    }
}
