using Microsoft.EntityFrameworkCore;
using Tournament.Domain.Games;
using Tournament.Infrastructure.Data;

namespace Tournament.Domain.Services.Games
{
    public interface IGameService
    {
        Task<Guid> Create(GameEntity entity, CancellationToken cancellationToken);
        Task<ICollection<GameEntity>> Get(Guid? matchId, CancellationToken cancellationToken);
        Task<GameEntity> GetById(Guid id, CancellationToken cancellationToken);
        Task Update(GameEntity entity, CancellationToken cancellationToken);
    }

    public class GameService : IGameService
    {
        private readonly AppDbContext _db;

        public GameService(AppDbContext db)
        {
            this._db = db;
        }

        public async Task<GameEntity> GetById(Guid id, CancellationToken cancellationToken)
            => await _db.Games.FirstOrDefaultAsync(x => x.Id == id, cancellationToken) ?? throw new Exception($"Game {id} not found.");

        public async Task<Guid> Create(GameEntity entity, CancellationToken cancellationToken)
        {
            await _db.Games.AddAsync(entity, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return entity.Id.Value;
        }

        public async Task Update(GameEntity entity, CancellationToken cancellationToken)
        {
            var game = await GetById(entity.Id.Value, cancellationToken);
            game.ModifiedAt = DateTime.UtcNow;
            game.Team1Score = entity.Team1Score;
            game.Team2Score = entity.Team2Score;
            game.Result = entity.Result;
            game.Scores = entity.Scores;
            _db.Games.Update(game);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<ICollection<GameEntity>> Get(Guid? matchId, CancellationToken cancellationToken)
        {
            var query = _db.Games.AsQueryable();
            if (matchId != null)
            {
                query = query.Where(x => x.MatchId == matchId);
            }
            return await query.ToListAsync(cancellationToken);
        }
    }
}
