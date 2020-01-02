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

		public async Task<IActionResult> Index(FilterDTO FilterDTO, SortState sortState = SortState.SensorAsc )
		{
			var histories = await _historyTestManager.GetAllHistoriesAsync();

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

		public async Task<IActionResult> Detail(FilterDTO FilterDTO, int sensorId, SortState sortState = SortState.SensorAsc)
		{
			var resultList = await _historyTestManager.GetHistoriesBySensorIdAsync(sensorId);

            var histories = SortValue.SortHistories(sortState, resultList);

            var models = _mapper.Map<IEnumerable<HistoryDto>, IEnumerable<HistoryViewModel>>(histories);
            return View(new AllHistoriesViewModel
			{
				Histories = models,
                FilterDTO=FilterDTO
			});
		}



        #region InvalidSensors

        public IActionResult InvalidSensors(FilterDTO filterDTO)
        {
            IEnumerable<HistoryDto> histories = _invalidSensorManager.getInvalidSensors(filterDTO.sortState);

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
    }
}