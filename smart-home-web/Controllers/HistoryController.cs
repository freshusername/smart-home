using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.History;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;
using smart_home_web.Models;
using smart_home_web.Models.History;

namespace smart_home_web.Controllers
{
    public class HistoryController : Controller
    {
        private readonly IHistoryTestManager _historyTestManager;
        private readonly IMapper _mapper;

        public HistoryController(IHistoryTestManager historyTestManager, IMapper mapper)
        {
            _historyTestManager = historyTestManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var histories = await _historyTestManager.GetAllHistoriesAsync();
            var models = _mapper.Map<IEnumerable<HistoryDto>, IEnumerable<HistoryViewModel>>(histories);

            return View(new AllHistoriesViewModel
            {
                Histories = models
            });
        }

        public async Task<IActionResult> Detail(int id)
        {
            var history = await _historyTestManager.GetHistoryByIdAsync(id);

            return View(_mapper.Map<HistoryDto, HistoryViewModel>(history));
        }

        [HttpGet]
        public IActionResult Graph(int sensorId, int days = 30)
        {
            GraphDTO graph = _historyTestManager.GetGraphBySensorId(sensorId, days);
            GraphViewModel result = _mapper.Map<GraphDTO, GraphViewModel>(graph);
            if (result.IsCorrect)
            {
                result.Days = days;
                string specifier = "G";
                result.StringDates = new List<string>();
                foreach (DateTimeOffset date in graph.Dates)
                {
                    result.StringDates.Add(date.ToString(specifier));
                }
            }
            return View(result);
        }

        [HttpPost]
        public IActionResult Graph(GraphViewModel model)
        {
            return RedirectToAction("Graph", new { sensorId = model.SensorId, days = model.Days == 0 ? 30 : model.Days });
        }
    }
}