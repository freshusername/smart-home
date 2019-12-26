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

        public IActionResult InvalidSensors()
        {
            var histories = _invalidSensorManager.getInvalidSensors();
            return View(_mapper.Map<List<HistoryDto>, List<HistoryViewModel>>(histories));
        }

        #endregion
    }
}