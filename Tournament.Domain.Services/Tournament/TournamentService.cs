using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Domain.Calculation;
using Tournament.Domain.Games;
using Tournament.Domain.Players;
using Tournament.Domain.Services.Players;
using Tournament.Domain.Tournaments;
using Tournament.Infrastructure.Data;
using Tournament.Shared.Games;

namespace Tournament.Domain.Services.Tournament
{
    public interface ITournamentService
    {
        Task<TournamentEntity> GetById(Guid id, CancellationToken cancellationToken);
        Task<ICollection<TournamentEntity>> Get(CancellationToken cancellationToken);
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

        public TournamentService(AppDbContext db, IPlayerService playerService)
        {
            this._db = db;
            this._playerService = playerService;
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

        public async Task<ICollection<TournamentEntity>> Get(CancellationToken cancellationToken)
            => await _db.Tournaments.ToListAsync(cancellationToken);

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

            var playersOrdered = players.ToList();
            switch (group.MatchType)
            {
                case Domain.Games.MatchType.MensSingles:
                case Domain.Games.MatchType.WomensSingles:
                    playersOrdered = playersOrdered.OrderByDescending(x => x.Player1.RatingSingles).ToList();
                    break;
                case Domain.Games.MatchType.MensDoubles:
                case Domain.Games.MatchType.WomensDoubles:
                    playersOrdered = playersOrdered.OrderByDescending(x => x.Player1.RatingDoubles).ToList();
                    break;
                case Domain.Games.MatchType.MixedDoubles:
                    playersOrdered = playersOrdered.OrderByDescending(x => x.Player1.RatingMixed).ToList();
                    break;

            }

            var seeds = Enumerable.Range(1, maxPlayerCount);
            var roundGroup = new MatchesGroupEntity()
            {
                Round = Calculations.GetCountOfRounds(maxPlayerCount),
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
                        GroupPosition = i + 1,
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
            group.MatchesGroups = new List<MatchesGroupEntity>()
            {
                roundGroup
            };

        }



        public Task StartTournament(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public Task FinishTournament(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task Update(TournamentEntity tournament, CancellationToken cancellationToken)
        {
            _db.Update(tournament);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
