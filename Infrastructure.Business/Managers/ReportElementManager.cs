using AutoMapper;
using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
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

            DateTime date = DateTime.Now.AddHours(-(int)reportElement.Hours);

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

        public async Task<GaugeDto> GetGaugeById(int gaugeId)
        {
            ReportElement reportElement = await unitOfWork.ReportElementRepo.GetById(gaugeId);
            GaugeDto gaugeDto = mapper.Map<ReportElement, GaugeDto>(reportElement);

            gaugeDto.Min = historyManager.GetMinValueForPeriod(reportElement.SensorId, (int)gaugeDto.Hours);
            gaugeDto.Max = historyManager.GetMaxValueForPeriod(reportElement.SensorId, (int)gaugeDto.Hours);
            if (gaugeDto.Min.HasValue && gaugeDto.Max.HasValue)
            {
                var value = historyManager.GetLastHistoryBySensorId(reportElement.SensorId);
                gaugeDto.Value = value.DoubleValue.HasValue ? value.DoubleValue : value.IntValue;
                gaugeDto.SensorName = reportElement.Sensor.Name;
                gaugeDto.MeasurementName = reportElement.Sensor.SensorType.MeasurementName;
                if (gaugeDto.Min == gaugeDto.Max)
                {
                    gaugeDto.Min--;
                }
                gaugeDto.IsValid = true;
            }

            return gaugeDto;
        }

        public async Task UpdateReportElementHours(int gaugeId, int hours)
        {
            ReportElement reportElement = await unitOfWork.ReportElementRepo.GetById(gaugeId);
            reportElement.Hours = (ReportElementHours)hours;
            unitOfWork.ReportElementRepo.Update(reportElement);
            unitOfWork.Save();
        }

        public async Task<ReportElementDto> GetColumnRangeById(int ReportElementId)
        {
            ReportElement reportElement = await unitOfWork.ReportElementRepo.GetById(ReportElementId);
            if (reportElement == null)
                return new ReportElementDto { IsCorrect = false, Message = "Invalid report element" };

            DateTime date = DateTime.Now.AddHours(-(int)reportElement.Hours);
            IEnumerable<History> histories = await unitOfWork.HistoryRepo.GetHistoriesBySensorIdAndDate(reportElement.SensorId, date);
            if (!histories.Any())
                return new ReportElementDto { Id = ReportElementId, IsCorrect = false, Message= "No histories for that period of time" };

            ReportElementDto columnRange = mapper.Map<Sensor, ReportElementDto>(reportElement.Sensor);

            columnRange.DashboardName = reportElement.Dashboard.Name;
            columnRange.Dates = new List<string>();
            columnRange.MinValues = new List<dynamic>();
            columnRange.MaxValues = new List<dynamic>();

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
                        columnRange.MinValues.Add(t.Min.GetValueOrDefault());
                        columnRange.MaxValues.Add(t.Max.GetValueOrDefault());
                    }
                    break;


                case MeasurementType.Double:
                    var doubleValues = histories.GroupBy(p => p.Date).Select(p => new
                    {
                        Min = p.Min(g => g.DoubleValue),
                        Max = p.Max(g => g.DoubleValue),
                        Date = p.Key
                    }).ToList();

                    foreach (var t in doubleValues)
                    {
                        columnRange.Dates.Add(t.Date.DateTime.ToShortDateString());
                        columnRange.MinValues.Add(Math.Round(t.Min.GetValueOrDefault(), 2));
                        columnRange.MaxValues.Add(Math.Round(t.Max.GetValueOrDefault(), 2));
                    }
                    break;
                case MeasurementType.Bool:
                    return new ReportElementDto { Id = ReportElementId, IsCorrect = false, Message="Incorrect sensor type for this element" };
                case MeasurementType.String:
                    return new ReportElementDto { Id = ReportElementId, IsCorrect = false, Message = "Incorrect sensor type for this element" };
                default:
                    return new ReportElementDto { Id = ReportElementId, IsCorrect = false, Message = "Incorrect sensor type for this element" };
            }
            return columnRange;
        }
    }
}
