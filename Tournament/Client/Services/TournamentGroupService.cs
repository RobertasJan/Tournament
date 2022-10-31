using System.Net;
using Tournament.Server.Models;
using Tournament.Shared.Players;
using Tournament.Shared.Tournaments;

namespace Tournament.Client.Services
{
    public class TournamentGroupService : BaseService
    {
        public TournamentGroupService(HttpClient client) : base(client)
        {

        }

        public async Task<TournamentGroupModel> GetTournamentGroupById(Guid id)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var httpResponse = await _client.GetAsync($"tournamentgroups/{id}", cancellationToken).ConfigureAwait(false);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var tournamentGroup = await httpResponse.Content.ReadAsAsync<TournamentGroupModel>(cancellationToken).ConfigureAwait(false);
                return tournamentGroup;
            }
            throw new NotImplementedException("No error handling");
        }
    }
}
