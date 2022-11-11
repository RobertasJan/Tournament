using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tournament.Infrastructure.Migrations
{
    public partial class Games_TeamsSwitched : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Team1Switched",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Team2Switched",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Team1Switched",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Team2Switched",
                table: "Games");
        }
    }
}
