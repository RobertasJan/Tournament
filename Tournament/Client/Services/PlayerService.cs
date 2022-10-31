using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using System.Net;
using System.Text.Json;
using Tournament.Domain.Players;
using Tournament.Shared.Players;
using Tournament.Shared.Tournaments;

namespace Tournament.Client.Services
{
    public class PlayerService : BaseService
    {
        public PlayerService(HttpClient client, IJSRuntime jsr) : base(client)
        {
            this.jsr = jsr;
        }

        private IJSRuntime jsr;

        public async Task<PlayerModel?> GetCurrent()
        {
            try
            {
                return JsonSerializer.Deserialize<PlayerModel>(await jsr.InvokeAsync<string>("localStorage.getItem", "player").ConfigureAwait(false));
            }
            catch
            {
                return null;
            }
        }

        public async Task<PlayerModel> GetPlayer(string playerId)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.GetAsync($"api/players/{playerId}", cancellationToken).ConfigureAwait(false);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var player = await httpResponse.Content.ReadAsAsync<PlayerModel>(cancellationToken).ConfigureAwait(false);
                return player;
            }
            throw new NotImplementedException("No error handling");
        }

        public async Task<ICollection<PlayerModel>> GetPartnerPlayers(Gender gender, Guid tournamentId, string searchText)
        {
            var allPlayers = await GetPlayers(gender: gender, tournamentId: null, searchText: searchText);
            var tournamentPlayers = await GetPlayers(gender: gender, tournamentId: tournamentId, searchText: searchText);
            return allPlayers.Where(x => !tournamentPlayers.Any(y => y.Id != x.Id)).ToList();
        }

        public async Task<ICollection<PlayerModel>> GetPlayers(Gender? gender = null, Guid? tournamentId = null, string? searchText = null)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            QueryString queryString = new QueryString();
            queryString = queryString.Add(nameof(gender), ((int?)gender)?.ToString());
            queryString = queryString.Add(nameof(tournamentId), tournamentId?.ToString());
            queryString = queryString.Add(nameof(searchText), searchText);
            var httpResponse = await _client.GetAsync($"api/players/{queryString.Value}", cancellationToken).ConfigureAwait(false);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                return await httpResponse.Content.ReadAsAsync<ICollection<PlayerModel>>(cancellationToken).ConfigureAwait(false);

            }
            throw new NotImplementedException("No error handling");
        }
    }
}
