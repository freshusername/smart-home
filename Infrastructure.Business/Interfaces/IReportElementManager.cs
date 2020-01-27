﻿using Domain.Core.Model;
using Infrastructure.Business.DTOs.History;
using Infrastructure.Business.DTOs.ReportElements;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
	public interface IReportElementManager
    {
		Task<ReportElementDto> GetWordCloudById(int ReportElementId);
		Task<ReportElementDto> GetColumnRangeById(int ReportElementId);
        Task<IEnumerable<HistoryDto>> GetStatusReport();
		Task<ReportElementDto> GetDataForTimeSeries(int id);

		Task<ReportElement> GetById(int id);
        Task<HeatmapDto> GetHeatmapById(int ReportElementId);
        Task<GaugeDto> GetGaugeById(int gaugeId);

        Task CreateReportElement(ReportElementDto reportElementDto);
        Task UpdateReportElementHours(int gaugeId, int hours);
        Task EditReportElement(ReportElementDto wordCloud);
		Task Update(ReportElement reportElement);
		Task Lock(ReportElement reportElement);
		Task Delete(ReportElement reportElement);
    }
}
