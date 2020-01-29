using AutoMapper;
using Domain.Core.Model;
using Infrastructure.Business.DTOs.Dashboard;
using Infrastructure.Business.Infrastructure;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        private UserManager<AppUser> _userManager;
        private readonly IIconManager _iconManager;

        public DashboardController(
            IMapper mapper,
            IHostingEnvironment env,
            IDashboardManager dashboardManager,
            UserManager<AppUser> userManager,
            IIconManager iconManager)
        {
            _mapper = mapper;
            _env = env;
            _dashboardManager = dashboardManager;
            _userManager = userManager;
            _iconManager = iconManager;
        }

        public async Task<IActionResult> Detail(int id)
        {
            var userId = _userManager.GetUserId(User);
            var dashboard = await _dashboardManager.GetById(id);
            var result = _mapper.Map<DashboardDto, DashboardViewModel>(dashboard);
            return View(result);
        }


        public async Task<IActionResult> Index(int id)
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

                if (result.Count() == 0)
                {
                    return View(new DashboardIndexViewModel
                    {
                        Dashboards = result.Reverse()
                    });
                }
                else
                {
                    return View(new DashboardIndexViewModel
                    {
                        Dashboards = result.Reverse()
                    });
                }
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(string name, bool isPublic, IFormFile IconFile)
        {
            DashboardDto dashboardDto = new DashboardDto()
            {
                Name = name,
                AppUserId = _userManager.GetUserId(User),
                IsPublic = isPublic,
                IconId = await _iconManager.CreateAndGetIconId(IconFile)
            };

            OperationDetails result = await _dashboardManager.Create(dashboardDto);
            if (result.Succeeded)
            {
                var dashboardDtos = await _dashboardManager.GetAll();
                var dashboard = _mapper.Map<DashboardDto, DashboardViewModel>(dashboardDtos.Last());
                return ViewComponent("Dashboard", new { model = dashboard });
            }
            else
            {
                ModelState.AddModelError(result.Property, result.Message);
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, string name)
        {
            await _dashboardManager.Update(id, name);
            return Ok();
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await _dashboardManager.DeleteById(id);
            return Ok();

        }
    }
}