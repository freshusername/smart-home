using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain.Core.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Domain.Core.Model.Enums;
using Domain.Core.CalculateModel;

namespace Infrastructure.Data
{
    public class ApplicationsDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Icon> Icons { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<SensorType> SensorTypes { get; set; }
        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<ReportElement> ReportElements { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Control> Controls { get; set; }
        public DbSet<SensorControl> SensorControls { get; set; }
        public DbQuery<AvgSensorValuePerDay> AvgSensorValuesPerDays { get; set; }

        public ApplicationsDbContext(DbContextOptions<ApplicationsDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder
            .Entity<SensorType>()
            .Property(e => e.MeasurementType)
            .HasConversion(
            v => v.ToString(),
            v => (MeasurementType)Enum.Parse(typeof(MeasurementType), v));

            modelBuilder
            .Entity<ReportElement>()
            .Property(e => e.Type)
            .HasConversion(
            v => v.ToString(),
            v => (ReportElementType)Enum.Parse(typeof(ReportElementType), v));
        }
    }
}
