using Domain.Core.Model;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
	public class OptionsRepo : BaseRepository<Options>, IOptionsRepo
	{
		public OptionsRepo(ApplicationsDbContext context) : base(context)
		{

		}
	}
}
