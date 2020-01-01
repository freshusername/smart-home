using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Core.Model;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
	public class NotificationRepository : BaseRepository<Message>, INotificationRepository
    {
		private readonly ApplicationsDbContext _context;

		public NotificationRepository(ApplicationsDbContext context) : base(context)
		{
			_context = context;
		}

		public override IEnumerable<Message> GetAll()
		{
			var res = _context.Messages
				.Include(h => h.History);
			return res;
		}

		public override Message GetById(int id)
		{
			return _context.Messages.Include(h => h.Id).FirstOrDefault(s => s.Id == id);
		}

		public IEnumerable<Message> GetNotificationByHistoryId(int HistoryId)
		{
			throw new NotImplementedException();
		}
    }
}
