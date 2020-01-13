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
using smart_home_web.Models.WordCloud;

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
        public async Task<IActionResult> EditWordCloud(int id)
        {
            ReportElement reportElement = await _reportElementManager.GetById(id);
            if (reportElement == null)
                return NotFound();
            EditWordCloudViewModel wordCloud = _mapper.Map<ReportElement, EditWordCloudViewModel>(reportElement);
            return View(wordCloud);
        }

        [HttpPost]
        public IActionResult EditWordCloud(EditWordCloudViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            WordCloudDTO wordCloud = _mapper.Map<EditWordCloudViewModel, WordCloudDTO>(model);
            _reportElementManager.EditWordCloud(wordCloud);
            return RedirectToAction("Index","Home");
            //return RedirectToAction("Index","Home", new { DashboardId = reportElement.DashboardId});
        }
    }
}