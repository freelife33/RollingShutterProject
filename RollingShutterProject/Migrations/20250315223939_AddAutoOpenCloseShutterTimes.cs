using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RollingShutterProject.Migrations
{
    /// <inheritdoc />
    public partial class AddAutoOpenCloseShutterTimes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AtoCloseShutterOnTime",
                table: "UserSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AutoOpenShutterOnTime",
                table: "UserSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "CloseTime",
                table: "UserSettings",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "OpenTime",
                table: "UserSettings",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AtoCloseShutterOnTime",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "AutoOpenShutterOnTime",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "CloseTime",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "OpenTime",
                table: "UserSettings");
        }
    }
}
