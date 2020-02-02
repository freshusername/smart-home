using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class nullableSensorId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportElements_Sensors_SensorId",
                table: "ReportElements");

            migrationBuilder.AlterColumn<int>(
                name: "SensorId",
                table: "ReportElements",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ReportElements_Sensors_SensorId",
                table: "ReportElements",
                column: "SensorId",
                principalTable: "Sensors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportElements_Sensors_SensorId",
                table: "ReportElements");

            migrationBuilder.AlterColumn<int>(
                name: "SensorId",
                table: "ReportElements",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportElements_Sensors_SensorId",
                table: "ReportElements",
                column: "SensorId",
                principalTable: "Sensors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
