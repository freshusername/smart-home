using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.History;
using Infrastructure.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models;
using smart_home_web.Models.History;
using Domain.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Infrastructure.Business.Managers;

namespace smart_home_web.Controllers
{
	[Authorize]
    public class HistoryController : Controller
	{
		private readonly IHistoryManager _historyManager;
		private readonly IReportElementManager _reportElementManager;
		private readonly IMapper _mapper;
		private readonly UserManager<AppUser> _userManager;


		public HistoryController(
			IHistoryManager historyTestManager,
			IReportElementManager reportElementManager, 
			IMapper mapper,
			UserManager<AppUser> userManager)
		{
			_historyManager = historyTestManager;
            _reportElementManager = reportElementManager;
			_mapper = mapper;
			_userManager = userManager;
		}

        public async Task<IActionResult> Index(FilterDto FilterDTO, bool isActivated = true)
		{
            return View( await GetHistories(FilterDTO, isActivated));
		}

        public async Task<IActionResult> UpdateHistoryTable(FilterDto FilterDTO, bool isActivated)
        {
            return ViewComponent("History", await GetHistories(FilterDTO, isActivated) );
        }

        private async Task<AllHistoriesViewModel> GetHistories(FilterDto FilterDTO, bool isActivated = true)
        {
            var histories = await _historyManager.GetHistoriesAsync(FilterDTO.PageSize, FilterDTO.CurrentPage, FilterDTO.sortState, isActivated);
            string userId = _userManager.GetUserId(User);
            histories = histories.Where(h => h.UserId == userId);
            FilterDTO.Amount = await _historyManager.GetAmountOfUserHistoriesAsync(true, userId);

            var historiesViewModel = _mapper.Map<IEnumerable<HistoryDto>, IEnumerable<HistoryViewModel>>(histories);
            return new AllHistoriesViewModel
            {
                Histories = historiesViewModel,
                FilterDto = FilterDTO
            };
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