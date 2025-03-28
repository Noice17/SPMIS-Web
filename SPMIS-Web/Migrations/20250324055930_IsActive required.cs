﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPMIS_Web.Migrations
{
    /// <inheritdoc />
    public partial class IsActiverequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "StrategyMaps",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "StrategyMaps",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
