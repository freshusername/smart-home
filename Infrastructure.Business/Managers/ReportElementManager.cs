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

        public void EditReportElement(ReportElementDto reportElementDTO)
        {
            ReportElement reportElement = mapper.Map<ReportElementDto, ReportElement>(reportElementDTO);
            unitOfWork.ReportElementRepo.Update(reportElement);
            unitOfWork.Save();
        }

        public async Task<ReportElementDto> GetWordCloudById(int ReportElementId)
        {
            ReportElement reportElement = await unitOfWork.ReportElementRepo.GetById(ReportElementId);

            DateTime date = DateTime.Now.AddHours(-reportElement.Hours);

            IEnumerable<History> histories = await unitOfWork.HistoryRepo.GetHistoriesBySensorIdAndDate(reportElement.SensorId, date);

            if (!histories.Any())
                return new ReportElementDto { Id = ReportElementId, IsCorrect = false };

            ReportElementDto wordCloud = mapper.Map<Sensor, ReportElementDto>(reportElement.Sensor);

            wordCloud.DashboardName = reportElement.Dashboard.Name;
            wordCloud.Values = new List<dynamic>();

            foreach (History history in histories)
            {
                switch (wordCloud.MeasurementType)
                {
                    case MeasurementType.Int when history.IntValue.HasValue:
                        wordCloud.Values.Add(history.IntValue);
                        break;
                    case MeasurementType.Double when history.DoubleValue.HasValue:
                        wordCloud.Values.Add(history.DoubleValue);
                        break;
                    case MeasurementType.Bool when history.BoolValue.HasValue:
                        wordCloud.Values.Add(history.BoolValue);
                        break;
                    case MeasurementType.String when !String.IsNullOrEmpty(history.StringValue):
                        wordCloud.Values.Add(history.IntValue);
                        break;
                }
            }
            if (!wordCloud.Values.Any())
                return new ReportElementDto { IsCorrect = false };
            return wordCloud;
        }

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
                var value = historyManager.GetLastHistoryBySensorId(reportElement.SensorId);
                gaugeDto.Value = value.DoubleValue.HasValue ? value.DoubleValue : value.IntValue;
                gaugeDto.SensorName = reportElement.Sensor.Name;
                gaugeDto.MeasurementName = reportElement.Sensor.SensorType.MeasurementName;
                gaugeDto.IsValid = true;
            }
            return gaugeDto;
        }

        public async Task<ReportElementDto> GetColumnRangeById(int ReportElementId)
        {
            ReportElement reportElement = await unitOfWork.ReportElementRepo.GetById(ReportElementId);
            if (reportElement == null)
                return new ReportElementDto { IsCorrect = false, Message = "Invalid report element" };

            DateTime date = DateTime.Now.AddHours(-reportElement.Hours);
            IEnumerable<History> histories = await unitOfWork.HistoryRepo.GetHistoriesBySensorIdAndDate(reportElement.SensorId, date);
            if (!histories.Any())
                return new ReportElementDto { Id = ReportElementId, IsCorrect = false, Message="No histories in this report element" };

            ReportElementDto columnRange = mapper.Map<Sensor, ReportElementDto>(reportElement.Sensor);

            columnRange.DashboardName = reportElement.Dashboard.Name;
            columnRange.Dates = new List<string>();
            columnRange.MinValues = new List<int?>();
            columnRange.MaxValues = new List<int?>();

            switch (columnRange.MeasurementType)
            {
                case MeasurementType.Int:
                    var intValues = histories.GroupBy(p => p.Date).Select(p => new
                    {
                        Min = p.Min(g => g.IntValue),
                        Max = p.Max(g => g.IntValue),
                        Date = p.Key
                    }).ToList();

                    foreach (var t in intValues)
                    {
                        columnRange.Dates.Add(t.Date.DateTime.ToShortDateString());
                        columnRange.MinValues.Add(t.Min);
                        columnRange.MaxValues.Add(t.Max);
                    }
                    break;


                case MeasurementType.Double:
                    var doubleValues = histories.GroupBy(p => p.Date).Select(p => new
                    {
                        Min = p.Min(g => g.IntValue),
                        Max = p.Max(g => g.IntValue),
                        Date = p.Key
                    }).ToList();

                    foreach (var t in doubleValues)
                    {
                        columnRange.Dates.Add(t.Date.DateTime.ToShortDateString());
                        columnRange.MinValues.Add(t.Min);
                        columnRange.MaxValues.Add(t.Max);
                    }
                    break;
                case MeasurementType.Bool:
                    return new ReportElementDto { Id = ReportElementId, IsCorrect = false, Message="Invalid sensor type" };
                case MeasurementType.String:
                    return new ReportElementDto { Id = ReportElementId, IsCorrect = false, Message = "Invalid sensor type" };
                default:
                    return new ReportElementDto { Id = ReportElementId, IsCorrect = false, Message = "Invalid sensor type" };
            }
            return columnRange;
        }
    }
}
