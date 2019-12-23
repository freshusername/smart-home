using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain.Core.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Domain.Core.Model.Enums;

namespace Infrastructure.Data
{
	public class ApplicationsDbContext : IdentityDbContext<AppUser> // TODO: Use Identity DB context
	{
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<SensorType> SensorTypes { get; set; }

        public ApplicationsDbContext(DbContextOptions<ApplicationsDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder
            .Entity<SensorType>()
            .Property(e => e.Mv)
            .HasConversion(
            v => v.ToString(),
            v => (Mv)Enum.Parse(typeof(Mv), v));
        }
    }
}
