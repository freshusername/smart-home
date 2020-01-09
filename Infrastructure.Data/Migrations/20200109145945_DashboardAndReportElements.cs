using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class DashboardAndReportElements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dashboards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    AppUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dashboards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dashboards_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReportElements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TypeId = table.Column<int>(nullable: true),
                    DashboardId = table.Column<int>(nullable: false),
                    SensorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportElements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportElements_Dashboards_DashboardId",
                        column: x => x.DashboardId,
                        principalTable: "Dashboards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportElements_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportElements_ReportElements_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ReportElements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dashboards_AppUserId",
                table: "Dashboards",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportElements_DashboardId",
                table: "ReportElements",
                column: "DashboardId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportElements_SensorId",
                table: "ReportElements",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportElements_TypeId",
                table: "ReportElements",
                column: "TypeId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Sensors_SensorTypes_SensorTypeId",
            //    table: "Sensors",
            //    column: "SensorTypeId",
            //    principalTable: "SensorTypes",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_SensorTypes_SensorTypeId",
                table: "Sensors");

            migrationBuilder.DropTable(
                name: "ReportElements");

            migrationBuilder.DropTable(
                name: "Dashboards");

            migrationBuilder.DropColumn(
                name: "MeasurementType",
                table: "SensorTypes");

            migrationBuilder.RenameColumn(
                name: "MeasurementName",
                table: "SensorTypes",
                newName: "MeasurmentType");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Icons",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "MeasurmentName",
                table: "SensorTypes",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SensorTypeId",
                table: "Sensors",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_SensorTypes_SensorTypeId",
                table: "Sensors",
                column: "SensorTypeId",
                principalTable: "SensorTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
