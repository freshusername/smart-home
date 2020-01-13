using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Model;
using Domain.Interfaces;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.Dashboard;
using Infrastructure.Business.Infrastructure;

namespace Infrastructure.Business.Managers
{
	public class DashboardManager : BaseManager, IDashboardManager
	{
		public DashboardManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
		{

		}

		public async Task<DashboardDto> GetById(int id)
		{
			var dashboard = await unitOfWork.DashboardRepo.GetById(id);
			var result = mapper.Map<Dashboard, DashboardDto>(dashboard);

			return result;
		}
	}
}
