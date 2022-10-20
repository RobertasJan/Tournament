using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tournament.Infrastructure.Migrations
{
    public partial class MatchesGroup_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_TournamentGroups_TournamentGroupId",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "TournamentGroupId",
                table: "Matches",
                newName: "MatchesGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_TournamentGroupId",
                table: "Matches",
                newName: "IX_Matches_MatchesGroupId");

            migrationBuilder.CreateTable(
                name: "MatchesGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Round = table.Column<int>(type: "int", nullable: false),
                    GroupName = table.Column<int>(type: "int", nullable: false),
                    TournamentGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchesGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchesGroups_TournamentGroups_TournamentGroupId",
                        column: x => x.TournamentGroupId,
                        principalTable: "TournamentGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchesGroups_TournamentGroupId",
                table: "MatchesGroups",
                column: "TournamentGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_MatchesGroups_MatchesGroupId",
                table: "Matches",
                column: "MatchesGroupId",
                principalTable: "MatchesGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_MatchesGroups_MatchesGroupId",
                table: "Matches");

            migrationBuilder.DropTable(
                name: "MatchesGroups");

            migrationBuilder.RenameColumn(
                name: "MatchesGroupId",
                table: "Matches",
                newName: "TournamentGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_MatchesGroupId",
                table: "Matches",
                newName: "IX_Matches_TournamentGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_TournamentGroups_TournamentGroupId",
                table: "Matches",
                column: "TournamentGroupId",
                principalTable: "TournamentGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
