using AutoMapper;
using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.Infrastructure;
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

        //TODO: Check if we are able to update options with this method
        public async Task EditReportElement(ReportElementDto reportElementDTO)
        {
            ReportElement reportElement = mapper.Map<ReportElementDto, ReportElement>(reportElementDTO);
            await unitOfWork.ReportElementRepo.Update(reportElement);
            unitOfWork.Save();
        }

        public async Task CreateReportElement(ReportElementDto reportElementDto)
        {
            ReportElement reportElement = mapper.Map<ReportElementDto, ReportElement>(reportElementDto);
            await unitOfWork.ReportElementRepo.Insert(reportElement);
            unitOfWork.Save();
        }

        public async Task<ReportElementDto> GetWordCloudById(int ReportElementId)
        {
            ReportElement reportElement = await unitOfWork.ReportElementRepo.GetById(ReportElementId);
            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0);
            if (reportElement.Hours != 0)
                date = DateTime.Now.AddHours(-(int)reportElement.Hours);
            IEnumerable<History> histories = await unitOfWork.HistoryRepo.GetHistoriesBySensorIdAndDate(reportElement.SensorId, date);

            if (!histories.Any())
                return new ReportElementDto { Id = ReportElementId, IsCorrect = false };

            ReportElementDto wordCloud = mapper.Map<ReportElement, ReportElementDto>(reportElement);

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
                        wordCloud.Values.Add(history.StringValue);
                        break;
                }
            }
            if (!wordCloud.Values.Any())
                return new ReportElementDto { Id = ReportElementId, IsCorrect = false };
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
                return new ReportElementDto { Id = ReportElementId, IsCorrect = false, Message = "No histories in this report element" };

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
                    return new ReportElementDto { Id = ReportElementId, IsCorrect = false, Message = "Incorrect sensor type for this element" };
                case MeasurementType.String:
                    return new ReportElementDto { Id = ReportElementId, IsCorrect = false, Message = "Incorrect sensor type for this element" };
                default:
                    return new ReportElementDto { Id = ReportElementId, IsCorrect = false, Message = "Incorrect sensor type for this element" };
            }
            return columnRange;
        }

        public async Task<ReportElementDto> GetDataForTimeSeries(int id, ReportElementHours hours)
        {
            var reportElement = await unitOfWork.ReportElementRepo.GetById(id);
            if (reportElement == null) return null;

            DateTimeOffset date;
            if (hours == 0)
                date = new DateTimeOffset(1970, 1, 1, 0, 0, 0, new TimeSpan(0, 0, 0));

            date = DateTimeOffset.Now.AddHours(-(int)hours);

            var histories = await unitOfWork.HistoryRepo.GetHistoriesBySensorIdAndDate(reportElement.SensorId, date);
            var dashboard = await unitOfWork.DashboardRepo.GetById(reportElement.DashboardId);

            var milliseconds = GetMilliseconds(histories).ToList();
            var values = GetValues(histories).ToList();

            ReportElementDto schedule = mapper.Map<Sensor, ReportElementDto>(histories.First().Sensor);
            schedule.Milliseconds = milliseconds;
            schedule.Values = values;
            schedule.DashboardName = dashboard.Name;

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

        private IEnumerable<long> GetMilliseconds(IEnumerable<History> histories)
        {
            foreach (var items in histories)
            {
                yield return items.Date.ToUnixTimeMilliseconds();

            }
        }

		public async Task Update(ReportElement reportElement)
		{
			ReportElement result = await unitOfWork.ReportElementRepo.GetById(reportElement.Id);
			result.X = reportElement.X;
			result.Y = reportElement.Y;
			result.Width = reportElement.Width;
			result.Height = reportElement.Height;
			unitOfWork.ReportElementRepo.Update(result);
			unitOfWork.Save();
		}

		public async Task Delete(ReportElement reportElement)
		{
			await unitOfWork.ReportElementRepo.Delete(reportElement);
			unitOfWork.Save();
		}

    }
}
