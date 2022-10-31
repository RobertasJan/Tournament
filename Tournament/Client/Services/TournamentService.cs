using System.Net;
using Tournament.Server.Models;
using Tournament.Shared.Players;
using Tournament.Shared.Tournaments;

namespace Tournament.Client.Services
{
    public class TournamentService : BaseService
    {
        public TournamentService(HttpClient client) : base(client)
        {

        }

        public async Task<TournamentModel> GetTournamentById(Guid id)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.GetAsync($"tournaments/{id}", cancellationToken).ConfigureAwait(false);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var tournament = await httpResponse.Content.ReadAsAsync<TournamentModel>(cancellationToken).ConfigureAwait(false);
                return tournament;
            }
            throw new NotImplementedException("No error handling");
        }

        public async Task<ICollection<TournamentModel>> GetTournaments()
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.GetAsync($"tournaments", cancellationToken).ConfigureAwait(false);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var tournaments = await httpResponse.Content.ReadAsAsync<ICollection<TournamentModel>>(cancellationToken).ConfigureAwait(false);
                return tournaments;
            }
            throw new NotImplementedException("No error handling");
        }


        public async Task<Guid> CreateTournament(TournamentModel model)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.PostAsJsonAsync($"tournaments", model, cancellationToken);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
                return await httpResponse.Content.ReadAsAsync<Guid>(cancellationToken).ConfigureAwait(false);
            throw new NotImplementedException("No error handling");
        }

        public async Task AddMatch(Guid tournamentId, MatchModel match)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.PostAsJsonAsync($"tournaments/{tournamentId}/match", match, cancellationToken);
            if (httpResponse.StatusCode != HttpStatusCode.OK)
                throw new NotImplementedException("No error handling");
        }

        public async Task RegisterPlayer(Guid tournamentId, Guid tournamentGroupId, RegisteredPlayersModel model)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.PostAsJsonAsync($"tournaments/{tournamentId}/groups/{tournamentGroupId}/register", model, cancellationToken);
            if (httpResponse.StatusCode != HttpStatusCode.OK)
                throw new NotImplementedException("No error handling");
        }

        public async Task<ICollection<RegisteredPlayersModel>> GetRegisteredPlayers(Guid tournamentId, Guid? tournamentGroupId = null)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var routeParams = tournamentGroupId is null ? $"tournaments/{tournamentId}/players" : $"tournaments/{tournamentId}/groups/{tournamentGroupId}/players";
            var httpResponse = await _client.GetAsync(routeParams, cancellationToken).ConfigureAwait(false);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var tournaments = await httpResponse.Content.ReadAsAsync<ICollection<RegisteredPlayersModel>>(cancellationToken).ConfigureAwait(false);
                return tournaments;
            }
            throw new NotImplementedException("No error handling");
        }
    }
}
