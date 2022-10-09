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
            builder.Property(x => x.Scores).IsRequired(false).HasMaxLength(1024);
            builder.Property(x => x.Team1Score).IsRequired();
            builder.Property(x => x.Team2Score).IsRequired();

            builder
                .HasOne(x => x.Match)
                .WithMany()
                .HasForeignKey(x => x.MatchId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
