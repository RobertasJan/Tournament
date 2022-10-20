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
        public Task<ICollection<PlayerEntity>> Get(Guid? tournamentId, string? searchText, Gender? gender);
        public Task<PlayerEntity> GetById(Guid id, CancellationToken cancellationToken);
        public Task<PlayerEntity> GetByUserId(string id, CancellationToken cancellationToken);
        public Task<ICollection<RegisteredPlayersEntity>> GetTournamentPlayers(Guid tournamentId, CancellationToken cancellationToken);
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

        public async Task<ICollection<PlayerEntity>> Get(Guid? tournamentId, string? searchText, Gender? gender)
        {
            var query = _db.Players.AsQueryable();
            if (tournamentId != null)
            {
                query = query.Where(x => x.PlayerMatches.Any(x => x.Match.TournamentGroup.TournamentId == tournamentId));
            }
            if (searchText != null)
            {
                query = query.Where(x => (x.FirstName + " " + x.LastName).Contains(searchText));
            }
            if (gender != null)
            {
                query = query.Where(x => x.Gender == gender);
            }
            return query.ToList();
        }

        public async Task<ICollection<RegisteredPlayersEntity>> GetTournamentPlayers(Guid tournamentId, CancellationToken cancellationToken)
            => _db.RegisteredPlayers.Include(x => x.TournamentGroup).Where(x => x.TournamentGroup.TournamentId == tournamentId).ToList();


        public async Task<PlayerEntity> GetById(Guid id, CancellationToken cancellationToken)
            => (await _db.Players.FirstOrDefaultAsync(x => x.Id == id)) ?? throw new Exception($"Player {id} not found.");

        public async Task<PlayerEntity> GetByUserId(string userId, CancellationToken cancellationToken)
            => (await _db.Players.FirstOrDefaultAsync(x => x.UserId == userId)) ?? throw new Exception($"Player by user id {userId} not found.");

    }
}
