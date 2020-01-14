using AutoMapper;
using Infrastructure.Business.DTOs.Dashboard;
using Infrastructure.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.Dashboard;
using System.Threading.Tasks;

namespace smart_home_web.Components
{
    public class DashboardViewComponent : ViewComponent
    {
		private readonly IDashboardManager _dashboardManager;
		private readonly IMapper _mapper;
		public DashboardViewComponent(IDashboardManager dashboardManager, IMapper mapper)
		{
			_dashboardManager = dashboardManager;
			_mapper = mapper;
		}

		public async Task<IViewComponentResult> InvokeAsync()
        {
			var dashboard = await _dashboardManager.GetById(1);
			var result = _mapper.Map<DashboardDto, DashboardViewModel>(dashboard);

			return View("Detail", result);
        }
    }
}
