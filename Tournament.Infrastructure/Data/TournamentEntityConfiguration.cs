using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Domain.Players;
using Tournament.Domain.Tournaments;

namespace Tournament.Infrastructure.Data
{
    public class TournamentEntityConfiguration : BaseEntityConfiguration<TournamentEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<TournamentEntity> builder)
        {
            builder.ToTable("Tournaments");

            builder.Property(x => x.PointsToWin).IsRequired();
            builder.Property(x => x.GamesToWin).IsRequired();
            builder.Property(x => x.PointsToFinalize).IsRequired();

            builder.Property(x => x.Public).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.Rated).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.Address).IsRequired().HasDefaultValue("");
            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.EndDate);
            builder.Property(x => x.TournamentCreatorId).HasDefaultValue(Guid.Empty);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(256);
            builder.Property(x => x.LongDescription).IsRequired();
            builder.Property(x => x.State).IsRequired().HasDefaultValue(TournamentState.Created);
            builder.Property(x => x.CourtsAvailable).IsRequired().HasDefaultValue(1);
            builder.Property(x => x.AverageTimePerMatch).IsRequired().HasDefaultValue(TimeSpan.FromMinutes(30));

            builder
             .HasMany(x => x.Groups)
             .WithOne(x => x.Tournament)
             .HasForeignKey(x => x.TournamentId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
