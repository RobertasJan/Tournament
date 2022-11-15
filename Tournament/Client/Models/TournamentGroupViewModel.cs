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
        private readonly GameService gameService;
        private readonly Guid tournamentId;
        private readonly Guid tournamentGroupId;
        private ICollection<RegisteredPlayersModel> registeredPlayers;
        private TournamentModel tournament;

        public TournamentGroupModel TournamentGroup { get; private set; }
        public MatchesGroupModel TournamentRounds { get; private set; }
        public int CountOfRounds { get; private set; }

        public TournamentGroupViewModel(TournamentService tournamentService, TournamentGroupService tournamentGroupService, GameService gameService, Guid tournamentId, Guid tournamentGroupId)
        {
            this.tournamentService = tournamentService;
            this.tournamentGroupService = tournamentGroupService;
            this.gameService = gameService;
            this.tournamentId = tournamentId;
            this.tournamentGroupId = tournamentGroupId;
        }

        public List<MatchModel> GroupAsList()
        {
            var list = new List<MatchModel>();
            RecursiveTake(list, TournamentRounds);
            SetTimes(list);
            return list;
            
        }

        private void SetTimes(List<MatchModel> list)
        {
            var startDate = tournament.StartDate;
            var averageTimeForMatch = tournament.AverageTimePerMatch;
            var courtsCount = tournament.CourtsAvailable;
            var currentCourt = 0;
            var currentTimeForMatch = startDate;
            var lastRound = 0;
            list.OrderBy(x => x.Round).ToList().ForEach(x => {
                if (lastRound == 0 && x.Round == 1)
                {
                    currentTimeForMatch += averageTimeForMatch;
                    currentCourt--;
                }
                //skip byes
                if (x.Round != 0 || (x.Team1?.Player1Name != null && x.Team2?.Player1Name != null))
                {
                    x.MatchDate = currentTimeForMatch;
                    if (currentCourt == courtsCount)
                    {
                        currentCourt = 0;
                        currentTimeForMatch += averageTimeForMatch;
                    }
                    else
                    {
                        currentCourt++;
                    }
                    lastRound = x.Round;
                }
            });
            
        }

        private void RecursiveTake(List<MatchModel> list, MatchesGroupModel model)
        {
            foreach (var match in model.Matches)
            {
                list.Add(match);
            }
            if (model.Matches.Count == 1)
            {
                return;
            }

            if (model.WinnersGroup != null)
            {
                RecursiveTake(list, model.WinnersGroup);
            }
            if (model.LosersGroup != null)
            {
                RecursiveTake(list, model.LosersGroup);
            }
        }



        public async Task Load()
        {
            registeredPlayers = await tournamentService.GetRegisteredPlayers(tournamentId, tournamentGroupId);
            tournament = await tournamentService.GetTournamentById(tournamentId);
            TournamentGroup = await this.tournamentGroupService.GetTournamentGroupById(tournamentGroupId);

            var playerCount = registeredPlayers.Count;
            TournamentRounds = new MatchesGroupModel();
            CountOfRounds = Calculations.GetCountOfRounds(playerCount);
            var maxPlayerCountInRound = (int)Math.Pow(2, CountOfRounds);
            var treeCount = maxPlayerCountInRound / 2;
            if (tournament.State == Domain.Tournaments.TournamentState.Registration || tournament.State == Domain.Tournaments.TournamentState.Created)
            {
                TournamentRounds =
                    CreateGroup(new MatchesGroupModel()
                    {
                        RoundType = Domain.Games.RoundType.Tree,
                        GroupName = 0,
                        Round = 0,
                        TournamentGroupId = tournamentGroupId,
                    }, (int)Math.Pow(2, CountOfRounds - 1), registeredPlayers, true);
            }
            else
            {
               // var matches = await gameService.GetMatches(TournamentGroup.Id.Value);
                var matchesGroups = await gameService.GetMatchesGroups(TournamentGroup.Id.Value);
                var firstGroup = matchesGroups.First(x => x.Round == 0 && x.GroupName == 0);
                var sortedSeeds = Calculations.SortSeeds(firstGroup.Matches.Count * 2);
                var i = 0;
                foreach (var match in firstGroup.Matches.OrderBy(x => x.GroupPosition))
                {
                    match.Round = 0;
                    if (match.Team1 is null)
                    {
                        match.Team1 = new MatchTeamModel()
                        {
                            Seed = sortedSeeds[i * 2]
                        };
                    } else
                    {
                        match.Team1.Seed = sortedSeeds[i * 2];
                    }

                    if (match.Team2 is null)
                    {
                        match.Team2 = new MatchTeamModel()
                        {
                            Seed = sortedSeeds[i * 2 + 1]
                        };
                    }
                    else
                    {
                        match.Team2.Seed = sortedSeeds[i * 2 + 1];
                    }

                    match.GroupName = 0;
                    i++;
                }
                var assignedFirstGroup = AssignGroup(firstGroup, matchesGroups);
                TournamentRounds = assignedFirstGroup;
            }
        }

        private MatchesGroupModel AssignGroup(MatchesGroupModel group, ICollection<MatchesGroupModel> groups)
        {
            if (group.WinnersGroupId is null && group.LosersGroupId is null)
            {
                return group;
            }
            group.WinnersGroup = AssignGroup(groups.FirstOrDefault(x => x.Id == group.WinnersGroupId), groups);
            foreach (var match in group.WinnersGroup.Matches)
            {
                match.Round = group.WinnersGroup.Round;
                match.GroupName = group.WinnersGroup.GroupName;
            }
            group.LosersGroup = AssignGroup(groups.FirstOrDefault(x => x.Id == group.LosersGroupId), groups);
            foreach (var match in group.LosersGroup.Matches)
            {
                match.Round = group.LosersGroup.Round;
                match.GroupName = group.LosersGroup.GroupName;
            }
            return group;

        }

        private int depth = 0;

        private MatchesGroupModel CreateGroup(MatchesGroupModel model, int count, ICollection<RegisteredPlayersModel> registeredPlayers, bool firstRound)
        {
            model.Matches = GetMatchTemplates(count, registeredPlayers, firstRound, model.GroupName, model.Round);
            if (count == 1)
            {
                return model;
            }
            model.WinnersGroup = CreateGroup(new MatchesGroupModel() {
                Round = model.Round + 1,
                RoundType = Domain.Games.RoundType.Tree,
                GroupName = model.GroupName,
                TournamentGroupId = model.TournamentGroupId
            }, count / 2, registeredPlayers, false);
            model.LosersGroup = CreateGroup(new MatchesGroupModel()
            {
                Round = model.Round + 1,
                RoundType = Domain.Games.RoundType.Tree,
                GroupName = ++depth,
                TournamentGroupId = model.TournamentGroupId
            }, count / 2, registeredPlayers, false);
            return model;
        }

        private ICollection<MatchModel> GetMatchTemplates(int count, ICollection<RegisteredPlayersModel> registeredPlayers, bool firstRound, int groupName, int round)
        {
            var maxPlayerCountInRound = count * 2;
            var playersOrdered = registeredPlayers.OrderByDescending(x => x.Rating).ToList();

            ICollection<MatchModel> matches = new List<MatchModel>();
            var sortedSeeds = Calculations.SortSeeds(maxPlayerCountInRound);
            for (var i = 0; i < count; i++)
            {
                matches.Add(new MatchModel()
                {
                    GroupPosition = i,
                    GroupName = groupName,
                    Round = round,
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
