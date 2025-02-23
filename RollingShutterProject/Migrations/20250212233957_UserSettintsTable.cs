using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RollingShutterProject.Migrations
{
    /// <inheritdoc />
    public partial class UserSettintsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoggingIntervalHours = table.Column<int>(type: "int", nullable: false),
                    DetectAnomalies = table.Column<bool>(type: "bit", nullable: false),
                    NotifyOnHighTemperature = table.Column<bool>(type: "bit", nullable: false),
                    NotifyOnPoorAirQuality = table.Column<bool>(type: "bit", nullable: false),
                    AutoOpenShutter = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSettings");
        }
    }
}
