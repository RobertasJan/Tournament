using Tournament.Client.Services;
using Tournament.Server.Models;
using Tournament.Shared.Games;
using Tournament.Shared.Players;
using Tournament.Shared.Tournaments;

namespace Tournament.Client.Models
{
    public class TournamentGroupViewModel
    {
        private readonly TournamentService tournamentService;
        private readonly TournamentGroupService tournamentGroupService;
        private readonly Guid tournamentId;
        private readonly Guid tournamentGroupId;
        private ICollection<RegisteredPlayersModel> registeredPlayers;

        public TournamentGroupModel TournamentGroup { get; private set; }
        public List<MatchesGroupModel> TournamentRounds { get; private set; }

        public TournamentGroupViewModel(TournamentService tournamentService, TournamentGroupService tournamentGroupService, Guid tournamentId, Guid tournamentGroupId)
        {
            this.tournamentService = tournamentService;
            this.tournamentGroupService = tournamentGroupService;
            this.tournamentId = tournamentId;
            this.tournamentGroupId = tournamentGroupId;
        }

        public async Task Load()
        {
            registeredPlayers = await tournamentService.GetRegisteredPlayers(tournamentId, tournamentGroupId);
            TournamentGroup = await this.tournamentGroupService.GetTournamentGroupById(tournamentGroupId);

            var playerCount = registeredPlayers.Count;
            TournamentRounds = new List<MatchesGroupModel>();
            for (var i = 0; i < GetCountOfRounds(playerCount); i++)
            {
                TournamentRounds.Add(new MatchesGroupModel()
                {
                    RoundType = Domain.Games.RoundType.Tree,
                    GroupName = 1,
                    Round = i,
                    TournamentGroupId = tournamentGroupId,
                    Matches = GetMatchTemplates((int)Math.Pow(2, i))
                });
            }
        }

        private int GetCountOfRounds(int playerCount)
        {
            if (playerCount < 2) return 1;
            var baseNumber = Math.Log2(playerCount);
            return (int)Math.Ceiling(baseNumber);
        }

        private ICollection<MatchModel> GetMatchTemplates(int count)
        {
            ICollection<MatchModel> matches = new List<MatchModel>();
            for (var i = 0; i < count; i++)
            {
                matches.Add(new MatchModel() { 
                    
                });
            }
            return matches;
        }

    }
}
