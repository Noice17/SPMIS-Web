using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPMIS_Web.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ObjectiveTypes",
                columns: table => new
                {
                    ObjectiveTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ObjectiveTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectiveTypes", x => x.ObjectiveTypeId);
                });

            migrationBuilder.CreateTable(
                name: "StrategyMaps",
                columns: table => new
                {
                    MapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MapDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MapTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MapStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MapEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StrategyMaps", x => x.MapId);
                });

            migrationBuilder.CreateTable(
                name: "Objectives",
                columns: table => new
                {
                    ObjectiveId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ObjectiveDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ObjectiveTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objectives", x => x.ObjectiveId);
                    table.ForeignKey(
                        name: "FK_Objectives_ObjectiveTypes_ObjectiveTypeId",
                        column: x => x.ObjectiveTypeId,
                        principalTable: "ObjectiveTypes",
                        principalColumn: "ObjectiveTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Objectives_StrategyMaps_MapId",
                        column: x => x.MapId,
                        principalTable: "StrategyMaps",
                        principalColumn: "MapId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Objectives_MapId",
                table: "Objectives",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_Objectives_ObjectiveTypeId",
                table: "Objectives",
                column: "ObjectiveTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Objectives");

            migrationBuilder.DropTable(
                name: "ObjectiveTypes");

            migrationBuilder.DropTable(
                name: "StrategyMaps");
        }
    }
}
