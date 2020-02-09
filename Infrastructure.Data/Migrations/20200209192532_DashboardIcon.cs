using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class DashboardIcon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IconId",
                table: "Dashboards",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dashboards_IconId",
                table: "Dashboards",
                column: "IconId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dashboards_Icons_IconId",
                table: "Dashboards",
                column: "IconId",
                principalTable: "Icons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dashboards_Icons_IconId",
                table: "Dashboards");

            migrationBuilder.DropIndex(
                name: "IX_Dashboards_IconId",
                table: "Dashboards");

            migrationBuilder.DropColumn(
                name: "IconId",
                table: "Dashboards");
        }
    }
}
