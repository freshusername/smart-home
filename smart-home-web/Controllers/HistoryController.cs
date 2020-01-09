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
using Infrastructure.Business.Services;

namespace smart_home_web.Controllers
{
      
    public class HistoryController : Controller
	{
		private readonly IHistoryManager _historyManager;
		private readonly IMapper _mapper;
        private readonly IInvalidSensorManager _invalidSensorManager;

		public HistoryController(IHistoryManager historyManager, IMapper mapper,IInvalidSensorManager invalidSensorManager)
		{
			_historyManager = historyManager;
			_mapper = mapper;
            _invalidSensorManager = invalidSensorManager;
		}

		public async Task<IActionResult> Index(FilterDTO FilterDTO)
		{
            var histories = await _historyManager.GetHistoriesAsync(FilterDTO.PageSize, FilterDTO.CurrentPage, FilterDTO.sortState);
			
            FilterDTO.Amount = await _historyManager.GetAmountAsync();

			//TODO: Replace mapper to service
            var historiesViewModel = _mapper.Map<IEnumerable<HistoryDto>, IEnumerable<HistoryViewModel>>(histories);
            return View(new AllHistoriesViewModel
            {
                Histories = historiesViewModel,
                FilterDTO = FilterDTO
            });
		}

		public async Task<IActionResult> Detail(FilterDTO FilterDTO)
		{
			var histories = await _historyManager.GetHistoriesAsync(FilterDTO.PageSize, FilterDTO.CurrentPage, FilterDTO.sortState, FilterDTO.sensorId);
			
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
            return View(new AllHistoriesViewModel
            {
                Histories=historiesViewModel,
                FilterDTO =filterDTO
            });
        }

        #endregion

        [HttpGet]
		public async Task<IActionResult> Graph(int sensorId, int days = 30)
		{
			GraphDTO graph = await _historyManager.GetGraphBySensorId(sensorId, days);
			GraphViewModel result = _mapper.Map<GraphDTO, GraphViewModel>(graph);
			if (result.IsCorrect)
			{
				result.Days = days;
				result.longDates = new List<long>();
				foreach (DateTimeOffset date in graph.Dates)
				{
					DateTimeOffset unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
					result.longDates.Add((long)date.Subtract(unixEpoch).TotalMilliseconds);
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