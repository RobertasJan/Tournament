using IdentityModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Domain.Players;
using Tournament.Infrastructure.Data;

namespace Tournament.Domain.Services.Players
{
    public interface IPlayerService
    {
        public Task<Guid> Create(PlayerEntity player, CancellationToken cancellationToken);
        public Task<PlayerEntity> GetById(Guid id, CancellationToken cancellationToken);
        public Task<PlayerEntity> GetByUserId(string id, CancellationToken cancellationToken);
    }

    public class PlayerService : IPlayerService
    {
        private readonly AppDbContext _db;

        public PlayerService(AppDbContext dbContext)
        {
            this._db = dbContext;
        }

        public async Task<Guid> Create(PlayerEntity entity, CancellationToken cancellationToken)
        {
            await _db.Players.AddAsync(entity, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }

        public async Task<PlayerEntity> GetById(Guid id, CancellationToken cancellationToken)
            => (await _db.Players.FirstOrDefaultAsync(x => x.Id == id)) ?? throw new Exception($"Player {id} not found.");

        public async Task<PlayerEntity> GetByUserId(string userId, CancellationToken cancellationToken)
            => (await _db.Players.FirstOrDefaultAsync(x => x.UserId == userId)) ?? throw new Exception($"Player by user id {userId} not found.");

    }
}
