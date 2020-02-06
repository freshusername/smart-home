using AutoMapper;
using Domain.Core.Model;
using Infrastructure.Business.DTOs.Dashboard;
using Infrastructure.Business.Infrastructure;
using Infrastructure.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.Dashboard;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Controllers
{
    [Route("[controller]/[action]")]
    public class DashboardController : Controller
    {
        private readonly IMapper _mapper;
        private IHostingEnvironment _env;
        private IDashboardManager _dashboardManager;
        private readonly IIconManager _iconManager;
        private UserManager<AppUser> _userManager;

        public DashboardController(
            IMapper mapper,
            IHostingEnvironment env,
            IDashboardManager dashboardManager,
            IIconManager iconManager,
            UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _env = env;
            _dashboardManager = dashboardManager;
            _iconManager = iconManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Detail(int id)
        {
            var userId = _userManager.GetUserId(User);
            var dashboard = await _dashboardManager.GetById(id);
            var result = _mapper.Map<DashboardDto, DashboardViewModel>(dashboard);
            return View(result);
        }


        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            if (User.Identity.IsAuthenticated)
            {
                var uDashs = await _dashboardManager.GetByUserId(userId);
                var pDashs = await _dashboardManager.GetAllPublic(userId);

                var userDashs = _mapper.Map<IEnumerable<DashboardDto>, IEnumerable<DashboardViewModel>>(uDashs);
                var publicDashs = _mapper.Map<IEnumerable<DashboardDto>, IEnumerable<DashboardViewModel>>(pDashs);
                var result = userDashs.Union(publicDashs);

                return View(new DashboardIndexViewModel
                {
                    Dashboards = result.Reverse()
                });
            }
            else
            {
                var dashboards = await _dashboardManager.GetAllPublic(userId);
                var result = _mapper.Map<IEnumerable<DashboardDto>, IEnumerable<DashboardViewModel>>(dashboards);

                return View(new DashboardIndexViewModel
                {
                    Dashboards = result.Reverse()
                });
            }
        }

        [Authorize]
        public IActionResult Create()
        {
            var userId = _userManager.GetUserId(User);
            return ViewComponent("DashboardCreate", userId);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateDashboardViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            DashboardDto dashboardDto = _mapper.Map<CreateDashboardViewModel, DashboardDto>(model);
            if (model.IconFile != null)
                dashboardDto.IconId = await _iconManager.CreateAndGetIconId(model.IconFile);

            if(!dashboardDto.IsPublic)
                dashboardDto.AppUserId = _userManager.GetUserId(User);

            OperationDetails result = await _dashboardManager.Create(dashboardDto);
            if (result.Succeeded)
            {
                DashboardDto dashboardDtoFromDB = await _dashboardManager.GetLastDashboard();
                return ViewComponent("DashboardElement", _mapper.Map<DashboardDto, DashboardViewModel>(dashboardDtoFromDB));
            }
            else
            {
                ModelState.AddModelError(result.Property, result.Message);
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditDashboardViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            DashboardDto dashboardDto = _mapper.Map<EditDashboardViewModel, DashboardDto>(model);
            if (model.IconFile != null)
            {
                dashboardDto.IconId = await _iconManager.CreateAndGetIconId(model.IconFile);
            }
            if (dashboardDto.IsPublic)
                dashboardDto.AppUserId = null;

            try
            {
                _dashboardManager.Update(dashboardDto);
                DashboardDto dashboardDtoFromDB = await _dashboardManager.GetLastDashboard();
                return ViewComponent("DashboardElement", _mapper.Map<DashboardDto, DashboardViewModel>(dashboardDtoFromDB));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            var dashboardDto = await _dashboardManager.GetById(id);
            EditDashboardViewModel model = _mapper.Map<DashboardDto, EditDashboardViewModel>(dashboardDto);
            return ViewComponent("DashboardEdit", model);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await _dashboardManager.DeleteById(id);
            return Ok();

        }
    }
}