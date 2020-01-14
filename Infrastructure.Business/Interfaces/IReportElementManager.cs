using Domain.Core.Model;
using Infrastructure.Business.DTOs.ReportElements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Interfaces
{
    public interface IReportElementManager
    {
        Task<ReportElement> GetById(int id);
        Task<ReportElementDTO> GetWordCloudById(int ReportElementId);
        Task<ReportElementDTO> GetColumnRangeById(int ReportElementId);
        Task<GaugeDto> GetGaugeById(int gaugeId);
        void EditReportElement(ReportElementDTO wordCloud);
    }
}
