using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Tournament.Domain.Games;
using Tournament.Domain.Players;
using Tournament.Domain.Tournaments;
using Tournament.Domain.User;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.Extensions.Options;

namespace Tournament.Infrastructure.Data;

public class AppDbContext : ApiAuthorizationDbContext<ApplicationUserEntity>
{
    public AppDbContext(
          DbContextOptions options,
          IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
    {
    }

    public DbSet<PlayerEntity> Players { get; set; }
    public DbSet<GameEntity> Games { get; set; }
    public DbSet<MatchEntity> Matches { get; set; }
    public DbSet<MatchesGroupEntity> MatchesGroups { get; set; }
    public DbSet<PlayerMatchEntity> PlayerMatches { get; set; }
    public DbSet<TournamentEntity> Tournaments { get; set; }
    public DbSet<TournamentGroupEntity> TournamentGroups { get; set; }
    public DbSet<RegisteredPlayersEntity> RegisteredPlayers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
