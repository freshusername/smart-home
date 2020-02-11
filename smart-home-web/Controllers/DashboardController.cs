using AutoMapper;
using Domain.Core.Model;
using Infrastructure.Business.DTOs.Dashboard;
using Infrastructure.Business.Infrastructure;
using Infrastructure.Business.Interfaces;
using Infrastructure.Business.Managers;
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
            ViewBag.userid = userId;
            var dashboard = await _dashboardManager.GetById(id);
            if (dashboard != null)
            {
                var result = _mapper.Map<DashboardDto, DashboardViewModel>(dashboard);
                return View(result);
            }
            return NotFound("The dashboard is not found!");
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
        public IActionResult Create() => ViewComponent("DashboardCreate");

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

            dashboardDto.AppUserId = _userManager.GetUserId(User);

            var res = _dashboardManager.Create(dashboardDto).Result;

            if (res != null)
            {
                DashboardViewModel dashmodel = _mapper.Map<DashboardDto, DashboardViewModel>(res);
                dashmodel.DashCreatorUserName = User.Claims.ElementAt(1).Value;
                return ViewComponent("DashboardElement", _mapper.Map<DashboardDto, DashboardViewModel>(res));
            }
            else
            {
                //ModelState.AddModelError(res.Property, res.Message);
                return View(model);
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

            var res = await _dashboardManager.Update(dashboardDto);

            if (res != null)
            {
                return ViewComponent("DashboardElement", _mapper.Map<DashboardDto, DashboardViewModel>(res));
            }
            else
            {
                //ModelState.AddModelError(res.Property, res.Message);
                return View(model);
            }
        }

        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            var dashboardDto = await _dashboardManager.GetById(id);
            if (dashboardDto != null)
            {
                EditDashboardViewModel model = _mapper.Map<DashboardDto, EditDashboardViewModel>(dashboardDto);
                return ViewComponent("DashboardEdit", model);
            }
            return NotFound("The dashboard is not found!");
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var res = await _dashboardManager.Delete(id);
                if (!res.Succeeded)
                {
                    ModelState.AddModelError(res.Property, res.Message);
                    return View();
                }

                return Ok();
            }
            catch
            {
                return View();
            }
        }
    }
}