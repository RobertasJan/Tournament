using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tournament.Infrastructure.Migrations
{
    public partial class Matches_TempNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Player1Name",
                table: "Matches",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Player2Name",
                table: "Matches",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Player3Name",
                table: "Matches",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Player4Name",
                table: "Matches",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Player1Name",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Player2Name",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Player3Name",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Player4Name",
                table: "Matches");
        }
    }
}
