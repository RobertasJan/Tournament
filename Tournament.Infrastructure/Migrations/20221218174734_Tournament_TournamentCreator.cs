using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tournament.Infrastructure.Migrations
{
    public partial class Tournament_TournamentCreator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TournamentCreatorId",
                table: "Tournaments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TournamentCreatorId",
                table: "Tournaments");
        }
    }
}
