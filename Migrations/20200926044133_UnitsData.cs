using Microsoft.EntityFrameworkCore.Migrations;

namespace SarkPayOuts.Migrations
{
    public partial class UnitsData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlockedDate",
                table: "ProjectUnitsData",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpiredDate",
                table: "ProjectUnitsData",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Facing",
                table: "ProjectUnitsData",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mortigaze",
                table: "ProjectUnitsData",
                maxLength: 60,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlockedDate",
                table: "ProjectUnitsData");

            migrationBuilder.DropColumn(
                name: "ExpiredDate",
                table: "ProjectUnitsData");

            migrationBuilder.DropColumn(
                name: "Facing",
                table: "ProjectUnitsData");

            migrationBuilder.DropColumn(
                name: "Mortigaze",
                table: "ProjectUnitsData");
        }
    }
}
