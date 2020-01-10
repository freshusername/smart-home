using Infrastructure.Business.DTOs.ReportElements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public interface IReportElementManager
    {
        Task<GaugeDto> GetGaugeById(int gaugeId);
    }
}
