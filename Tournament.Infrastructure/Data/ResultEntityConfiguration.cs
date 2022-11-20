using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Domain.Games;
using Tournament.Domain.Tournaments;
using Tournament.Domain.Results;

namespace Tournament.Infrastructure.Data
{
    public class ResultEntityConfiguration : BaseEntityConfiguration<ResultEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ResultEntity> builder)
        {
            builder.ToTable("Results");

            builder.Property(x => x.Position).IsRequired(true);
            builder.Property(x => x.RatingPoints).IsRequired(true);

            builder
                .HasOne(x => x.Player)
                .WithMany()
                .HasForeignKey(x => x.PlayerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
              .HasOne(x => x.TournamentGroup)
              .WithMany()
              .HasForeignKey(x => x.TournamentGroupId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
