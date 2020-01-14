using Domain.Core.JoinModel;
using Domain.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IDashboardOptionsRepo : IGenericRepository<DashboardOptions>
    {
		Task<DashboardOptions> GetByDashboardId(int id);

	}
}
