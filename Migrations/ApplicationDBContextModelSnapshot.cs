﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SarkPayOuts.Models.DbModels;

namespace SarkPayOuts.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    partial class ApplicationDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("SarkPayOuts.Models.DbModels.AdminDetails", b =>
                {
                    b.Property<int>("AdminId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Mobile")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("AdminId");

                    b.ToTable("AdminDetails");
                });

            modelBuilder.Entity("SarkPayOuts.Models.DbModels.AgentPayOuts", b =>
                {
                    b.Property<string>("LeadUUID")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("AgentId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("CreationDate")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("CustomerEmail")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("CustomerName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<decimal?>("GstAmount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal?>("GstPercentage")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("InvoiceDate")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("InvoiceNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("LeadHistory")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PayOutStatus")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<decimal?>("PayoutAmount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal?>("PayoutPercentage")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("ProjectName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<decimal?>("TdsAmount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal?>("TdsPercentage")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("UnitNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UnitSize")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("LeadUUID");

                    b.ToTable("AgentPayOuts");
                });

            modelBuilder.Entity("SarkPayOuts.Models.DbModels.AgentRegistration", b =>
                {
                    b.Property<string>("AgentId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Aadhar")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("AgetName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("BankAccountNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("CreatedDate")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Docuents")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("IFSCCode")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Mobile")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PAN")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ParentAgentID")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Password")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("AgentId");

                    b.ToTable("AgentRegistration");
                });

            modelBuilder.Entity("SarkPayOuts.Models.DbModels.ProjectUnitsData", b =>
                {
                    b.Property<int>("UnitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AdminId")
                        .HasColumnType("int");

                    b.Property<string>("BlockedDate")
                        .HasColumnType("varchar(60) CHARACTER SET utf8mb4")
                        .HasMaxLength(60);

                    b.Property<string>("BookingHistory")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ExpiredDate")
                        .HasColumnType("varchar(60) CHARACTER SET utf8mb4")
                        .HasMaxLength(60);

                    b.Property<string>("Facing")
                        .HasColumnType("varchar(60) CHARACTER SET utf8mb4")
                        .HasMaxLength(60);

                    b.Property<string>("Mortigaze")
                        .HasColumnType("varchar(60) CHARACTER SET utf8mb4")
                        .HasMaxLength(60);

                    b.Property<string>("PaymentStatus")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("UnitNumber")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<string>("UnitSize")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<string>("status")
                        .HasColumnType("varchar(20) CHARACTER SET utf8mb4")
                        .HasMaxLength(20);

                    b.HasKey("UnitId");

                    b.ToTable("ProjectUnitsData");
                });

            modelBuilder.Entity("SarkPayOuts.Models.DbModels.ProjectsData", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ProjectName")
                        .HasColumnType("varchar(70) CHARACTER SET utf8mb4")
                        .HasMaxLength(70);

                    b.Property<string>("status")
                        .HasColumnType("varchar(20) CHARACTER SET utf8mb4")
                        .HasMaxLength(20);

                    b.HasKey("ProjectId");

                    b.ToTable("ProjectsData");
                });
#pragma warning restore 612, 618
        }
    }
}
