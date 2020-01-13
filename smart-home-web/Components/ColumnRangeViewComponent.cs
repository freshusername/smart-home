using AutoMapper;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models;
using System;
using System.Threading.Tasks;

namespace smart_home_web.Components
{
    public class ColumnRangeViewComponent : BaseViewComponent
    {
        private readonly IMapper _mapper;
        private readonly IHistoryManager _historyManager;

        public ColumnRangeViewComponent(IMapper mapper,IHistoryManager manager)
        {
            _mapper = mapper;
            _historyManager = manager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int dashboardId, int sensorId, int days)
        {
            GraphDTO graphDTO = await _historyManager.GetGraphBySensorId(sensorId, days);
            GraphViewModel result = _mapper.Map<GraphDTO, GraphViewModel>(graphDTO);
            if (result.IsCorrect)
            {

            }
            return View(result);
        }
    }
}
