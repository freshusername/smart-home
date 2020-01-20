using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Model;
using Domain.Interfaces.Repositories;
using Infrastructure.Business.DTOs.Dashboard;
using Infrastructure.Business.Infrastructure;
using System.Linq;
using System;

namespace Infrastructure.Business.Managers
{
	public class DashboardManager : BaseManager, IDashboardManager
	{
		public DashboardManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
		{

		}

		public async Task<OperationDetails> Create(DashboardDto dashboardDto)
		{
			var dashboard = mapper.Map<DashboardDto, Dashboard> (dashboardDto);
			await unitOfWork.DashboardRepo.Insert(dashboard);
			var res = unitOfWork.Save();

			if (res > 0)
			{
				return new OperationDetails(true, "Saved successfully", "");
			}
			return new OperationDetails(true, "Something is wrong", "");
		}

		public async Task<IEnumerable<DashboardDto>> GetAll()
		{			
			var dashboards = await unitOfWork.DashboardRepo.GetAll();
			return mapper.Map<IEnumerable<Dashboard>, IEnumerable<DashboardDto>>(dashboards);
		}

		public async Task<IEnumerable<DashboardDto>> GetByUserId(string userId)
		{
			var dashboards = await unitOfWork.DashboardRepo.GetAll();
			dashboards = dashboards.Where(d => d.AppUserId == userId);
			return mapper.Map<IEnumerable<Dashboard>, IEnumerable<DashboardDto>>(dashboards);
		}

		public async Task<DashboardDto> GetById(int id)
		{
			var dashboard = await unitOfWork.DashboardRepo.GetById(id);
			var result = mapper.Map<Dashboard, DashboardDto>(dashboard);

			return result;
		}
	}
}
