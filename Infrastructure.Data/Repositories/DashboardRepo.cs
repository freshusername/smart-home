using Domain.Core.Model;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Repositories
{
    class DashboardRepo : BaseRepository<Dashboard>, IDashboardRepo
    {
        public DashboardRepo(ApplicationsDbContext context) : base(context)
        {

        }
    }
}
