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
	}
}
