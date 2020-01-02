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

		public async Task<IActionResult> Index(PaginationDTO paginationDTO)
		{
			var histories = await _historyTestManager.GetAllHistoriesAsync();
			histories = histories.OrderBy(s => s.Id);

			var models = _mapper.Map<IEnumerable<HistoryDto>, IEnumerable<HistoryViewModel>>(histories);

            paginationDTO.Amount = histories.Count();
            histories = histories.Skip((paginationDTO.CurrentPage - 1) * paginationDTO.PageSize).Take(paginationDTO.PageSize).ToList();
			
            IEnumerable<HistoryViewModel> historiesViewModel = _mapper.Map<IEnumerable<HistoryDto>, IEnumerable<HistoryViewModel>>(histories);
            return View(new AllHistoriesViewModel
            {
                Histories = historiesViewModel,
                paginationDTO = paginationDTO
            });
		}

		public async Task<IActionResult> Detail(PaginationDTO paginationDTO, int sensorId)
		{
			var histories = await _historyTestManager.GetHistoriesBySensorIdAsync(sensorId);

			var models = _mapper.Map<IEnumerable<HistoryDto>, IEnumerable<HistoryViewModel>>(histories);

			return View(new AllHistoriesViewModel
			{
				Histories = models,
                paginationDTO=paginationDTO
			});
		}

		#region InvalidSensors

        public IActionResult InvalidSensors(PaginationDTO paginationDTO, SortState sortState=SortState.SensorAsc)
        {
            IEnumerable<HistoryDto> histories = _invalidSensorManager.getInvalidSensors();

            ViewData["SensorSort"] = sortState == SortState.SensorAsc ? SortState.SensorDesc : SortState.SensorAsc;
            ViewData["DateSort"] = sortState == SortState.DateAsc ? SortState.DateDesc : SortState.DateAsc;

            histories = SortValue.SortHistories(sortState, histories);

            paginationDTO.Amount = histories.Count();
            histories = histories.Skip((paginationDTO.CurrentPage - 1) * paginationDTO.PageSize).Take(paginationDTO.PageSize).ToList();
            IEnumerable<HistoryViewModel> historiesViewModel = _mapper.Map<IEnumerable<HistoryDto>, IEnumerable<HistoryViewModel>>(histories);
            return View(new InvalidSensorsViewModel
            {
                Histories=historiesViewModel,
                paginationDTO =paginationDTO
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