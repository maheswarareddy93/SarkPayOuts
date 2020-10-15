using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SarkPayOuts.Migrations
{
    public partial class ProjectsData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "AgentRegistration",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerEmail",
                table: "AgentPayOuts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "AgentPayOuts",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProjectsData",
                columns: table => new
                {
                    ProjectId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectName = table.Column<string>(maxLength: 70, nullable: true),
                    status = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectsData", x => x.ProjectId);
                });

            migrationBuilder.CreateTable(
                name: "ProjectUnitsData",
                columns: table => new
                {
                    UnitId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UnitSize = table.Column<string>(maxLength: 50, nullable: true),
                    UnitNumber = table.Column<string>(maxLength: 50, nullable: true),
                    ProjectId = table.Column<int>(nullable: false),
                    status = table.Column<string>(maxLength: 20, nullable: true),
                    BookingHistory = table.Column<string>(nullable: true),
                    AdminId = table.Column<int>(nullable: false),
                    PaymentStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUnitsData", x => x.UnitId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectsData");

            migrationBuilder.DropTable(
                name: "ProjectUnitsData");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "AgentRegistration");

            migrationBuilder.DropColumn(
                name: "CustomerEmail",
                table: "AgentPayOuts");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "AgentPayOuts");
        }
    }
}
