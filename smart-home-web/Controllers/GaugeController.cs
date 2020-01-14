using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.ReportElements;

namespace smart_home_web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GaugeController : ControllerBase
    {
        private readonly IReportElementManager _reportElementManager;
        private readonly IMapper _mapper;

        public GaugeController(IReportElementManager reportElementManager, IMapper mapper)
        {
            _reportElementManager = reportElementManager;
            _mapper = mapper;
        }

        [HttpGet("GetGauge/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            GaugeDto gaugeDto = await _reportElementManager.GetGaugeById(id);
            GaugeUpdateViewModel result = _mapper.Map<GaugeDto, GaugeUpdateViewModel>(gaugeDto);

            return Ok(result);
        }

    }
}