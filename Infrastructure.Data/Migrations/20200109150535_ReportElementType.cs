using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class ReportElementType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportElements_ReportElements_TypeId",
                table: "ReportElements");

            migrationBuilder.DropIndex(
                name: "IX_ReportElements_TypeId",
                table: "ReportElements");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "ReportElements");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "ReportElements",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "ReportElements");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "ReportElements",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReportElements_TypeId",
                table: "ReportElements",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportElements_ReportElements_TypeId",
                table: "ReportElements",
                column: "TypeId",
                principalTable: "ReportElements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
