using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Tournament.Domain.Games;

namespace Tournament.Infrastructure.Data
{
    public class GameEntityConfiguration : BaseEntityConfiguration<GameEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<GameEntity> builder)
        {
            builder.ToTable("Games");

            builder.Property(x => x.Result).IsRequired(false);
            builder.Property(x => x.Scores).IsRequired(false).HasMaxLength(2048);
            builder.Property(x => x.Team1Score).IsRequired();
            builder.Property(x => x.Team2Score).IsRequired();
            builder.Property(x => x.Team1LeftSide).IsRequired().HasDefaultValue(true);
            builder.Property(x => x.Team1Switched).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.Team2Switched).IsRequired().HasDefaultValue(false);

            builder
                .HasOne(x => x.Match)
                .WithMany()
                .HasForeignKey(x => x.MatchId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
