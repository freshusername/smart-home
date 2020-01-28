using Domain.Core.Model;
using Infrastructure.Business.DTOs.History;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.DTOs.Sensor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
	public interface IReportElementManager
    {
		Task<ReportElementDto> GetWordCloudById(int ReportElementId);
		Task<ReportElementDto> GetColumnRangeById(int ReportElementId);
        Task<ReportElementDto> GetStatusReport(int ReportElementId);
		Task<ReportElementDto> GetDataForTimeSeries(int id);
		Task<ReportElementDto> GetOnOffById(int id);

		Task<ReportElement> GetById(int id);
        Task<HeatmapDto> GetHeatmapById(int ReportElementId);
        Task<GaugeDto> GetGaugeById(int gaugeId);
        Task<SensorDto> GetLastSensorByUserId(string userId);

        Task CreateReportElement(ReportElementDto reportElementDto,string userId);
        Task UpdateReportElementHours(int gaugeId, int hours);
        Task EditReportElement(ReportElementDto wordCloud);
		Task Update(ReportElement reportElement);
		Task Lock(ReportElement reportElement);
		Task Delete(ReportElement reportElement);
    }
}
