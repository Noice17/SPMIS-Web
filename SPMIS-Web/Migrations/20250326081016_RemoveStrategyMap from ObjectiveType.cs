using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPMIS_Web.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStrategyMapfromObjectiveType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ObjectiveTypes_StrategyMaps_StrategyMapMapId",
                table: "ObjectiveTypes");

            migrationBuilder.DropIndex(
                name: "IX_ObjectiveTypes_StrategyMapMapId",
                table: "ObjectiveTypes");

            migrationBuilder.DropColumn(
                name: "StrategyMapMapId",
                table: "ObjectiveTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StrategyMapMapId",
                table: "ObjectiveTypes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ObjectiveTypes_StrategyMapMapId",
                table: "ObjectiveTypes",
                column: "StrategyMapMapId");

            migrationBuilder.AddForeignKey(
                name: "FK_ObjectiveTypes_StrategyMaps_StrategyMapMapId",
                table: "ObjectiveTypes",
                column: "StrategyMapMapId",
                principalTable: "StrategyMaps",
                principalColumn: "MapId");
        }
    }
}
