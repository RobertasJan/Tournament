using Microsoft.EntityFrameworkCore;
using System.Numerics;
using Tournament.Domain.Players;
using Tournament.Domain.Results;
using Tournament.Infrastructure.Data;
using MatchType = Tournament.Domain.Games.MatchType;

namespace Tournament.Domain.Services.Players
{
    public interface IPlayerService
    {
        public Task<Guid> Create(PlayerEntity player, CancellationToken cancellationToken);
        public Task<ICollection<PlayerEntity>> Get(Guid? tournamentId, string? searchText, Gender? gender, CancellationToken cancellationToken);
        public Task<PlayerEntity> GetById(Guid id, CancellationToken cancellationToken);
        public Task<PlayerEntity> GetByUserId(string id, CancellationToken cancellationToken);
        public Task<ICollection<RegisteredPlayersEntity>> GetTournamentPlayers(Guid tournamentId, Guid? tournamentGroupId, CancellationToken cancellationToken);
        public Task<ICollection<TournamentPlayerModel>> GetAggregatedTournamentPlayers(Guid tournamentId, Guid? tournamentGroupId, CancellationToken cancellationToken);
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
            return entity.Id.Value;
        }

        public async Task<ICollection<PlayerEntity>> Get(Guid? tournamentId, string? searchText, Gender? gender, CancellationToken cancellationToken)
        {
            var query = _db.Players.AsQueryable();
            
            if (tournamentId != null)
            {
                query = query.Where(x => x.PlayerMatches.Any(x => x.Match.MatchesGroup.TournamentGroup.TournamentId == tournamentId));
            }
            if (searchText != null)
            {
                query = query.Where(x => (x.FirstName + " " + x.LastName).Contains(searchText));
            }
            if (gender != null)
            {
                query = query.Where(x => x.Gender == gender);
            }
            var result = query.ToList();
            result.ForEach(x => x.CalculateRatings(_db.Results));
            return result;
        }

        public async Task<ICollection<RegisteredPlayersEntity>> GetTournamentPlayers(Guid tournamentId, Guid? tournamentGroupId, CancellationToken cancellationToken)
        {
            var query = _db.RegisteredPlayers.Include(x => x.TournamentGroup).Include(x => x.Player1).Include(x => x.Player2).AsQueryable();
            if (tournamentGroupId != null)
            {
                query = query.Where(x => x.TournamentGroupId == tournamentGroupId.Value);
            }
            return query.Where(x => x.TournamentGroup.TournamentId == tournamentId).ToList();
        }

        public async Task<PlayerEntity> GetById(Guid id, CancellationToken cancellationToken)
            => (await _db.Players.FirstOrDefaultAsync(x => x.Id == id)).CalculateRatings(_db.Results) ?? throw new Exception($"Player {id} not found.");

        public async Task<PlayerEntity> GetByUserId(string userId, CancellationToken cancellationToken)
            => (await _db.Players.FirstOrDefaultAsync(x => x.UserId == userId)) ?? throw new Exception($"Player by user id {userId} not found.");

        public async Task<ICollection<TournamentPlayerModel>> GetAggregatedTournamentPlayers(Guid tournamentId, Guid? tournamentGroupId, CancellationToken cancellationToken)
        {
            var list = new List<TournamentPlayerModel>();
            var query = _db.RegisteredPlayers.Include(x => x.TournamentGroup).Include(x => x.Player1).Include(x => x.Player2).AsQueryable();
            if (tournamentGroupId != null)
            {
                query = query.Where(x => x.TournamentGroupId == tournamentGroupId.Value);
            }
            query = query.Where(x => x.TournamentGroup.TournamentId == tournamentId);

            var registeredPlayerIds = query.Select(x => x.Player1Id).Union(query.Where(x => x.Player2Id.HasValue).Select(x => x.Player2Id.Value)).ToList();
            foreach (var id in registeredPlayerIds)
            {
                var playerModel = new TournamentPlayerModel();
                var group1 = query.Where(x => x.Player1Id == id).ToList();
                var group2 = query.Where(x => x.Player2Id == id).ToList();
                if (group1.Count > 0)
                {
                    await AssignValues(group1, playerModel);
                }
                if (group2.Count > 0)
                {
                    await AssignValues2(group2, playerModel);
                }
                async Task AssignValues(List<RegisteredPlayersEntity> registeredPlayers, TournamentPlayerModel playerModel)
                {
                    var player = registeredPlayers.First();
                    playerModel.PlayerId = player.Player1.Id.Value;
                    playerModel.PlayerName = player.Player1.ShortenedFullName;
                    var doublesTypes = new MatchType[] { MatchType.MensDoubles, MatchType.WomensDoubles };
                    var singlesTypes = new MatchType[] { MatchType.MensSingles, MatchType.WomensSingles };
                    var singles = registeredPlayers.FirstOrDefault(x => singlesTypes.Contains(x.TournamentGroup.MatchType) && x.Player1Id == playerModel.PlayerId);

                    playerModel.Singles = singles != null;
                    var doubles = registeredPlayers.FirstOrDefault(x => doublesTypes.Contains(x.TournamentGroup.MatchType) && x.Player1Id == playerModel.PlayerId);
                    if (doubles != null)
                    {
                        playerModel.PartnerDoublesId = doubles.Player2Id;
                        playerModel.PartnerDoublesName = doubles.Player2?.ShortenedFullName;
                    }
                    var mixed = registeredPlayers.FirstOrDefault(x => x.TournamentGroup.MatchType == MatchType.MixedDoubles && x.Player1Id == playerModel.PlayerId);
                    if (mixed != null)
                    {
                        playerModel.PartnerMixedId = mixed.Player2Id;
                        playerModel.PartnerMixedName = mixed.Player2?.ShortenedFullName;
                    }

                }
                async Task AssignValues2(List<RegisteredPlayersEntity> registeredPlayers, TournamentPlayerModel playerModel)
                {
                    var player = registeredPlayers.First();
                    playerModel.PlayerId = player.Player2.Id.Value;
                    playerModel.PlayerName = player.Player2.ShortenedFullName;
                    var doublesTypes = new MatchType[] { MatchType.MensDoubles, MatchType.WomensDoubles };
                    var doubles = registeredPlayers.FirstOrDefault(x => doublesTypes.Contains(x.TournamentGroup.MatchType) && x.Player2Id == playerModel.PlayerId);
                    if (doubles != null)
                    {
                        playerModel.PartnerDoublesId = doubles.Player1Id;
                        playerModel.PartnerDoublesName = doubles.Player1.ShortenedFullName;
                    }
                    var mixed = registeredPlayers.FirstOrDefault(x => x.TournamentGroup.MatchType == MatchType.MixedDoubles && x.Player2Id == playerModel.PlayerId);
                    if (mixed != null)
                    {
                        playerModel.PartnerMixedId = mixed.Player1Id;
                        playerModel.PartnerMixedName = mixed.Player1.ShortenedFullName;
                    }

                }
                list.Add(playerModel);
            }
            return list;


        }
    }
    internal static class PlayerExtensions
    {
        public static PlayerEntity CalculateRatings(this PlayerEntity? player, IQueryable<ResultEntity> results)
        {
            player.RatingSingles = results.Where(x => x.PlayerId == player.Id && (x.TournamentGroup.MatchType == MatchType.MensSingles || x.TournamentGroup.MatchType == MatchType.WomensSingles)).OrderByDescending(x => x.RatingPoints).Take(12).Sum(x => x.RatingPoints);
            player.RatingDoubles = results.Where(x => x.PlayerId == player.Id && (x.TournamentGroup.MatchType == MatchType.MensDoubles || x.TournamentGroup.MatchType == MatchType.WomensDoubles)).OrderByDescending(x => x.RatingPoints).Take(12).Sum(x => x.RatingPoints);
            player.RatingMixed = results.Where(x => x.PlayerId == player.Id && (x.TournamentGroup.MatchType == MatchType.MixedDoubles)).OrderByDescending(x => x.RatingPoints).Take(12).Sum(x => x.RatingPoints);
            return player;
        }
    }
}
