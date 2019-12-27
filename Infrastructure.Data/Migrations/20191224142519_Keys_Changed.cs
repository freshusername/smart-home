using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Keys_Changed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	     
            migrationBuilder.AddColumn<string>(
                name: "MeasurmentType",
                table: "SensorTypes",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "SensorTypeId",
                table: "Sensors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_SensorTypeId",
                table: "Sensors",
                column: "SensorTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_SensorTypes_SensorTypeId",
                table: "Sensors",
                column: "SensorTypeId",
                principalTable: "SensorTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DropIndex(
                name: "IX_Sensors_SensorTypeId",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "MeasurmentType",
                table: "SensorTypes");

            migrationBuilder.DropColumn(
                name: "SensorTypeId",
                table: "Sensors");

            migrationBuilder.RenameColumn(
                name: "MeasurmentName",
                table: "SensorTypes",
                newName: "Mn");

            migrationBuilder.AddColumn<string>(
                name: "Mv",
                table: "SensorTypes",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SensorId",
                table: "SensorTypes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SensorTypes_SensorId",
                table: "SensorTypes",
                column: "SensorId");

        }
    }
}
