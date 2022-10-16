using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tournament.Infrastructure.Migrations
{
    public partial class TournamentGroup_FK_Fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentGroups_Tournaments_TournamentEntityId",
                table: "TournamentGroups");

            migrationBuilder.DropIndex(
                name: "IX_TournamentGroups_TournamentEntityId",
                table: "TournamentGroups");

            migrationBuilder.DropColumn(
                name: "TournamentEntityId",
                table: "TournamentGroups");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TournamentEntityId",
                table: "TournamentGroups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TournamentGroups_TournamentEntityId",
                table: "TournamentGroups",
                column: "TournamentEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentGroups_Tournaments_TournamentEntityId",
                table: "TournamentGroups",
                column: "TournamentEntityId",
                principalTable: "Tournaments",
                principalColumn: "Id");
        }
    }
}
