using Tournament.Client.Services;
using Tournament.Server.Models;
using Tournament.Shared.Games;
using Tournament.Shared.Players;
using Tournament.Shared.Tournaments;
using static MudBlazor.CategoryTypes;

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
            var countOfRounds = GetCountOfRounds(playerCount);

            for (var i = 0; i < countOfRounds; i++)
            {
                TournamentRounds.Add(new MatchesGroupModel()
                {
                    RoundType = Domain.Games.RoundType.Tree,
                    GroupName = 1,
                    Round = i,
                    TournamentGroupId = tournamentGroupId,
                    Matches = GetMatchTemplates((int)Math.Pow(2, i), registeredPlayers, i == countOfRounds - 1)
                });
            }
        }


        private int GetCountOfRounds(int playerCount)
        {
            if (playerCount < 2) return 1;
            var baseNumber = Math.Log2(playerCount);
            return (int)Math.Ceiling(baseNumber);
        }

        private ICollection<MatchModel> GetMatchTemplates(int count, ICollection<RegisteredPlayersModel> registeredPlayers, bool firstRound)
        {
            var maxCountInRound = count * 2;
            //bool matchesInRound = false;
            //if (Enumerable.Range(count + 1, maxCountInRound).Contains(registeredPlayers.Count()))
            //{
            //    matchesInRound = true;
            //}
            var playersOrdered = registeredPlayers.OrderByDescending(x => x.Rating).ToList();

            ICollection<MatchModel> matches = new List<MatchModel>();
            //var playersOrdered = players.OrderBy(x => x.Rating);
            //  var seeds = Enumerable.Range(1, maxCountInRound);
            var sortedSeeds = SortSeeds(maxCountInRound);
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

        private List<int> SortSeeds(int seedCount)
        {
            var seeds = Enumerable.Range(1, seedCount);
            var seedList = new List<int[]>();
            foreach (var seed in seeds)
            {
                seedList.Add(new int[] { seed });
            }
            var sorted = RecursiveSeedMatch(seedList);
            var sortedList = new List<int>();
            foreach (var sortedArray in sorted)
            {
                foreach (var array in sortedArray)
                {
                    sortedList.Add(array);
                }
            }
            return sortedList;
        }

        private List<int[]> RecursiveSeedMatch(List<int[]> seeds)
        {
            if (seeds.Count == 2)
            {
                seeds[1] = seeds[1].Reverse().ToArray();
                return seeds;
            }
            var newList = new List<int[]>();
            for (var i = 0; i < seeds.Count / 2; i++)
            {
                var seed = seeds[i];
                var newSeed = seed.Concat(seeds[seeds.Count - 1 - i]).ToArray();
                newList.Add(newSeed);
            }

            return RecursiveSeedMatch(newList);
        }

    }
}
