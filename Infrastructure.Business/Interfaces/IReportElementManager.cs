using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.DTOs.SensorType;
using Infrastructure.Business.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public interface IReportElementManager
    {
        Task<ReportElement> GetById(int id);
        Task<OperationDetails> CreateReportElement(ReportElementDto reportElementDto);
        Task<ReportElementDto> GetWordCloudById(int ReportElementId);
        Task<ReportElementDto> GetColumnRangeById(int ReportElementId);
        Task<GaugeDto> GetGaugeById(int gaugeId);
        Task UpdateReportElementHours(int gaugeId, int hours);
        void EditReportElement(ReportElementDto wordCloud);
		Task Update(ReportElement reportElement);
        Task<ReportElementDto> GetDataForTimeSeries(int id);
    }
}
