using AutoMapper;
using Domain.Core.Model;
using Domain.Interfaces;
using Infrastructure.Business.DTOs.ReportElements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public class ReportElementManager : BaseManager, IReportElementManager
    {
        protected readonly IHistoryManager historyManager;

        public ReportElementManager(IHistoryManager historyManager, IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            this.historyManager = historyManager;
        }

        public void CreateGauge(int dashboardId, int sensorId)
        {
            throw new NotImplementedException();
        }

        public async Task<GaugeDto> GetGaugeById(int gaugeId)
        {
            ReportElement reportElement = await unitOfWork.ReportElementRepo.GetById(gaugeId);
            GaugeDto gaugeDto = mapper.Map<ReportElement, GaugeDto>(reportElement);
            gaugeDto.Min = historyManager.GetMinValueAfterDate(reportElement.SensorId, DateTimeOffset.Now - new TimeSpan(14, 0, 0, 0));
            gaugeDto.Max = historyManager.GetMaxValueAfterDate(reportElement.SensorId, DateTimeOffset.Now - new TimeSpan(14, 0, 0, 0));
            if (gaugeDto.Min.HasValue && gaugeDto.Max.HasValue && gaugeDto.Min != gaugeDto.Max)
            {
                gaugeDto.SensorName = reportElement.Sensor.Name;
                gaugeDto.MeasurementName = reportElement.Sensor.SensorType.MeasurementName;
                gaugeDto.IsValid = true;
            }
            return gaugeDto;
        }
    }
}
