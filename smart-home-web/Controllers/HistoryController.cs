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

		public async Task<IActionResult> Index(PaginationDTO paginationDTO, SortState sortState = SortState.SensorAsc )
		{
			var histories = await _historyTestManager.GetAllHistoriesAsync();

            //ViewData["SensorSort"] = sortState == SortState.SensorAsc ? SortState.SensorDesc : SortState.SensorAsc;
            //ViewData["DateSort"] = sortState == SortState.DateAsc ? SortState.DateDesc : SortState.DateAsc;

            //histories = SortValue.SortHistories(sortState, histories);

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

		public async Task<IActionResult> Detail(PaginationDTO paginationDTO, int sensorId, SortState sortState = SortState.SensorAsc)
		{
			var histories = await _historyTestManager.GetHistoriesBySensorIdAsync(sensorId);

            //ViewData["SensorSort"] = sortState == SortState.SensorAsc ? SortState.SensorDesc : SortState.SensorAsc;
            //ViewData["DateSort"] = sortState == SortState.DateAsc ? SortState.DateDesc : SortState.DateAsc;
            //ViewData["StringSort"] = sortState == SortState.StringValueAsc ? SortState.StringValueDesc : SortState.StringValueAsc;
            //ViewData["IntSort"] = sortState == SortState.IntValueAsc ? SortState.IntValueDesc : SortState.IntValueAsc;
            //ViewData["DoubleSort"] = sortState == SortState.DoubleValueAsc ? SortState.DoubleValueDesc : SortState.DoubleValueAsc;
            //ViewData["BoolSort"] = sortState == SortState.BoolValueAsc ? SortState.BoolValueDesc : SortState.BoolValueAsc;

            //histories = SortValue.SortHistories(sortState, histories);
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
    }
}