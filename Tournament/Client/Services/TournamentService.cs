using System.Net;
using Tournament.Server.Models;
using Tournament.Shared.Tournaments;

namespace Tournament.Client.Services
{
    public class TournamentService : BaseService
    {
        public TournamentService(IHttpClientFactory client) : base(client)
        {

        }

        public async Task<TournamentModel> GetTournamentById(Guid id)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.GetAsync($"tournaments/{id}", cancellationToken).ConfigureAwait(false);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var tournament = await httpResponse.Content.ReadAsAsync<TournamentModel>(CancellationToken.None).ConfigureAwait(false);
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
                var tournaments = await httpResponse.Content.ReadAsAsync<ICollection<TournamentModel>>(CancellationToken.None).ConfigureAwait(false);
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
    }
}
