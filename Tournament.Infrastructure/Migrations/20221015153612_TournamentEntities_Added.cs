using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tournament.Infrastructure.Migrations
{
    public partial class TournamentEntities_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Matches_MatchId",
                table: "Games");

            migrationBuilder.AddColumn<Guid>(
                name: "NextMatchIfLostId",
                table: "Matches",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NextMatchIfWonId",
                table: "Matches",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TournamentGroupId",
                table: "Matches",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LongDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GamesToWin = table.Column<int>(type: "int", nullable: false),
                    PointsToWin = table.Column<int>(type: "int", nullable: false),
                    PointsToFinalize = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EndDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TournamentGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    OtherTypeName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    MatchType = table.Column<byte>(type: "tinyint", nullable: false),
                    OtherMatchTypeName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    TournamentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TournamentEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentGroups_Tournaments_TournamentEntityId",
                        column: x => x.TournamentEntityId,
                        principalTable: "Tournaments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TournamentGroups_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_NextMatchIfLostId",
                table: "Matches",
                column: "NextMatchIfLostId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_NextMatchIfWonId",
                table: "Matches",
                column: "NextMatchIfWonId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_TournamentGroupId",
                table: "Matches",
                column: "TournamentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentGroups_TournamentEntityId",
                table: "TournamentGroups",
                column: "TournamentEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentGroups_TournamentId",
                table: "TournamentGroups",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Matches_MatchId",
                table: "Games",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Matches_NextMatchIfLostId",
                table: "Matches",
                column: "NextMatchIfLostId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Matches_NextMatchIfWonId",
                table: "Matches",
                column: "NextMatchIfWonId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_TournamentGroups_TournamentGroupId",
                table: "Matches",
                column: "TournamentGroupId",
                principalTable: "TournamentGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Matches_MatchId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Matches_NextMatchIfLostId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Matches_NextMatchIfWonId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_TournamentGroups_TournamentGroupId",
                table: "Matches");

            migrationBuilder.DropTable(
                name: "TournamentGroups");

            migrationBuilder.DropTable(
                name: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Matches_NextMatchIfLostId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_NextMatchIfWonId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_TournamentGroupId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "NextMatchIfLostId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "NextMatchIfWonId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "TournamentGroupId",
                table: "Matches");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Matches_MatchId",
                table: "Games",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
