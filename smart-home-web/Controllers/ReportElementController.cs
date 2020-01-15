using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Model;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models;
using smart_home_web.Models.ReportElements;

namespace smart_home_web.Controllers
{
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
        public IActionResult CreateReportElement(CreateReportElementViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            return View(model);
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
        public IActionResult EditReportElement(EditReportElementViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            ReportElementDto reportElement = _mapper.Map<EditReportElementViewModel, ReportElementDto>(model);
            _reportElementManager.EditReportElement(reportElement);
            return RedirectToAction("Index","Home");
            //return RedirectToAction("Index","Home", new { DashboardId = reportElement.DashboardId});
        }
	}
}