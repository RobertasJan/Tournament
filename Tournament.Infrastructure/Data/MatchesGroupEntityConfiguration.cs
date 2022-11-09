using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Domain.Games;

namespace Tournament.Infrastructure.Data
{
    public class MatchesGroupEntityConfiguration : BaseEntityConfiguration<MatchesGroupEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<MatchesGroupEntity> builder)
        {
            builder.ToTable("MatchesGroups");

            builder.Property(x => x.Round).IsRequired();
            builder.Property(x => x.GroupName).IsRequired();
            builder.Property(x => x.RoundType).IsRequired();

            builder
             .HasMany(x => x.Matches)
             .WithOne(x => x.MatchesGroup)
             .HasForeignKey(x => x.MatchesGroupId)
             .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.TournamentGroup)
                .WithMany()
                .HasForeignKey(x => x.TournamentGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.WinnersGroup)
                .WithMany()
                .HasForeignKey(x => x.WinnersGroupId);


            builder.HasOne(x => x.LosersGroup)
                .WithMany()
                .HasForeignKey(x => x.LosersGroupId);
        }
    }
}
