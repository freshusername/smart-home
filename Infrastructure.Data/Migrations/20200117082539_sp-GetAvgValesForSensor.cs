﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class spGetAvgValesForSensor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE PROCEDURE `GetAvgValuesForSensor`(
				IN sensorId INT,
                IN dateFrom DateTime,
                IN dateTo DateTime)
                BEGIN
	                SELECT Date as WeekDay, CAST(avg(COALESCE(Histories.IntValue, Histories.DoubleValue)) as DECIMAL(10, 2)) as AvgValue
		                FROM Histories
		                WHERE (Histories.Date BETWEEN dateFrom AND dateTo)
						AND Histories.SensorId = sensorId
                        GROUP BY Histories.Date;
                END";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}