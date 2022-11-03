﻿using Tournament.Client.Services;
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
            var countOfRounds = GetCountOfRounds(playerCount);
            for (var i = 0; i < countOfRounds; i++)
            {
                TournamentRounds.Add(new MatchesGroupModel()
                {
                    RoundType = Domain.Games.RoundType.Tree,
                    GroupName = 1,
                    Round = i,
                    TournamentGroupId = tournamentGroupId,
                    Matches = GetMatchTemplates((int)Math.Pow(2, i), countOfRounds, registeredPlayers)
                });
            }
        }

        private int GetCountOfRounds(int playerCount)
        {
            if (playerCount < 2) return 1;
            var baseNumber = Math.Log2(playerCount);
            return (int)Math.Ceiling(baseNumber);
        }

        private ICollection<MatchModel> GetMatchTemplates(int count, int countOfRounds, IEnumerable<RegisteredPlayersModel> players)
        {
            var maxCountInRound = count * 2;
            if (Enumerable.Range(count + 1, maxCountInRound).Contains(players.Count()))
            {
                var matchesInRound = players.Count() - count;
            }

            ICollection<MatchModel> matches = new List<MatchModel>();
            var playersOrdered = players.OrderBy(x => x.Rating);
          //  var seeds = Enumerable.Range(1, maxCountInRound);
            var sortedSeeds = SortSeeds(maxCountInRound);
            for (var i = 0; i < count; i++)
            {
                matches.Add(new MatchModel()
                {
                    Seed = sortedSeeds[i]
                });
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
                return seeds;
            }
            var newList = new List<int[]>();
            for (var i = 0; i < seeds.Count / 2; i++)
            {
                var seed = seeds[i];
                var newSeed = seed.Concat(seeds[seeds.Count - i]).ToArray();
                newList.Add(newSeed);
            }

            return RecursiveSeedMatch(newList);
        }

    }
}
