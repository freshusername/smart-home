using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class NotifValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoolValue",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "DoubleValue",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "IntValue",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "StringValue",
                table: "Notifications",
                newName: "Value");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Notifications",
                newName: "StringValue");

            migrationBuilder.AddColumn<bool>(
                name: "BoolValue",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DoubleValue",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IntValue",
                table: "Notifications",
                nullable: true);
        }
    }
}
