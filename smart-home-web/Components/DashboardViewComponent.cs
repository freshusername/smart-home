using AutoMapper;
using Infrastructure.Business.DTOs.Dashboard;
using Infrastructure.Business.Managers;
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

		public async Task<IViewComponentResult> InvokeAsync(int dashboardId)
        {
			var dashboard = await _dashboardManager.GetById(dashboardId);
			var result = _mapper.Map<DashboardDto, DashboardViewModel>(dashboard);

			return View("Detail", dashboard);
        }
    }
}
