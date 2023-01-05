using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Domain.Players;

namespace Tournament.Infrastructure.Data
{
    public class RegisteredPlayersEntityConfiguration : BaseEntityConfiguration<RegisteredPlayersEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<RegisteredPlayersEntity> builder)
        {
            builder.ToTable("RegisteredPlayers");

            builder.Ignore(x => x.Rating);

            builder
                .HasOne(x => x.Player1)
                .WithMany()
                .HasForeignKey(x => x.Player1Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Player2)
                .WithMany()
                .HasForeignKey(x => x.Player2Id)
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
