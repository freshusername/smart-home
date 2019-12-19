using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain.Core.Model;

namespace Infrastructure.Data
{
	public class ApplicationsDbContext : DbContext // TODO: Use Identity DB context
	{
		DbSet<Sensor> Sensors { get; set; }

		public ApplicationsDbContext()
		{
			Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
		}
	}
}
