﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tournament.Infrastructure.Data;

#nullable disable

namespace Tournament.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20221015173140_Tournaments_IsPublic")]
    partial class Tournaments_IsPublic
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Tournament.Domain.Games.GameEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("SYSDATETIMEOFFSET()");

                    b.Property<Guid>("MatchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("SYSDATETIMEOFFSET()");

                    b.Property<byte?>("Result")
                        .HasColumnType("tinyint");

                    b.Property<string>("Scores")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<int>("Team1Score")
                        .HasColumnType("int");

                    b.Property<int>("Team2Score")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.ToTable("Games", (string)null);
                });

            modelBuilder.Entity("Tournament.Domain.Games.MatchEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("SYSDATETIMEOFFSET()");

                    b.Property<int>("GamesToWin")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("SYSDATETIMEOFFSET()");

                    b.Property<Guid?>("NextMatchIfLostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("NextMatchIfWonId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PointsToFinalize")
                        .HasColumnType("int");

                    b.Property<int>("PointsToWin")
                        .HasColumnType("int");

                    b.Property<byte>("Record")
                        .HasColumnType("tinyint");

                    b.Property<byte>("Result")
                        .HasColumnType("tinyint");

                    b.Property<Guid?>("TournamentGroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte>("Type")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("NextMatchIfLostId");

                    b.HasIndex("NextMatchIfWonId");

                    b.HasIndex("TournamentGroupId");

                    b.ToTable("Matches", (string)null);
                });

            modelBuilder.Entity("Tournament.Domain.Players.PlayerEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("SYSDATETIMEOFFSET()");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("SYSDATETIMEOFFSET()");

                    b.HasKey("Id");

                    b.ToTable("Players", (string)null);
                });

            modelBuilder.Entity("Tournament.Domain.Players.PlayerMatchEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("SYSDATETIMEOFFSET()");

                    b.Property<Guid?>("MatchEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MatchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("SYSDATETIMEOFFSET()");

                    b.Property<Guid?>("PlayerEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Team")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MatchEntityId");

                    b.HasIndex("MatchId");

                    b.HasIndex("PlayerEntityId");

                    b.HasIndex("PlayerId");

                    b.ToTable("PlayerMatches", (string)null);
                });

            modelBuilder.Entity("Tournament.Domain.Tournaments.TournamentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("SYSDATETIMEOFFSET()");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTimeOffset?>("EndDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("GamesToWin")
                        .HasColumnType("int");

                    b.Property<string>("LongDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("SYSDATETIMEOFFSET()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<int>("PointsToFinalize")
                        .HasColumnType("int");

                    b.Property<int>("PointsToWin")
                        .HasColumnType("int");

                    b.Property<bool>("Public")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<DateTimeOffset>("StartDate")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("Tournaments", (string)null);
                });

            modelBuilder.Entity("Tournament.Domain.Tournaments.TournamentGroupEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("SYSDATETIMEOFFSET()");

                    b.Property<byte>("MatchType")
                        .HasColumnType("tinyint");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("SYSDATETIMEOFFSET()");

                    b.Property<string>("OtherMatchTypeName")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("OtherTypeName")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<Guid?>("TournamentEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TournamentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TournamentEntityId");

                    b.HasIndex("TournamentId");

                    b.ToTable("TournamentGroups", (string)null);
                });

            modelBuilder.Entity("Tournament.Domain.Games.GameEntity", b =>
                {
                    b.HasOne("Tournament.Domain.Games.MatchEntity", "Match")
                        .WithMany("Games")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Match");
                });

            modelBuilder.Entity("Tournament.Domain.Games.MatchEntity", b =>
                {
                    b.HasOne("Tournament.Domain.Games.MatchEntity", "NextMatchIfLost")
                        .WithMany()
                        .HasForeignKey("NextMatchIfLostId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Tournament.Domain.Games.MatchEntity", "NextMatchIfWon")
                        .WithMany()
                        .HasForeignKey("NextMatchIfWonId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Tournament.Domain.Tournaments.TournamentGroupEntity", "TournamentGroup")
                        .WithMany("Matches")
                        .HasForeignKey("TournamentGroupId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("NextMatchIfLost");

                    b.Navigation("NextMatchIfWon");

                    b.Navigation("TournamentGroup");
                });

            modelBuilder.Entity("Tournament.Domain.Players.PlayerMatchEntity", b =>
                {
                    b.HasOne("Tournament.Domain.Games.MatchEntity", null)
                        .WithMany("PlayersMatches")
                        .HasForeignKey("MatchEntityId");

                    b.HasOne("Tournament.Domain.Games.MatchEntity", "Match")
                        .WithMany()
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Tournament.Domain.Players.PlayerEntity", null)
                        .WithMany("PlayerMatches")
                        .HasForeignKey("PlayerEntityId");

                    b.HasOne("Tournament.Domain.Players.PlayerEntity", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Match");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Tournament.Domain.Tournaments.TournamentGroupEntity", b =>
                {
                    b.HasOne("Tournament.Domain.Tournaments.TournamentEntity", null)
                        .WithMany("Groups")
                        .HasForeignKey("TournamentEntityId");

                    b.HasOne("Tournament.Domain.Tournaments.TournamentEntity", "Tournament")
                        .WithMany()
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("Tournament.Domain.Games.MatchEntity", b =>
                {
                    b.Navigation("Games");

                    b.Navigation("PlayersMatches");
                });

            modelBuilder.Entity("Tournament.Domain.Players.PlayerEntity", b =>
                {
                    b.Navigation("PlayerMatches");
                });

            modelBuilder.Entity("Tournament.Domain.Tournaments.TournamentEntity", b =>
                {
                    b.Navigation("Groups");
                });

            modelBuilder.Entity("Tournament.Domain.Tournaments.TournamentGroupEntity", b =>
                {
                    b.Navigation("Matches");
                });
#pragma warning restore 612, 618
        }
    }
}
