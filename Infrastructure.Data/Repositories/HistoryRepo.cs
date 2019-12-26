using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Core.Model;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
	public class HistoryRepo : BaseRepository<History>
	{
		private readonly ApplicationsDbContext _context;

		public HistoryRepo(ApplicationsDbContext context) : base(context)
		{
			_context = context;
		}

		public new IEnumerable<History> GetAll()
		{
			return _context.Histories.Include(h => h.Sensor).ToList();
		}

		public new History GetById(int id)
		{
			return _context.Histories.Include(h => h.Sensor).FirstOrDefault(s => s.Id == id);
		}
	}
}
