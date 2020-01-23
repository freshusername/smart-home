using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Message : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE PROCEDURE `GetAvgValuesForSensor`(
				IN sensorId INT,
                IN dateFrom DateTime,
                IN dateTo DateTime)
                BEGIN
	                SELECT DATE(Date) as WeekDay, CAST(avg(COALESCE(Histories.IntValue, Histories.DoubleValue)) as DECIMAL(10, 2)) as AvgValue
		                FROM Histories
		                WHERE (Histories.Date BETWEEN dateFrom AND dateTo)
						AND Histories.SensorId = sensorId
                        GROUP BY DATE(Histories.Date);
                END";

            migrationBuilder.Sql(sp);

            migrationBuilder.AlterColumn<string>(
                name: "MeasurementType",
                table: "SensorTypes",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Notifications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "Notifications");

            migrationBuilder.AlterColumn<string>(
                name: "MeasurementType",
                table: "SensorTypes",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
