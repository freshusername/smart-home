using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Model.Enums;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.History;
using Infrastructure.Business.Managers;
using Infrastructure.Business.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;
using smart_home_web.Models;
using smart_home_web.Models.History;
using Domain.Core.Model;

namespace smart_home_web.Controllers
{
	public class HistoryController : Controller
	{
		private readonly IHistoryTestManager _historyTestManager;
		private readonly IMapper _mapper;
        private readonly IInvalidSensorManager _invalidSensorManager;

		public HistoryController(IHistoryTestManager historyTestManager, IMapper mapper,IInvalidSensorManager invalidSensorManager)
		{
			_historyTestManager = historyTestManager;
			_mapper = mapper;
            _invalidSensorManager = invalidSensorManager;
		}

        public async Task<IActionResult> Index(FilterDTO FilterDTO)
		{
            if (FilterDTO.sortState == SortState.None) FilterDTO.sortState = SortState.HistoryAsc;

            var histories = await _historyTestManager.GetAllHistoriesAsync();

            histories = SortValue.SortHistories(FilterDTO.sortState, histories);

            var models = _mapper.Map<IEnumerable<HistoryDto>, IEnumerable<HistoryViewModel>>(histories);
            
            FilterDTO.Amount = histories.Count();
            histories = histories.Skip((FilterDTO.CurrentPage - 1) * FilterDTO.PageSize).Take(FilterDTO.PageSize).ToList();
            IEnumerable<HistoryViewModel> historiesViewModel = _mapper.Map<IEnumerable<HistoryDto>, IEnumerable<HistoryViewModel>>(histories);
            return View(new AllHistoriesViewModel
            {
                Histories = historiesViewModel,
                FilterDTO = FilterDTO
            });
		}

		public async Task<IActionResult> Detail(FilterDTO FilterDTO)
		{
            if (FilterDTO.sortState == SortState.None) FilterDTO.sortState = SortState.HistoryAsc;

            var histories = await _historyTestManager.GetHistoriesBySensorIdAsync(FilterDTO.sensorId);

            histories = SortValue.SortHistories(FilterDTO.sortState, histories);
                  
            var result = _mapper.Map<IEnumerable<HistoryDto>, IEnumerable<HistoryViewModel>>(histories);

            return View(new AllHistoriesViewModel
			{
				Histories = result,
                FilterDTO = FilterDTO
			});
		}

		#region InvalidSensors

		public async Task<IActionResult> InvalidSensors(FilterDTO filterDTO)
		{
	        if (filterDTO.sortState == SortState.None) filterDTO.sortState = SortState.SensorAsc;

			IEnumerable<HistoryDto> histories = await _invalidSensorManager.getInvalidSensors(filterDTO.sortState);

            filterDTO.Amount = histories.Count();
            histories = histories.Skip((filterDTO.CurrentPage - 1) * filterDTO.PageSize).Take(filterDTO.PageSize).ToList();
            IEnumerable<HistoryViewModel> historiesViewModel = _mapper.Map<IEnumerable<HistoryDto>, IEnumerable<HistoryViewModel>>(histories);
            return View(new InvalidSensorsViewModel
            {
                Histories=historiesViewModel,
                FilterDTO =filterDTO
            });
        }

		#endregion

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