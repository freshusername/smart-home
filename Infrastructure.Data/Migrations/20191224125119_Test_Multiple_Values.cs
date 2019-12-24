using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Test_Multiple_Values : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "Histories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    StringValue = table.Column<string>(nullable: true),
                    IntValue = table.Column<int>(nullable: false),
                    DoubleValue = table.Column<double>(nullable: false),
                    BoolValue = table.Column<bool>(nullable: false),
                    SensorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Histories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Histories_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SensorTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    Mv = table.Column<string>(nullable: false),
                    Mn = table.Column<string>(nullable: true),
                    Icon = table.Column<byte[]>(nullable: true),
                    SensorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorTypes_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Comment = table.Column<string>(nullable: true),
                    IsRead = table.Column<bool>(nullable: false),
                    HistoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });


            migrationBuilder.CreateIndex(
                name: "IX_Histories_SensorId",
                table: "Histories",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_HistoryId",
                table: "Messages",
                column: "HistoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SensorTypes_SensorId",
                table: "SensorTypes",
                column: "SensorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

	        migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "SensorTypes");


            migrationBuilder.DropTable(
                name: "Histories");

        }
    }
}
