using Microsoft.EntityFrameworkCore;
using Tournament.Domain.Games;
using Tournament.Domain.Players;
using Tournament.Infrastructure.Data;

namespace Tournament.Domain.Services.Games
{
    public interface IMatchService
    {
        Task<MatchEntity> GetById(Guid id, CancellationToken cancellationToken);
        Task<ICollection<MatchEntity>> Get(Guid? tournamentId = null , Guid? tournamentGroupId = null, bool? active = null, CancellationToken cancellationToken = default);
        Task<ICollection<MatchesGroupEntity>> GetGroups(Guid tournamentGroupId, CancellationToken cancellationToken);
        Task<Guid> Create(MatchEntity entity, CancellationToken cancellationToken);
        Task Update(MatchEntity entity, CancellationToken cancellationToken);
        Task SetNextMatch(MatchEntity match);
        Task SetByes(Guid tournamentGroupId, CancellationToken cancellationToken);

    }

    public class MatchService : IMatchService
    {
        private readonly AppDbContext _db;

        public MatchService(AppDbContext dbContext)
        {
            this._db = dbContext;
        }

        public async Task<Guid> Create(MatchEntity entity, CancellationToken cancellationToken)
        {
            await _db.Matches.AddAsync(entity, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return entity.Id.Value;
        }

        public async Task<ICollection<MatchEntity>> Get(Guid? tournamentId = null, Guid? tournamentGroupId = null, bool? active = null, CancellationToken cancellationToken = default)
        {
            var query = _db.Matches.Include(x => x.Games).Include(x => x.PlayersMatches).ThenInclude(x => x.Player).AsQueryable();
            if (tournamentId != null)
            {
                query = query.Where(x => x.MatchesGroup.TournamentGroup.TournamentId == tournamentId);
            }
            if (tournamentGroupId != null)
            {
                query = query.Where(x => x.MatchesGroup.TournamentGroupId == tournamentGroupId);
            }
            if (active != null)
            {
                query = query.Where(x => x.Result == MatchResult.Undetermined 
                && x.PlayersMatches.Any(x => x.Team == Domain.Players.Team.Team1) 
                && x.PlayersMatches.Any(x => x.Team == Domain.Players.Team.Team2));
            }
            return query.ToList();
        }

        public async Task<MatchEntity> GetById(Guid id, CancellationToken cancellationToken)
            => (await _db.Matches.Include(x => x.MatchesGroup).Include(x => x.PlayersMatches).ThenInclude(x => x.Player).FirstOrDefaultAsync(x => x.Id == id)) ?? throw new Exception($"Match {id} not found.");

        public async Task<ICollection<MatchesGroupEntity>> GetGroups(Guid tournamentGroupId, CancellationToken cancellationToken)
        {
            return _db.MatchesGroups
                .Include(x => x.Matches).ThenInclude(x => x.PlayersMatches).ThenInclude(x => x.Player)
                .Include(x => x.Matches).ThenInclude(x => x.Games).Where(x => x.TournamentGroupId == tournamentGroupId).ToList();
        }

        public async Task Update(MatchEntity entity, CancellationToken cancellationToken)
        {
            _db.Update(entity);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task SetByes(Guid tournamentGroupId, CancellationToken cancellationToken)
        {

            var matches = _db.Matches.Include(x => x.PlayersMatches).Include(x => x.MatchesGroup)
                .Where(x => x.MatchesGroup.TournamentGroupId == tournamentGroupId
                    && x.PlayersMatches.Count < 2
                    && x.Result == MatchResult.Undetermined
                    && x.Record == MatchRecord.ToBePlayed).ToList();

            foreach (var match in matches)
            {
                if (await CanAdvanceToNextGroup(match.MatchesGroupId.Value))
                {
                    var playerMatch = match.PlayersMatches.FirstOrDefault();
                    if (playerMatch is null) {
                        match.Result = MatchResult.Undetermined;
                    }
                    else
                    {
                        match.Result = playerMatch.Team == Team.Team1 ? MatchResult.Team1Victory : MatchResult.Team2Victory;
                    }
                    match.Record = MatchRecord.Bye;
                    match.ModifiedAt = DateTime.UtcNow;
                    _db.Update(match);
                    await SetNextMatch(match);
                }
            }
            _db.SaveChanges();
        }
        private async Task<bool> CanAdvanceToNextGroup(Guid matchGroupId)
        {
            var previousGroup = await _db.MatchesGroups.Include(x => x.Matches).FirstOrDefaultAsync(x => x.WinnersGroupId == matchGroupId || x.LosersGroupId == matchGroupId);
            if (previousGroup is null)
            {
                return true;
            }

            if (previousGroup.Matches.Any(x => x.Result == MatchResult.Undetermined))
            {
                return false;
            }
            else
            {
                return await CanAdvanceToNextGroup(previousGroup.Id.Value);
            }

        }
        public async Task SetNextMatch(MatchEntity match)
        {
            var winnerMatchGroup = _db.MatchesGroups.Include(x => x.Matches).ThenInclude(x => x.PlayersMatches).FirstOrDefault(x => x.Id == match.MatchesGroup.WinnersGroupId);
            var losersMatchGroup = _db.MatchesGroups.Include(x => x.Matches).ThenInclude(x => x.PlayersMatches).FirstOrDefault(x => x.Id == match.MatchesGroup.LosersGroupId);
            var nextPositionGroup = (int)Math.Floor(match.GroupPosition.Value / 2d);

            var winners = match.Result == MatchResult.Team1Victory ? match.PlayersMatches.Where(x => x.Team == Team.Team1) : match.PlayersMatches.Where(x => x.Team == Team.Team2);
            var losers = match.Result == MatchResult.Team1Victory ? match.PlayersMatches.Where(x => x.Team == Team.Team2) : match.PlayersMatches.Where(x => x.Team == Team.Team1);


            if (winnerMatchGroup != null)
            {
                var nextMatch = winnerMatchGroup.Matches.First(x => x.GroupPosition == nextPositionGroup);
                await AssignPlayersToMatch(winners, nextMatch, nextMatch.GroupPosition == (match.GroupPosition / 2d));
                _db.Update(nextMatch);
            }

            if (losersMatchGroup != null)
            {
                var nextMatch = losersMatchGroup.Matches.First(x => x.GroupPosition == nextPositionGroup);
                await AssignPlayersToMatch(losers, nextMatch, nextMatch.GroupPosition == (match.GroupPosition / 2d));
                _db.Update(nextMatch);
            }
            _db.SaveChanges();
        }
        private async Task AssignPlayersToMatch(IEnumerable<PlayerMatchEntity> players, MatchEntity match, bool team1)
        {
            foreach (var player in players)
            {
                match.PlayersMatches.Add(new PlayerMatchEntity()
                {
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    Team = team1 ? Team.Team1 : Team.Team2,
                    PlayerId = player.PlayerId,
                    MatchId = match.Id
                });
            }
        }
    }
}