using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class spGetAvgValesForSensor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE PROCEDURE SensorValuesForTimePeriod
            (IN sensorId INT,
            IN dateFrom DateTime,
            IN dateTo DateTime)
            BEGIN
	            SELECT Date,AVG(IntValue)
		            FROM Histories
		            WHERE Date BETWEEN dateFrom AND dateTo
		            AND SensorId = sensorId;
            END;";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
