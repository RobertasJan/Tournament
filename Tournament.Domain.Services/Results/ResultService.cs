using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Domain.Players;
using Tournament.Domain.Results;
using Tournament.Domain.Services.Games;
using Tournament.Domain.Services.Players;
using Tournament.Infrastructure.Data;

namespace Tournament.Domain.Services.Results
{
    public interface IResultService
    {
        Task<ICollection<ResultEntity>> Get(Guid playerId, Domain.Games.MatchType matchType, CancellationToken cancellationToken);
    }

    public class ResultService : IResultService
    {
        private readonly AppDbContext _db;

        public ResultService(AppDbContext db)
        {
            this._db = db;
        }

        public async Task<ICollection<ResultEntity>> Get(Guid playerId, Domain.Games.MatchType matchType, CancellationToken cancellationToken)
        {
            return await _db.Results.Include(x => x.TournamentGroup).ThenInclude(x => x.Tournament).Where(x => x.PlayerId == playerId && x.TournamentGroup.MatchType == matchType).ToListAsync(cancellationToken);
        }
    }
}
