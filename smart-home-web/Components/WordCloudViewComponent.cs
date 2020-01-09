using AutoMapper;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Components
{
    public class WordCloudViewComponent : BaseViewComponent
    {
        private readonly IHistoryManager _historyManager;
        private readonly IMapper _mapper;
        public WordCloudViewComponent(IHistoryManager historyManager, IMapper mapper)
        {
            _historyManager = historyManager;
            _mapper = mapper;
        }

        public IViewComponentResult Invoke(int dashboardId, int sensorId, int days)
        {
            GraphDTO graph = _historyManager.GetGraphBySensorId(sensorId, days);
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
            return View("WordCloud", result);
        }
    }
}
