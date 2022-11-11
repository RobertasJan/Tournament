using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using System.Net;
using Tournament.Domain.Players;
using Tournament.Server.Models;
using Tournament.Shared.Games;
using Tournament.Shared.Players;

namespace Tournament.Client.Services
{
    public class GameService : BaseService
    {
        public GameService(HttpClient client, IJSRuntime jsr) : base(client, jsr)
        {

        }

        public async Task<MatchModel> GetMatchById(Guid id)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.GetAsync($"api/matches/{id}", cancellationToken).ConfigureAwait(false);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var match = await httpResponse.Content.ReadAsAsync<MatchModel>(cancellationToken).ConfigureAwait(false);
                return match;
            }
            throw new NotImplementedException("No error handling");
        }

        public async Task<ICollection<MatchModel>> GetMatches(Guid tournamentGroupId)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            QueryString queryString = new QueryString();
            queryString = queryString.Add(nameof(tournamentGroupId), tournamentGroupId.ToString());
            var httpResponse = await _client.GetAsync($"api/matches/{queryString.Value}", cancellationToken).ConfigureAwait(false);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                return await httpResponse.Content.ReadAsAsync<ICollection<MatchModel>>(cancellationToken).ConfigureAwait(false);

            }
            throw new NotImplementedException("No error handling");
        }

        public async Task<ICollection<MatchesGroupModel>> GetMatchesGroups(Guid tournamentGroupId)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            QueryString queryString = new QueryString();
            queryString = queryString.Add(nameof(tournamentGroupId), tournamentGroupId.ToString());
            var httpResponse = await _client.GetAsync($"api/matches/groups/{queryString.Value}", cancellationToken).ConfigureAwait(false);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                return await httpResponse.Content.ReadAsAsync<ICollection<MatchesGroupModel>>(cancellationToken).ConfigureAwait(false);

            }
            throw new NotImplementedException("No error handling");
        }


        public async Task<ICollection<GameModel>> GetMatchGames(Guid id)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.GetAsync($"api/matches/{id}/games", cancellationToken).ConfigureAwait(false);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var games = await httpResponse.Content.ReadAsAsync<ICollection<GameModel>>(cancellationToken).ConfigureAwait(false);
                return games;
            }
            throw new NotImplementedException("No error handling");
        }



        public async Task<Guid> CreateGame(GameModel model, Guid matchId)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.PostAsJsonAsync($"api/matches/{matchId}/games", model, cancellationToken).ConfigureAwait(false);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                return await httpResponse.Content.ReadAsAsync<Guid>(cancellationToken).ConfigureAwait(false);
            }
            throw new NotImplementedException("No error handling");
        }

        public async Task UpdateGame(GameModel model, Guid matchId)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.PutAsJsonAsync($"api/matches/{matchId}/games/{model.Id.Value}", model, cancellationToken).ConfigureAwait(false);
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new NotImplementedException("No error handling");
            }
        }

        public async Task UpdateMatch(MatchModel model)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.PutAsJsonAsync($"api/matches/{model.Id}", model, cancellationToken).ConfigureAwait(false);
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new NotImplementedException("No error handling");
            }
        }

        public async Task<Guid> CreateMatch(MatchModel model)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.PostAsJsonAsync($"api/matches", model, cancellationToken);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
                return await httpResponse.Content.ReadAsAsync<Guid>(cancellationToken).ConfigureAwait(false);
            throw new NotImplementedException("No error handling");
        }
    }
}
