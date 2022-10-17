﻿using System.Net;
using Tournament.Server.Models;
using Tournament.Shared.Games;

namespace Tournament.Client.Services
{
    public class GameService : BaseService
    {
        public GameService(IHttpClientFactory client) : base(client)
        {

        }

        public async Task<MatchModel> GetMatchById(Guid id)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.GetAsync($"matches/{id}", cancellationToken).ConfigureAwait(false);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var match = await httpResponse.Content.ReadAsAsync<MatchModel>(cancellationToken).ConfigureAwait(false);
                return match;
            }
            throw new NotImplementedException("No error handling");
        }

        public async Task<Guid> CreateGame(GameModel model, Guid matchId)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.PostAsJsonAsync($"matches/{matchId}/games", model, cancellationToken).ConfigureAwait(false);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                return await httpResponse.Content.ReadAsAsync<Guid>(cancellationToken).ConfigureAwait(false);
            }
            throw new NotImplementedException("No error handling");
        }

        public async Task UpdateGame(GameModel model, Guid matchId)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.PutAsJsonAsync($"matches/{matchId}/games/{model.Id.Value}", model, cancellationToken).ConfigureAwait(false);
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new NotImplementedException("No error handling");
            }
        }

        public async Task<Guid> CreateMatch(MatchModel model)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.PostAsJsonAsync($"matches", model, cancellationToken);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
                return await httpResponse.Content.ReadAsAsync<Guid>(cancellationToken).ConfigureAwait(false);
            throw new NotImplementedException("No error handling");
        }
    }
}
