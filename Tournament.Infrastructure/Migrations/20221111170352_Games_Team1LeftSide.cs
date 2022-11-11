using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tournament.Infrastructure.Migrations
{
    public partial class Games_Team1LeftSide : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Team1LeftSide",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Team1LeftSide",
                table: "Games");
        }
    }
}
