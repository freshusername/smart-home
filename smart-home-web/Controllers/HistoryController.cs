﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.History;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models;
using smart_home_web.Models.History;
using Domain.Core.Model;
using Infrastructure.Business.Services;
using Infrastructure.Business.DTOs.SensorType;


namespace smart_home_web.Controllers
{
      
    public class HistoryController : Controller
	{
		private readonly IHistoryManager _historyManager;
		private readonly IReportElementManager _reportElementManager;
		private readonly IMapper _mapper;

		public HistoryController(IHistoryManager historyTestManager,IReportElementManager reportElementManager , IMapper mapper)
		{
			_historyManager = historyTestManager;
            _reportElementManager = reportElementManager;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index(FilterDto FilterDTO, bool isActivated=true)
		{
			var histories = await _historyManager.GetHistoriesAsync(FilterDTO.PageSize, FilterDTO.CurrentPage, FilterDTO.sortState, isActivated);
			
            FilterDTO.Amount = await _historyManager.GetAmountAsync(isActivated);

			//TODO: Replace mapper to service
            var historiesViewModel = _mapper.Map<IEnumerable<HistoryDto>, IEnumerable<HistoryViewModel>>(histories);
			AllHistoriesViewModel model = new AllHistoriesViewModel
			{
				Histories = historiesViewModel,
				FilterDto = FilterDTO
			};


			return View(!isActivated ? "InvalidSensors" : "Index", model);
		}

		public async Task<IActionResult> Detail(FilterDto FilterDTO)
		{
			var histories = await _historyManager.GetHistoriesAsync(FilterDTO.PageSize, FilterDTO.CurrentPage, FilterDTO.sortState, true, FilterDTO.sensorId);
			
			var result = _mapper.Map<IEnumerable<HistoryDto>, IEnumerable<HistoryViewModel>>(histories);

			return View(new AllHistoriesViewModel
			{
				Histories = result,
				FilterDto = FilterDTO
			});
		}

		#region InvalidSensors

		public async Task<IActionResult> InvalidSensors(FilterDto filterDTO)
		{
			return await Index(filterDTO, false);
        }

        #endregion

        [HttpGet]
		public async Task<IActionResult> Graph(int sensorId, int days = 30)
		{
			GraphDto graph = await _historyManager.GetGraphBySensorId(sensorId, days);
			GraphViewModel result = _mapper.Map<GraphDto, GraphViewModel>(graph);
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