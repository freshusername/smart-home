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

		public async Task<IActionResult> Index(FilterDTO FilterDTO, bool isActivated=true)
		{
			var histories = await _historyManager.GetHistoriesAsync(FilterDTO.PageSize, FilterDTO.CurrentPage, FilterDTO.sortState, isActivated);
			
            FilterDTO.Amount = await _historyManager.GetAmountAsync(isActivated);

			//TODO: Replace mapper to service
            var historiesViewModel = _mapper.Map<IEnumerable<HistoryDto>, IEnumerable<HistoryViewModel>>(histories);
			AllHistoriesViewModel model = new AllHistoriesViewModel
			{
				Histories = historiesViewModel,
				FilterDTO = FilterDTO
			};


			return View(!isActivated ? "InvalidSensors" : "Index", model);
		}

		public async Task<IActionResult> Detail(FilterDTO FilterDTO)
		{
			var histories = await _historyManager.GetHistoriesAsync(FilterDTO.PageSize, FilterDTO.CurrentPage, FilterDTO.sortState, true, FilterDTO.sensorId);
			
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
			return await Index(filterDTO, false);
        }

        #endregion

  //      [HttpGet]
		//public async Task<IActionResult> Graph(int sensorId, int days = 30)
		//{
		//	var schedule =  _reportElementManager.GetDataForSchedule(sensorId);
		//	 var result = _mapper.Map<ScheduleDto, ScheduleViewModel>(schedule);
														
		//	return View(result);
		//}

		[HttpPost]
		public IActionResult Graph(GraphViewModel model)
		{
			return RedirectToAction("Graph", new { sensorId = model.SensorId, days = model.Days == 0 ? 30 : model.Days });
		}
      
    }
}