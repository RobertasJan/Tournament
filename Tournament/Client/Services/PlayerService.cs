using System.Net;
using Tournament.Shared.Players;
using Tournament.Shared.Tournaments;

namespace Tournament.Client.Services
{
    public class PlayerService : BaseService
    {
        public PlayerService(HttpClient client) : base(client)
        {

        }

        public async Task<PlayerModel> GetPlayer(string playerId)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.GetAsync($"players/{playerId}", CancellationToken.None).ConfigureAwait(false);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var player = await httpResponse.Content.ReadAsAsync<PlayerModel>(CancellationToken.None).ConfigureAwait(false);
                return player;
            }
            throw new NotImplementedException("No error handling");
        }
    }
}
