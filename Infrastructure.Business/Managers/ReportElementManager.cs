using AutoMapper;
using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Domain.Interfaces;
using Infrastructure.Business.DTOs.ReportElements;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<ReportElement> GetById(int id)
        {
            ReportElement reportElement = await unitOfWork.ReportElementRepo.GetById(id);
            return reportElement;
        }

        #region WordCloud
        public async Task<WordCloudDTO> GetWordCloudById(int ReportElementId)
        {
            ReportElement reportElement = await unitOfWork.ReportElementRepo.GetById(ReportElementId);
            Dashboard dashboard = await unitOfWork.DashboardRepo.GetById(reportElement.DashboardId);

            DateTime date = DateTime.Now.AddDays(-reportElement.Days);

            IEnumerable<History> histories = await unitOfWork.HistoryRepo.GetHistoriesBySensorIdAndDate(reportElement.SensorId, date);

            Sensor sensor = histories.FirstOrDefault().Sensor;
            WordCloudDTO wordCloud = mapper.Map<Sensor, WordCloudDTO>(sensor);

            wordCloud.DashboardName = dashboard.Name;

            wordCloud.IntValues = new List<int>();
            wordCloud.DoubleValues = new List<double>();
            wordCloud.BoolValues = new List<bool>();
            wordCloud.StringValues = new List<string>();

            foreach (History history in histories)
            {
                switch (wordCloud.MeasurementType)
                {
                    case MeasurementType.Int:
                        if (history.IntValue.HasValue)
                            wordCloud.IntValues.Add(history.IntValue.Value);
                        break;

                    case MeasurementType.Double:
                        if (history.DoubleValue.HasValue)
                            wordCloud.DoubleValues.Add(history.DoubleValue.Value);
                        break;

                    case MeasurementType.Bool:
                        if (history.BoolValue.HasValue)
                            wordCloud.BoolValues.Add(history.BoolValue.Value);
                        break;

                    case MeasurementType.String:
                        if (!String.IsNullOrEmpty(history.StringValue))
                            wordCloud.StringValues.Add(history.StringValue);
                        break;
                    default:
                        break;
                }
            }
            return wordCloud;
        }

        public async Task<ColumnRangeDTO> GetColumnRangeById(int ReportElementId)
        {
            ReportElement reportElement = await unitOfWork.ReportElementRepo.GetById(ReportElementId);
            Dashboard dashboard = await unitOfWork.DashboardRepo.GetById(reportElement.DashboardId);

            DateTime date = DateTime.Now.AddDays(-reportElement.Days);

            IEnumerable<History> histories = await unitOfWork.HistoryRepo.GetHistoriesBySensorIdAndDate(reportElement.SensorId, date);

            Sensor sensor = histories.FirstOrDefault().Sensor;

            return null;

        }

        public void EditWordCloud(WordCloudDTO wordCloud)
        {
            ReportElement reportElement = mapper.Map<WordCloudDTO, ReportElement>(wordCloud);
            unitOfWork.ReportElementRepo.Update(reportElement);
            unitOfWork.Save();
        }
        #endregion

        public void CreateGauge(int dashboardId, int sensorId)
        {
            throw new NotImplementedException();
        }

        public async Task<GaugeDto> GetGaugeById(int gaugeId)
        {
            ReportElement reportElement = await unitOfWork.ReportElementRepo.GetById(gaugeId);
            GaugeDto gaugeDto = mapper.Map<ReportElement, GaugeDto>(reportElement);
            gaugeDto.Min = await historyManager.GetMinValueAfterDate(reportElement.SensorId, DateTimeOffset.Now - new TimeSpan(14, 0, 0, 0));
            gaugeDto.Max = await historyManager.GetMaxValueAfterDate(reportElement.SensorId, DateTimeOffset.Now - new TimeSpan(14, 0, 0, 0));
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
