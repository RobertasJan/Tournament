using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tournament.Infrastructure.Migrations
{
    public partial class RoundGroup_Changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Matches_NextMatchIfLostId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Matches_NextMatchIfWonId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_NextMatchIfLostId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_NextMatchIfWonId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "GroupName",
                table: "MatchesGroups");

            migrationBuilder.DropColumn(
                name: "NextMatchIfLostId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "NextMatchIfWonId",
                table: "Matches");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupName",
                table: "MatchesGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_Matches_NextMatchIfLostId",
                table: "Matches",
                column: "NextMatchIfLostId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_NextMatchIfWonId",
                table: "Matches",
                column: "NextMatchIfWonId");

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
        }
    }
}
