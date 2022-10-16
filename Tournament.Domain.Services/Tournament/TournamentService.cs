using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Domain.Games;
using Tournament.Domain.Tournaments;
using Tournament.Infrastructure.Data;

namespace Tournament.Domain.Services.Tournament
{
    public interface ITournamentService
    {
        Task<TournamentEntity> GetById(Guid id, CancellationToken cancellationToken);
        Task<ICollection<TournamentEntity>> Get(CancellationToken cancellationToken);
        Task<Guid> Create(TournamentEntity tournament, CancellationToken cancellationToken);
    }


    public class TournamentService : ITournamentService
    {
        private readonly AppDbContext _db;

        public TournamentService(AppDbContext db)
        {
            this._db = db;
        }

        public async Task<Guid> Create(TournamentEntity tournament, CancellationToken cancellationToken)
        {
            tournament.Groups = null;
            await _db.Tournaments.AddAsync(tournament, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return tournament.Id;
        }

        public async Task<ICollection<TournamentEntity>> Get(CancellationToken cancellationToken)
            => await _db.Tournaments.ToListAsync(cancellationToken);

        public async Task<TournamentEntity> GetById(Guid id, CancellationToken cancellationToken)
        {  
            var item = await _db.Tournaments.Include(x => x.Groups).FirstOrDefaultAsync(x => x.Id == id, cancellationToken) ?? throw new Exception($"Tournament {id} not found.");
            return item;
        } 
    }
}
