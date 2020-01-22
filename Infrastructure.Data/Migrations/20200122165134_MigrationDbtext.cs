using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class MigrationDbtext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "SensorControls");

            migrationBuilder.AddColumn<int>(
                name: "IconId",
                table: "SensorControls",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "SensorControls",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "maxValue",
                table: "SensorControls",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "minValue",
                table: "SensorControls",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SensorControls_IconId",
                table: "SensorControls",
                column: "IconId");

            migrationBuilder.AddForeignKey(
                name: "FK_SensorControls_Icons_IconId",
                table: "SensorControls",
                column: "IconId",
                principalTable: "Icons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorControls_Icons_IconId",
                table: "SensorControls");

            migrationBuilder.DropIndex(
                name: "IX_SensorControls_IconId",
                table: "SensorControls");

            migrationBuilder.DropColumn(
                name: "IconId",
                table: "SensorControls");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "SensorControls");

            migrationBuilder.DropColumn(
                name: "maxValue",
                table: "SensorControls");

            migrationBuilder.DropColumn(
                name: "minValue",
                table: "SensorControls");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "SensorControls",
                nullable: false,
                defaultValue: "");
        }
    }
}
