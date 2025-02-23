using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RollingShutterProject.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSensorDataModel1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "SensorData",
                newName: "SensorType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SensorType",
                table: "SensorData",
                newName: "Type");
        }
    }
}
