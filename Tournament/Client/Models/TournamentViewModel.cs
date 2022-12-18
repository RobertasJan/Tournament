using System.Reflection;
using Tournament.Client.Services;
using Tournament.Domain.Games;
using Tournament.Domain.Players;
using Tournament.Domain.Tournaments;
using Tournament.Server.Models;
using Tournament.Shared;
using Tournament.Shared.Games;
using Tournament.Shared.Players;
using Tournament.Shared.Tournaments;
using MatchType = Tournament.Domain.Games.MatchType;

namespace Tournament.Client.Models
{
    public class TournamentViewModel : BaseViewModel<TournamentModel>
    {

        private TournamentService service { get; set; }

        public IEnumerable<RegisteredPlayersModel>? Players { get; set; } 
        public IEnumerable<TournamentPlayerModel>? TournamentPlayers { get; set; } 
        public IEnumerable<MatchModel>? UpcomingMatches { get; set; }

        public TournamentViewModel(TournamentService service, TournamentModel tournamentModel = null)
        {
            if (tournamentModel != null)
            {
                this.Data = tournamentModel;
            }
            this.service = service;
        }

        public async Task<ResponseModel<Guid>> Save(Guid tournamentCreatorId)
        {
            this.Data.TournamentCreatorId = tournamentCreatorId;
            return await service.CreateTournament(this.Data);
        }
    }
}
