using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Business.DTOs.History;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;
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



        #region InvalidSensors

        public IActionResult InvalidSensors(int page = 1, HistorySortState sortState=HistorySortState.SensorAsc)
        {
            ViewData["SensorSort"] = sortState == HistorySortState.SensorAsc ? HistorySortState.SensorDesc : HistorySortState.SensorAsc;
            ViewData["DateSort"] = sortState == HistorySortState.DateAsc ? HistorySortState.DateDesc : HistorySortState.DateAsc;
            ViewData["StringSort"] = sortState == HistorySortState.StringValueAsc ? HistorySortState.StringValueDesc : HistorySortState.StringValueAsc;
            ViewData["IntSort"] = sortState == HistorySortState.IntValueAsc ? HistorySortState.IntValueDesc : HistorySortState.IntValueAsc;
            ViewData["DoubleSort"] = sortState == HistorySortState.DoubleValueAsc ? HistorySortState.DoubleValueDesc : HistorySortState.DoubleValueAsc;
            ViewData["BoolSort"] = sortState == HistorySortState.BoolValueAsc ? HistorySortState.BoolValueDesc : HistorySortState.BoolValueAsc;

            List<HistoryDto> histories = _invalidSensorManager.getInvalidSensors();
            switch (sortState)
            {
                case HistorySortState.SensorAsc:
                    histories = histories.OrderBy(p => p.SensorId).ToList();
                    break;
                case HistorySortState.SensorDesc:
                    histories = histories.OrderByDescending(p => p.SensorId).ToList();
                    break;
                case HistorySortState.DateAsc:
                    histories = histories.OrderBy(p => p.Date).ToList();
                    break;
                case HistorySortState.DateDesc:
                    histories = histories.OrderByDescending(p => p.Date).ToList();
                    break;
                case HistorySortState.StringValueAsc:
                    histories = histories.OrderBy(p => p.StringValue).ToList();
                    break;
                case HistorySortState.StringValueDesc:
                    histories = histories.OrderByDescending(p => p.StringValue).ToList();
                    break;
                case HistorySortState.IntValueAsc:
                    histories = histories.OrderBy(p => p.IntValue).ToList();
                    break;
                case HistorySortState.IntValueDesc:
                    histories = histories.OrderByDescending(p => p.IntValue).ToList();
                    break;
                case HistorySortState.DoubleValueAsc:
                    histories = histories.OrderBy(p => p.DoubleValue).ToList();
                    break;
                case HistorySortState.DoubleValueDesc:
                    histories = histories.OrderByDescending(p => p.DoubleValue).ToList();
                    break;
                case HistorySortState.BoolValueAsc:
                    histories = histories.OrderBy(p => p.BoolValue).ToList();
                    break;
                case HistorySortState.BoolValueDesc:
                    histories = histories.OrderByDescending(p => p.BoolValue).ToList();
                    break;
                default:
                    histories = histories.OrderBy(p => p.SensorId).ToList();
                    break;
            }

            int pageSize = 10;
            int count = histories.Count();
            histories = histories.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            HistoriesPageViewModel pageViewModel = new HistoriesPageViewModel(count, page, pageSize);
            List<HistoryViewModel> historiesViewModel = _mapper.Map<List<HistoryDto>, List<HistoryViewModel>>(histories);
            return View(new InvalidSensorsViewModel
            {
                Histories=historiesViewModel,
                PageViewModel=pageViewModel
            });
        }

        #endregion
    }
}