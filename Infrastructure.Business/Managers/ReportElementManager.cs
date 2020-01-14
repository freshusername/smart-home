using AutoMapper;
using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Domain.Interfaces;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.DTOs.SensorType;
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

        public async Task<ReportElementDto> GetDataForSchedule(int id , int days)
        {
            DateTimeOffset date = DateTimeOffset.Now.AddDays(-days);
            var histories = await unitOfWork.HistoryRepo.GetHistoriesBySensorIdAndDate(id, date);
             
             var dates = GetDates(histories);
              var values = GetValues(histories);

            ReportElementDto schedule = mapper.Map<Sensor,ReportElementDto>(histories.First().Sensor);
             schedule.Dates = dates;
              schedule.Values = values;

            return schedule;
        }

        private IEnumerable<dynamic> GetValues(IEnumerable<History> histories)
        {
                      
            foreach (var items in histories)
            {
                if (items.Sensor.SensorType.MeasurementType == MeasurementType.Int)
                    yield return items.IntValue;
                else
                if (items.Sensor.SensorType.MeasurementType == MeasurementType.Double)
                    yield return items.DoubleValue;
                else
                 if (items.Sensor.SensorType.MeasurementType == MeasurementType.Bool)
                    yield return items.BoolValue;
                else
                    yield return items.StringValue;
            }        
        }

        private IEnumerable<DateTimeOffset> GetDates(IEnumerable<History> histories)
        {                      
            foreach (var items in histories)
            {
                yield return items.Date;
            }      
        }
    }
}
