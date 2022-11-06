using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tournament.Infrastructure.Migrations
{
    public partial class Tournament_CourtsAndTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "AverageTimePerMatch",
                table: "Tournaments",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 30, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "CourtsAvailable",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageTimePerMatch",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "CourtsAvailable",
                table: "Tournaments");
        }
    }
}
