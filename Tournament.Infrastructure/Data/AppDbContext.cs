using System.Numerics;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Tournament.Domain.Games;
using Tournament.Domain.Players;

namespace Tournament.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<PlayerEntity> Players { get; set; }
    public DbSet<GameEntity> Games { get; set; }
    public DbSet<MatchEntity> Matches { get; set; }
    public DbSet<PlayerMatchEntity> PlayerMatches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
