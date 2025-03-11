using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPMIS_Web.Migrations
{
    /// <inheritdoc />
    public partial class Adjust : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "StrategyMaps",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectiveCode",
                table: "Objectives",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "StrategyMaps");

            migrationBuilder.DropColumn(
                name: "ObjectiveCode",
                table: "Objectives");
        }
    }
}
