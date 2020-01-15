using Domain.Core.Model;
using Infrastructure.Business.DTOs.ReportElements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public interface IReportElementManager
    {
        Task<ReportElement> GetById(int id);
        Task<ReportElementDto> GetWordCloudById(int ReportElementId);
        Task<ReportElementDto> GetColumnRangeById(int ReportElementId);
        Task<GaugeDto> GetGaugeById(int gaugeId);
        Task UpdateReportElementHours(int gaugeId, int hours);
        void EditReportElement(ReportElementDto wordCloud);
    }
}
