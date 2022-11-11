using Microsoft.EntityFrameworkCore;
using Tournament.Domain.Games;
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
            var query = _db.Matches.Include(x => x.PlayersMatches).ThenInclude(x => x.Player).AsQueryable();
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
            => (await _db.Matches.Include(x => x.PlayersMatches).ThenInclude(x => x.Player).FirstOrDefaultAsync(x => x.Id == id)) ?? throw new Exception($"Match {id} not found.");

        public async Task<ICollection<MatchesGroupEntity>> GetGroups(Guid tournamentGroupId, CancellationToken cancellationToken)
        {
            return _db.MatchesGroups.Include(x => x.Matches).ThenInclude(x => x.PlayersMatches).ThenInclude(x => x.Player).Where(x => x.TournamentGroupId == tournamentGroupId).ToList();
        }

        public async Task Update(MatchEntity entity, CancellationToken cancellationToken)
        {
            _db.Update(entity);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}