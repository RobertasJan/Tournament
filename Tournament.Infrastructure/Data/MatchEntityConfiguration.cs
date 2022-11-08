using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Tournament.Domain.Games;

namespace Tournament.Infrastructure.Data
{
    public class MatchEntityConfiguration : BaseEntityConfiguration<MatchEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<MatchEntity> builder)
        {
            builder.ToTable("Matches");

            builder.Property(x => x.Result).IsRequired();
            builder.Property(x => x.PointsToWin).IsRequired();
            builder.Property(x => x.GamesToWin).IsRequired();
            builder.Property(x => x.PointsToFinalize).IsRequired();
            builder.Property(x => x.Type).IsRequired();

            builder
             .HasMany(x => x.Games)
             .WithOne(x => x.Match)
             .HasForeignKey(x => x.MatchId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Restrict);

            builder
             .HasMany(x => x.PlayersMatches)
             .WithOne(x => x.Match)
             .HasForeignKey(x => x.MatchId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.MatchesGroup)
                .WithMany()
                .HasForeignKey(x => x.MatchesGroupId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
