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
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> EditWordCloud(int id)
        {
            ReportElement reportElement = await _reportElementManager.GetById(id);
            if (reportElement == null)
                return NotFound();
            EditReportElementViewModel wordCloud = _mapper.Map<ReportElement, EditReportElementViewModel>(reportElement);
            return View(wordCloud);
        }

        [HttpPost]
        public IActionResult EditWordCloud(EditReportElementViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            ReportElementDTO wordCloud = _mapper.Map<EditReportElementViewModel, ReportElementDTO>(model);
            _reportElementManager.EditReportElement(wordCloud);
            return RedirectToAction("Index","Home");
            //return RedirectToAction("Index","Home", new { DashboardId = reportElement.DashboardId});
        }

        [HttpGet("GetGauge/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            GaugeDto gaugeDto = await _reportElementManager.GetGaugeById(id);
            GaugeUpdateViewModel result = _mapper.Map<GaugeDto, GaugeUpdateViewModel>(gaugeDto);

            return Ok(result);
        }
    }
}