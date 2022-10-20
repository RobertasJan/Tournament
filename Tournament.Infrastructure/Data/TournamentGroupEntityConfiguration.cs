using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Tournament.Domain.Tournaments;

namespace Tournament.Infrastructure.Data
{
    public class TournamentGroupEntityConfiguration : BaseEntityConfiguration<TournamentGroupEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<TournamentGroupEntity> builder)
        {
            builder.ToTable("TournamentGroups");

            builder.Property(x => x.OtherTypeName).HasMaxLength(64);
            builder.Property(x => x.OtherMatchTypeName).HasMaxLength(64);
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.MatchType).IsRequired();

            builder
                .HasOne(x => x.Tournament)
                .WithMany(x => x.Groups)
                .HasForeignKey(x => x.TournamentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(x => x.Matches)
                .WithOne(x => x.TournamentGroup)
                .HasForeignKey(x => x.TournamentGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
               .HasMany(x => x.Registrations)
               .WithOne(x => x.TournamentGroup)
               .HasForeignKey(x => x.TournamentGroupId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
