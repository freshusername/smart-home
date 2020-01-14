using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.JoinModel;
using Domain.Core.Model;
using Domain.Interfaces;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.Dashboard;
using Infrastructure.Business.DTOs.DashboardOptions;
using Infrastructure.Business.Infrastructure;

namespace Infrastructure.Business.Managers
{
	public class DashboardOptionsManager : BaseManager, IDashboardOptionsManager
	{
		public DashboardOptionsManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
		{

		}

		public async Task<OperationDetails> Create(DashboardOptions dashboardOptions)
		{
			await unitOfWork.DashboardOptionsRepo.Insert(dashboardOptions);
			var res = unitOfWork.Save();

			if (res > 0)
			{
				return new OperationDetails(true, "Saved successfully", "");
			}
			return new OperationDetails(true, "Something is wrong", "");
		}

		public async Task<DashboardOptionsDto> GetByDashboardId(int id)
		{
			var dashboardOptions = await unitOfWork.DashboardOptionsRepo.GetByDashboardId(id);

			var dashboardOptionsDto = mapper.Map<DashboardOptions, DashboardOptionsDto>(dashboardOptions);

			return dashboardOptionsDto;
		}
	}
}