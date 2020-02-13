using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class addedimagecolumforuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "IconId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IconId",
                table: "AspNetUsers",
                column: "IconId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Icons_IconId",
                table: "AspNetUsers",
                column: "IconId",
                principalTable: "Icons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Icons_IconId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_IconId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IconId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Messages",
                nullable: true);
        }
    }
}
