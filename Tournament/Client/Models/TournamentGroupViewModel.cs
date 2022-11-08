using Tournament.Client.Services;
using Tournament.Domain.Calculation;
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
            var countOfRounds = Calculations.GetCountOfRounds(playerCount);

            for (var i = 0; i < countOfRounds; i++)
            {
                TournamentRounds.Add(new MatchesGroupModel()
                {
                    RoundType = Domain.Games.RoundType.Tree,
                    GroupName = 1,
                    Round = i,
                    TournamentGroupId = tournamentGroupId,
                    Matches = GetMatchTemplates((int)Math.Pow(2, i), registeredPlayers, i == countOfRounds - 1)
                   // Matches = GetMatchTemplates((int)Math.Pow(2, countOfRounds), registeredPlayers, i == countOfRounds - 1)
                });
            }
        }

        private ICollection<MatchModel> GetMatchTemplates(int count, ICollection<RegisteredPlayersModel> registeredPlayers, bool firstRound)
        {
            var maxPlayerCountInRound = count * 2;
            //bool matchesInRound = false;
            //if (Enumerable.Range(count + 1, maxCountInRound).Contains(registeredPlayers.Count()))
            //{
            //    matchesInRound = true;
            //}
            var playersOrdered = registeredPlayers.OrderByDescending(x => x.Rating).ToList();

            ICollection<MatchModel> matches = new List<MatchModel>();
            //var playersOrdered = players.OrderBy(x => x.Rating);
            //  var seeds = Enumerable.Range(1, maxCountInRound);
            var sortedSeeds = Calculations.SortSeeds(maxPlayerCountInRound);
            for (var i = 0; i < count; i++)
            {
                matches.Add(new MatchModel()
                {
                    Team1 = new MatchTeamModel()
                    {
                        Seed = sortedSeeds[i * 2]
                    },
                    Team2 = new MatchTeamModel()
                    {
                        Seed = sortedSeeds[i * 2 + 1]
                    }
                });
                if (firstRound)
                {
                    var match = matches.Last();
                    match.Team1.Player1Name = playersOrdered.ElementAtOrDefault(match.Team1.Seed - 1)?.Player1Name;
                    match.Team2.Player1Name = playersOrdered.ElementAtOrDefault(match.Team2.Seed - 1)?.Player1Name;
                }
            }
            return matches;
        }



    }
}
