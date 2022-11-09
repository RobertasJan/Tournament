using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tournament.Infrastructure.Migrations
{
    public partial class MatchGroup_GroupName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupName",
                table: "MatchesGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "LosersGroupId",
                table: "MatchesGroups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WinnersGroupId",
                table: "MatchesGroups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MatchesGroups_LosersGroupId",
                table: "MatchesGroups",
                column: "LosersGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchesGroups_WinnersGroupId",
                table: "MatchesGroups",
                column: "WinnersGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchesGroups_MatchesGroups_LosersGroupId",
                table: "MatchesGroups",
                column: "LosersGroupId",
                principalTable: "MatchesGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchesGroups_MatchesGroups_WinnersGroupId",
                table: "MatchesGroups",
                column: "WinnersGroupId",
                principalTable: "MatchesGroups",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchesGroups_MatchesGroups_LosersGroupId",
                table: "MatchesGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchesGroups_MatchesGroups_WinnersGroupId",
                table: "MatchesGroups");

            migrationBuilder.DropIndex(
                name: "IX_MatchesGroups_LosersGroupId",
                table: "MatchesGroups");

            migrationBuilder.DropIndex(
                name: "IX_MatchesGroups_WinnersGroupId",
                table: "MatchesGroups");

            migrationBuilder.DropColumn(
                name: "GroupName",
                table: "MatchesGroups");

            migrationBuilder.DropColumn(
                name: "LosersGroupId",
                table: "MatchesGroups");

            migrationBuilder.DropColumn(
                name: "WinnersGroupId",
                table: "MatchesGroups");
        }
    }
}
