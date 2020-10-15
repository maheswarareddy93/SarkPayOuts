using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SarkPayOuts.Migrations
{
    public partial class FirsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminDetails",
                columns: table => new
                {
                    AdminId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminDetails", x => x.AdminId);
                });

            migrationBuilder.CreateTable(
                name: "AgentPayOuts",
                columns: table => new
                {
                    LeadUUID = table.Column<string>(nullable: false),
                    AgentId = table.Column<string>(nullable: true),
                    ProjectName = table.Column<string>(nullable: true),
                    UnitSize = table.Column<string>(nullable: true),
                    UnitNumber = table.Column<string>(nullable: true),
                    PayoutPercentage = table.Column<decimal>(nullable: true),
                    TdsPercentage = table.Column<decimal>(nullable: true),
                    GstPercentage = table.Column<decimal>(nullable: true),
                    PayoutAmount = table.Column<decimal>(nullable: true),
                    TdsAmount = table.Column<decimal>(nullable: true),
                    GstAmount = table.Column<decimal>(nullable: true),
                    PayOutStatus = table.Column<string>(nullable: true),
                    CreationDate = table.Column<string>(nullable: true),
                    InvoiceDate = table.Column<string>(nullable: true),
                    InvoiceNumber = table.Column<string>(nullable: true),
                    LeadHistory = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentPayOuts", x => x.LeadUUID);
                });

            migrationBuilder.CreateTable(
                name: "AgentRegistration",
                columns: table => new
                {
                    AgentId = table.Column<string>(nullable: false),
                    AgetName = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PAN = table.Column<string>(nullable: true),
                    Aadhar = table.Column<string>(nullable: true),
                    ParentAgentID = table.Column<string>(nullable: true),
                    BankAccountNumber = table.Column<string>(nullable: true),
                    IFSCCode = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Docuents = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentRegistration", x => x.AgentId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminDetails");

            migrationBuilder.DropTable(
                name: "AgentPayOuts");

            migrationBuilder.DropTable(
                name: "AgentRegistration");
        }
    }
}
