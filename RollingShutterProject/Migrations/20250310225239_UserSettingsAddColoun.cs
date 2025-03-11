using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RollingShutterProject.Migrations
{
    /// <inheritdoc />
    public partial class UserSettingsAddColoun : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AutoOpenShutterOnHighTemperature",
                table: "UserSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AutoOpenShutterOnPoorAirQuality",
                table: "UserSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<float>(
                name: "HighTemperatureThreshold",
                table: "UserSettings",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "PoorAirQualityThreshold",
                table: "UserSettings",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoOpenShutterOnHighTemperature",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "AutoOpenShutterOnPoorAirQuality",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "HighTemperatureThreshold",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "PoorAirQualityThreshold",
                table: "UserSettings");
        }
    }
}
