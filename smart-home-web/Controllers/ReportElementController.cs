using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Model;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models;
using smart_home_web.Models.ReportElements;

namespace smart_home_web.Controllers
{
    [Authorize]
    public class ReportElementController : Controller
    {
        private readonly IReportElementManager _reportElementManager;
        private readonly IMapper _mapper;

        public ReportElementController(IReportElementManager reportElementManager, IMapper mapper)
        {
            _reportElementManager = reportElementManager;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult CreateReportElement(int dashboardId)
        {
            CreateReportElementViewModel model = new CreateReportElementViewModel
            {
                DashboardId = dashboardId
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReportElement(CreateReportElementViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            ReportElementDto reportElement = _mapper.Map<CreateReportElementViewModel, ReportElementDto>(model);
            await _reportElementManager.CreateReportElement(reportElement);
            return RedirectToAction("Detail", "Dashboard", new { id = model.DashboardId});
        }

        [HttpGet]
        public async Task<IActionResult> EditReportElement(int id)
        {
            ReportElement reportElement = await _reportElementManager.GetById(id);
            if (reportElement == null)
                return NotFound();
            EditReportElementViewModel model = _mapper.Map<ReportElement, EditReportElementViewModel>(reportElement);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditReportElement(EditReportElementViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            ReportElementDto reportElement = _mapper.Map<EditReportElementViewModel, ReportElementDto>(model);
            await _reportElementManager.EditReportElement(reportElement);
            return RedirectToAction("Detail", "Dashboard", new { id = model.DashboardId });
        }

		[HttpPost]
		public async Task<IActionResult> DeleteReportElement(int id)
		{
			var reportElement = await _reportElementManager.GetById(id);
			await _reportElementManager.Delete(reportElement);
			return Ok();
		}
	}
}