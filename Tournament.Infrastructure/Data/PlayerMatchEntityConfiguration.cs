using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Tournament.Domain.Players;

namespace Tournament.Infrastructure.Data
{
    public class PlayerMatchEntityConfiguration : BaseEntityConfiguration<PlayerMatchEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PlayerMatchEntity> builder)
        {
            builder.ToTable("PlayerMatches");

            builder
                .HasOne(x => x.Match)
                .WithMany()
                .HasForeignKey(x => x.MatchId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
              .HasOne(x => x.Player)
              .WithMany()
              .HasForeignKey(x => x.PlayerId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}