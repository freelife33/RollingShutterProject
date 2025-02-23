using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RollingShutterProject.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSensorDataModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Value",
                table: "SensorData",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "SensorData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeviceId",
                table: "SensorData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SensorData_DeviceId",
                table: "SensorData",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_SensorData_Devices_DeviceId",
                table: "SensorData",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorData_Devices_DeviceId",
                table: "SensorData");

            migrationBuilder.DropIndex(
                name: "IX_SensorData_DeviceId",
                table: "SensorData");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "SensorData");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "SensorData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "SensorData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
