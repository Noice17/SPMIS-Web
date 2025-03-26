﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SPMIS_Web.Data;

#nullable disable

namespace SPMIS_Web.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SPMIS_Web.Models.Entities.Objective", b =>
                {
                    b.Property<Guid>("ObjectiveId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MapId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ObjectiveCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ObjectiveDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ObjectiveTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ObjectiveId");

                    b.HasIndex("MapId");

                    b.HasIndex("ObjectiveTypeId");

                    b.ToTable("Objectives", (string)null);
                });

            modelBuilder.Entity("SPMIS_Web.Models.Entities.ObjectiveType", b =>
                {
                    b.Property<Guid>("ObjectiveTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ObjectiveTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ObjectiveTypeId");

                    b.HasIndex("StrategyMapMapId");

                    b.ToTable("ObjectiveTypes", (string)null);
                });

            modelBuilder.Entity("SPMIS_Web.Models.Entities.StrategyMap", b =>
                {
                    b.Property<Guid>("MapId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("MapDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("MapEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("MapStart")
                        .HasColumnType("datetime2");

                    b.Property<string>("MapTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MapId");

                    b.ToTable("StrategyMaps", (string)null);
                });

            modelBuilder.Entity("SPMIS_Web.Models.Entities.Objective", b =>
                {
                    b.HasOne("SPMIS_Web.Models.Entities.StrategyMap", "Map")
                        .WithMany()
                        .HasForeignKey("MapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SPMIS_Web.Models.Entities.ObjectiveType", "Type")
                        .WithMany()
                        .HasForeignKey("ObjectiveTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Map");

                    b.Navigation("Type");
                });
#pragma warning restore 612, 618
        }
    }
}
