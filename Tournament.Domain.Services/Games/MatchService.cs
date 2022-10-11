using Microsoft.EntityFrameworkCore;
using Tournament.Domain.Games;
using Tournament.Infrastructure.Data;

namespace Tournament.Domain.Services.Games
{
    public interface IMatchService
    {
        Task<MatchEntity> GetById(Guid id, CancellationToken cancellationToken);
        Task<Guid> Create(MatchEntity entity, CancellationToken cancellationToken);
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
            return entity.Id;
        }

        public async Task<MatchEntity> GetById(Guid id, CancellationToken cancellationToken)
            => (await _db.Matches.FirstOrDefaultAsync(x => x.Id == id)) ?? throw new Exception($"Match {id} not found.");
    }
}