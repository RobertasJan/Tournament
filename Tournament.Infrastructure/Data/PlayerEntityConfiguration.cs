﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Tournament.Domain.Games;
using Tournament.Domain.Players;

namespace Tournament.Infrastructure.Data
{
    public class PlayerEntityConfiguration : BaseEntityConfiguration<PlayerEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PlayerEntity> builder)
        {
            builder.ToTable("Players");

            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(64);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(64);
            builder.Property(x => x.Gender).IsRequired();
            builder.Property(x => x.RatingSingles).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.RatingDoubles).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.RatingMixed).IsRequired().HasDefaultValue(0);
           
            builder.Property(x => x.BirthDate).HasDefaultValue(DateTime.MaxValue).IsRequired();

            builder
             .HasMany(x => x.PlayerMatches)
             .WithOne(x => x.Player)
             .HasForeignKey(x => x.PlayerId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                .WithOne(x => x.Player)
                .HasForeignKey<PlayerEntity>(x => x.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
