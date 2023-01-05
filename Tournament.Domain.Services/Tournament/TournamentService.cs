using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tournament.Domain.Calculation;
using Tournament.Domain.Games;
using Tournament.Domain.Players;
using Tournament.Domain.Results;
using Tournament.Domain.Services.Games;
using Tournament.Domain.Services.Players;
using Tournament.Domain.Tournaments;
using Tournament.Infrastructure.Data;
using Tournament.Server.Models;
using Tournament.Shared.Games;
using Tournament.Shared.Players;

namespace Tournament.Domain.Services.Tournament
{
    public interface ITournamentService
    {
        Task<TournamentEntity> GetById(Guid id, CancellationToken cancellationToken);
        Task<ICollection<TournamentEntity>> Get(bool? finished = null, CancellationToken cancellationToken = default);
        Task<Guid> Create(TournamentEntity tournament, CancellationToken cancellationToken);
        Task Update(TournamentEntity tournament, CancellationToken cancellationToken);
        Task AddMatch(MatchEntity matchEntity, CancellationToken cancellationToken);

        Task StartDraws(Guid id, CancellationToken cancellationToken);
        Task StartTournament(Guid id, CancellationToken cancellationToken);
        Task FinishTournament(Guid id, CancellationToken cancellationToken);
    }


    public class TournamentService : ITournamentService
    {
        private readonly AppDbContext _db;
        private readonly IPlayerService _playerService;
        private readonly IMatchService _matchService;

        public TournamentService(AppDbContext db, IPlayerService playerService, IMatchService matchService)
        {
            this._db = db;
            this._playerService = playerService;
            this._matchService = matchService;
        }

        public async Task AddMatch(MatchEntity matchEntity, CancellationToken cancellationToken)
        {
            await _db.Matches.AddAsync(matchEntity, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<Guid> Create(TournamentEntity tournament, CancellationToken cancellationToken)
        {
            tournament.Groups = null;
            tournament.State = TournamentState.Registration;
            await _db.Tournaments.AddAsync(tournament, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return tournament.Id.Value;
        }

        public async Task<ICollection<TournamentEntity>> Get(bool? finished = null, CancellationToken cancellationToken = default)
        {
            var query = _db.Tournaments.Where(x => x.Public);
            if (finished != null)
            {
                if (finished.Value)
                {
                    query = query.Where(x => x.State == TournamentState.Finished);
                }
                else
                {
                    query = query.Where(x => x.State != TournamentState.Finished);
                }
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<TournamentEntity> GetById(Guid id, CancellationToken cancellationToken)
        {
            var item = await _db.Tournaments.Include(x => x.Groups).FirstOrDefaultAsync(x => x.Id == id, cancellationToken) ?? throw new Exception($"Tournament {id} not found.");
            return item;
        }


        public async Task StartDraws(Guid id, CancellationToken cancellationToken)
        {
            var tournament = await GetById(id, cancellationToken);
            foreach (var group in tournament.Groups)
            {
                await SetTournamentMatchesBySeed(group, tournament, cancellationToken);
            }
            tournament.State = TournamentState.Draws;
            _db.Update(tournament);
            await _db.SaveChangesAsync(cancellationToken);
        }

        private async Task SetTournamentMatchesBySeed(TournamentGroupEntity group, TournamentEntity tournament, CancellationToken cancellationToken)
        {
            var players = await _playerService.GetTournamentPlayers(group.TournamentId, group.Id, cancellationToken);
            var playerCount = players.Count;
            var countOfRounds = Calculations.GetCountOfRounds(playerCount);
            var matchesCount = (int)Math.Pow(2, countOfRounds - 1);
            var maxPlayerCount = matchesCount * 2;
            var playersOrdered = players.OrderByDescending(x => x.Rating).ToList();

            var seeds = Enumerable.Range(1, maxPlayerCount);
            var roundGroup = new MatchesGroupEntity()
            {
                Round = 0,//Calculations.GetCountOfRounds(maxPlayerCount),
                RoundType = RoundType.Tree,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                Matches = new List<MatchEntity>()
            };

            var sortedSeeds = Calculations.SortSeeds(maxPlayerCount);
            if (playerCount < 3)
            {
                return;
            }

            for (var i = 0; i < matchesCount; i++)
            {
                var seed = sortedSeeds[i * 2] - 1;
                var seed2 = sortedSeeds[i * 2 + 1] - 1;
                var players1 = playersOrdered.ElementAtOrDefault(seed);
                var players2 = playersOrdered.ElementAtOrDefault(seed2);

                if (players1 != null || players2 != null)
                {
                    roundGroup.Matches.Add(new MatchEntity()
                    {
                        Record = MatchRecord.ToBePlayed,
                        Result = MatchResult.Undetermined,
                        GroupPosition = i,
                        GamesToWin = tournament.GamesToWin,
                        PointsToFinalize = Deuces.DeucesList[tournament.PointsToWin],
                        PointsToWin = tournament.PointsToWin,
                        ModifiedAt = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow,
                        Type = group.MatchType,
                        PlayersMatches = new List<PlayerMatchEntity>()
                    });
                    var lastMatch = roundGroup.Matches.Last();
                    if (players1 != null)
                    {
                        lastMatch.PlayersMatches.Add(new PlayerMatchEntity()
                        {
                            Team = Team.Team1,
                            Player = players1.Player1,
                            Match = lastMatch
                        });
                        if (players1.Player2 != null)
                        {
                            lastMatch.PlayersMatches.Add(new PlayerMatchEntity()
                            {
                                Team = Team.Team1,
                                Player = players1.Player2,
                                Match = lastMatch
                            });
                        }
                    }

                    if (players2 != null)
                    {
                        lastMatch.PlayersMatches.Add(new PlayerMatchEntity()
                        {
                            Team = Team.Team2,
                            Player = players2.Player1,
                            Match = lastMatch

                        });
                        if (players2.Player2 != null)
                        {
                            lastMatch.PlayersMatches.Add(new PlayerMatchEntity()
                            {
                                Team = Team.Team2,
                                Player = players2.Player2,
                                Match = lastMatch
                            });
                        }
                    }
                }
            }
            group.Tournament = tournament;
            roundGroup.TournamentGroup = group;
            depth = 0;
            CreateGroupMatches(roundGroup, true, (int)Math.Pow(2, countOfRounds - 1));
            group.MatchesGroups = new List<MatchesGroupEntity>()
            {
                roundGroup
            };

        }
        int depth = 0;
        private MatchesGroupEntity CreateGroupMatches(MatchesGroupEntity group, bool skipFirst, int countOfMatches)
        {
            if (!skipFirst)
            {
                group.Matches = CreateMatches(group, countOfMatches);
            }
            if (group.Matches.Count == 1)
            {
                return group;
            }
            group.WinnersGroup = CreateGroupMatches(new MatchesGroupEntity()
            {
                Round = group.Round + 1,
                GroupName = group.GroupName,
                TournamentGroup = group.TournamentGroup,
                RoundType = RoundType.Tree,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
            }, false, countOfMatches / 2);
            group.LosersGroup = CreateGroupMatches(new MatchesGroupEntity()
            {
                Round = group.Round + 1,
                GroupName = ++depth,
                TournamentGroup = group.TournamentGroup,
                RoundType = RoundType.Tree,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
            }, false, countOfMatches / 2);
            return group;
        }

        private ICollection<MatchEntity> CreateMatches(MatchesGroupEntity group, int countOfMatches)
        {
            ICollection<MatchEntity> matches = new List<MatchEntity>();
            for (var i = 0; i < countOfMatches; i++)
            {
                matches.Add(new MatchEntity()
                {
                    GroupPosition = i,
                    Type = group.TournamentGroup.MatchType,
                    GamesToWin = group.TournamentGroup.Tournament.GamesToWin,
                    Record = MatchRecord.ToBePlayed,
                    PointsToFinalize = Deuces.DeucesList[group.TournamentGroup.Tournament.PointsToWin],
                    PointsToWin = group.TournamentGroup.Tournament.PointsToWin,
                    Result = MatchResult.Undetermined,
                    MatchesGroup = group,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                });
            }
            return matches;
        }

        public async Task StartTournament(Guid id, CancellationToken cancellationToken)
        {
            var tournament = await GetById(id, cancellationToken);
            foreach (var group in tournament.Groups)
            {
                await _matchService.SetByes(group.Id.Value, cancellationToken);
            }
            tournament.State = TournamentState.Ongoing;
            _db.Update(tournament);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task FinishTournament(Guid id, CancellationToken cancellationToken)
        {
            var tournament = await GetById(id, cancellationToken);
            foreach (var group in tournament.Groups)
            {
                await _matchService.SetByes(group.Id.Value, cancellationToken);
            }
            foreach (var group in tournament.Groups)
            {
                var groupPlayers = await _playerService.GetAggregatedTournamentPlayers(id, group.Id, cancellationToken);
                foreach (var player in groupPlayers)
                {
                    var match = _db.Matches.Include(x => x.PlayersMatches).Include(x => x.MatchesGroup).Where(x => x.PlayersMatches.Any(y => y.PlayerId == player.PlayerId)).Where(x => x.MatchesGroup.TournamentGroupId == group.Id)
                        .OrderByDescending(x => x.MatchEnd).First();

                    var playerMatch = match.PlayersMatches.First(x => x.PlayerId == player.PlayerId);
                    var team = playerMatch.Team;
                    var position = match.MatchesGroup.GroupName * 2 + 1;
                    if ((team != Team.Team1 || match.Result != MatchResult.Team1Victory)
                        && (team != Team.Team2 || match.Result != MatchResult.Team2Victory))
                    {
                        position += 1;
                    }
                    _db.Results.Add(new ResultEntity()
                    {
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow,
                        RatingPoints = tournament.Rated ? 256 / (position + 2) : 0,
                        PlayerId = player.PlayerId,
                        Position = position,
                        TournamentGroupId = group.Id.Value
                    }); 
                }

            }
            tournament.State = TournamentState.Finished;
            _db.Update(tournament);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task Update(TournamentEntity tournament, CancellationToken cancellationToken)
        {
            _db.Update(tournament);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
