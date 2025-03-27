﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPMIS_Web.Migrations
{
    /// <inheritdoc />
    public partial class objectiveTypeIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ObjectiveTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ObjectiveTypes");
        }
    }
}
