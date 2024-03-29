﻿using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using System.Net;
using Tournament.Client.Pages;
using Tournament.Domain.Players;
using Tournament.Domain.Tournaments;
using Tournament.Server.Models;
using Tournament.Shared;
using Tournament.Shared.Games;
using Tournament.Shared.Players;
using Tournament.Shared.Tournaments;

namespace Tournament.Client.Services
{
    public class TournamentService : BaseService
    {
        public TournamentService(HttpClient client, IJSRuntime jsr) : base(client, jsr)
        {

        }

        public async Task<TournamentModel> GetTournamentById(Guid id)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.GetAsync($"api/tournaments/{id}", cancellationToken).ConfigureAwait(false);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var tournament = await httpResponse.Content.ReadAsAsync<TournamentModel>(cancellationToken).ConfigureAwait(false);
                return tournament;
            }
            throw new NotImplementedException("No error handling");
        }

        public async Task<ResponseModel<ICollection<TournamentModel>>> GetTournaments(bool? finished = null)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            QueryString queryString = new QueryString();
            queryString = queryString.Add(nameof(finished), finished.ToString());
            var httpResponse = await _client.GetAsync($"api/tournaments/{queryString}", cancellationToken).ConfigureAwait(false);
            return await httpResponse.Content.ReadAsAsync<ResponseModel<ICollection<TournamentModel>>>(cancellationToken).ConfigureAwait(false);
        }


        public async Task<ResponseModel<Guid>> CreateTournament(TournamentModel model)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.PostAsJsonAsync($"api/tournaments", model, cancellationToken);
            return await httpResponse.Content.ReadAsAsync<ResponseModel<Guid>>(cancellationToken).ConfigureAwait(false);
        }

        public async Task AddMatch(Guid tournamentId, MatchModel match)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.PostAsJsonAsync($"api/tournaments/{tournamentId}/match", match, cancellationToken);
            if (httpResponse.StatusCode != HttpStatusCode.OK)
                throw new NotImplementedException("No error handling");
        }

        public async Task<ResponseModel> RegisterPlayer(Guid tournamentId, Guid tournamentGroupId, RegisteredPlayersModel model)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.PostAsJsonAsync($"api/tournaments/{tournamentId}/groups/{tournamentGroupId}/register", model, cancellationToken);
            return await httpResponse.Content.ReadAsAsync<ResponseModel>(cancellationToken).ConfigureAwait(false);
        }

        public async Task<ICollection<RegisteredPlayersModel>> GetRegisteredPlayers(Guid tournamentId, Guid? tournamentGroupId = null)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var routeParams = tournamentGroupId is null ? $"api/tournaments/{tournamentId}/players" : $"api/tournaments/{tournamentId}/groups/{tournamentGroupId}/players";
            var httpResponse = await _client.GetAsync(routeParams, cancellationToken).ConfigureAwait(false);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var players = await httpResponse.Content.ReadAsAsync<ICollection<RegisteredPlayersModel>>(cancellationToken).ConfigureAwait(false);
                return players;
            }
            throw new NotImplementedException("No error handling");
        }

        public async Task<ICollection<TournamentPlayerModel>> GetAggregatedRegisteredPlayers(Guid tournamentId, Guid? tournamentGroupId = null)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var routeParams = tournamentGroupId is null ? $"api/tournaments/{tournamentId}/players/aggregated" : $"api/tournaments/{tournamentId}/groups/{tournamentGroupId}/players/aggregated";
            var httpResponse = await _client.GetAsync(routeParams, cancellationToken).ConfigureAwait(false);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var players = await httpResponse.Content.ReadAsAsync<ICollection<TournamentPlayerModel>>(cancellationToken).ConfigureAwait(false);
                return players;
            }
            throw new NotImplementedException("No error handling");
        }

        public async Task SetState(Guid tournamentId, TournamentState state)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var model = new SetStateModel() { State = state };
            var httpResponse = await _client.PutAsJsonAsync($"api/tournaments/{tournamentId}/state", model, cancellationToken);
            if (httpResponse.StatusCode != HttpStatusCode.OK)
                throw new NotImplementedException("No error handling");
        }

        public async Task<ICollection<MatchModel>> GetUpcomingMatches(Guid tournamentId)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            QueryString queryString = new QueryString();
            queryString = queryString.Add(nameof(tournamentId), tournamentId.ToString());
            queryString = queryString.Add("active", true.ToString());
            var httpResponse = await _client.GetAsync($"api/tournaments/{tournamentId}/matches/{queryString}", cancellationToken).ConfigureAwait(false);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                return await httpResponse.Content.ReadAsAsync<ICollection<MatchModel>>(cancellationToken).ConfigureAwait(false);

            }
            throw new NotImplementedException("No error handling");
        }
    }
}
