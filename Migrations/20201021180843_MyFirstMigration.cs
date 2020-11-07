using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SarkPayOuts.Migrations
{
    public partial class MyFirstMigration : Migration
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
                    IsActive = table.Column<bool>(nullable: false),
                    password = table.Column<string>(maxLength: 10, nullable: true),
                    BlockedUnits = table.Column<string>(nullable: true),
                    BookingConfirmed = table.Column<string>(nullable: true),
                    RejectedUnits = table.Column<string>(nullable: true),
                    AdminUUID = table.Column<string>(nullable: true)
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
                    CustomerName = table.Column<string>(nullable: true),
                    CustomerEmail = table.Column<string>(nullable: true),
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
                    AccountHolderName = table.Column<string>(nullable: true),
                    BankAccountNumber = table.Column<string>(nullable: true),
                    IFSCCode = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Docuents = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    BlockedUnits = table.Column<string>(nullable: true),
                    BookingConfirmed = table.Column<string>(nullable: true),
                    RejectedUnits = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentRegistration", x => x.AgentId);
                });

            migrationBuilder.CreateTable(
                name: "AutharityData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LeadsMailTo = table.Column<string>(maxLength: 45, nullable: true),
                    LeadsMailCc = table.Column<string>(maxLength: 45, nullable: true),
                    AdminMobile = table.Column<string>(nullable: true),
                    SuperAdminMailId = table.Column<string>(nullable: true),
                    AccountMailId = table.Column<string>(nullable: true),
                    AccountMobile = table.Column<string>(nullable: true),
                    AdminMailId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutharityData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommonSetting",
                columns: table => new
                {
                    SettingID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SMTPServer = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 255, nullable: true),
                    Password = table.Column<string>(maxLength: 20, nullable: true),
                    Port = table.Column<int>(nullable: false),
                    SiteURL = table.Column<string>(maxLength: 100, nullable: true),
                    IsSSL = table.Column<byte>(nullable: false),
                    CompanyName = table.Column<string>(maxLength: 200, nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonSetting", x => x.SettingID);
                });

            migrationBuilder.CreateTable(
                name: "MailTemplate",
                columns: table => new
                {
                    TemplateID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TemplateName = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailTemplate", x => x.TemplateID);
                });

            migrationBuilder.CreateTable(
                name: "ProjectsData",
                columns: table => new
                {
                    ProjectId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectName = table.Column<string>(maxLength: 70, nullable: true),
                    status = table.Column<string>(maxLength: 20, nullable: true),
                    projectuuid = table.Column<string>(nullable: true)
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
                    Projectuuid = table.Column<string>(nullable: true),
                    status = table.Column<string>(maxLength: 20, nullable: true),
                    BookingHistory = table.Column<string>(nullable: true),
                    AgentId = table.Column<string>(nullable: true),
                    PaymentStatus = table.Column<string>(nullable: true),
                    Facing = table.Column<string>(maxLength: 60, nullable: true),
                    Mortigaze = table.Column<string>(maxLength: 60, nullable: true),
                    BlockedDate = table.Column<string>(maxLength: 60, nullable: true),
                    ExpiredDate = table.Column<string>(maxLength: 60, nullable: true),
                    StatusConfiredDate = table.Column<string>(maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUnitsData", x => x.UnitId);
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

            migrationBuilder.DropTable(
                name: "AutharityData");

            migrationBuilder.DropTable(
                name: "CommonSetting");

            migrationBuilder.DropTable(
                name: "MailTemplate");

            migrationBuilder.DropTable(
                name: "ProjectsData");

            migrationBuilder.DropTable(
                name: "ProjectUnitsData");
        }
    }
}
