using AutoMapper;
using Domain.Core.CalculateModel;
using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.Business.DTOs.History;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.DTOs.Sensor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public class ReportElementManager : BaseManager, IReportElementManager
    {
        protected readonly IHistoryManager historyManager;
        private string UserId;

        public ReportElementManager(IHistoryManager historyManager, IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            this.historyManager = historyManager;
        }

        public async Task<ReportElement> GetById(int id)
        {
            ReportElement reportElement = await unitOfWork.ReportElementRepo.GetById(id);
            return reportElement;
        }

        public async Task EditReportElement(ReportElementDto reportElementDTO)
        {
            ReportElement reportElement = mapper.Map<ReportElementDto, ReportElement>(reportElementDTO);
            await unitOfWork.ReportElementRepo.Update(reportElement);
            unitOfWork.Save();
        }

        public async Task<bool> CreateReportElement(ReportElementDto reportElementDto, string userId)
        {
            UserId = userId;
            var reportElements = await unitOfWork.ReportElementRepo.GetAll();
            reportElements = reportElements.Where(r => r.DashboardId == reportElementDto.DashboardId);
            if (reportElements.Any())
            {
                var el = reportElements.OrderByDescending(r => r.Y).First();
                var maxElements = reportElements.Where(e => e.Y == el.Y);
                bool rightPos = false;
                int totalWidth = 0;
                foreach (var element in maxElements)
                {
                    if (element.X < 5)
                    {
                        rightPos = true;
                    }
                    totalWidth += element.Width;
                }

                ReportElement reportElement = mapper.Map<ReportElementDto, ReportElement>(reportElementDto);
                reportElement.Height = 6;
                reportElement.Width = 4;
                if (rightPos)
                {
                    reportElement.X = totalWidth;
                    reportElement.Y = el.Y;
                }
                else
                {
                    reportElement.X = 0;
                    reportElement.Y = el.Y;
                }

                await unitOfWork.ReportElementRepo.Insert(reportElement);
                unitOfWork.Save();
                return true;
            }
            else
            {
                ReportElement reportElement = mapper.Map<ReportElementDto, ReportElement>(reportElementDto);
                reportElement.Height = 6;
                reportElement.Width = 4;
                reportElement.X = 0;
                reportElement.Y = 0;
                await unitOfWork.ReportElementRepo.Insert(reportElement);
                unitOfWork.Save();
                return true;
            }
        }

        public async Task<HeatmapDto> GetHeatmapById(int reportElementId)
        {
            ReportElement reportElement = await unitOfWork.ReportElementRepo.GetById(reportElementId);

            DateTime dateFrom = new DateTime();
            DateTime dateTo = DateTime.Now.AddDays(1);

            if (reportElement.Hours != 0)
                dateFrom = DateTime.Now.AddHours(-(int)reportElement.Hours).Date.AddDays(1);

            DateTime[] daysArray = new DateTime[(int)(dateTo - dateFrom).TotalDays];

            for (int i = 0; i < daysArray.Length; i++)
            {
                daysArray[i] = dateFrom.AddDays(i);
            }

            IEnumerable<AvgSensorValuePerDay> avgSensorValuesPerDays = await
                unitOfWork.HistoryRepo.GetAvgSensorsValuesPerDays(reportElement.SensorId.Value, dateFrom, dateTo);
            List<AvgSensorValuePerDay> AvgSensorValuesPerDays = avgSensorValuesPerDays.ToList();

            for (int i = 0; i < daysArray.Length; i++)
            {
                if (!avgSensorValuesPerDays.Any(a => a.WeekDay.ToString("yyyy-MM-dd") == daysArray[i].ToString("yyyy-MM-dd")))
                    AvgSensorValuesPerDays.Add(new AvgSensorValuePerDay { WeekDay = daysArray[i], AvgValue = null });
            }

            AvgSensorValuesPerDays = AvgSensorValuesPerDays.OrderBy(d => d.WeekDay).ToList();

            if (avgSensorValuesPerDays.Count() == 0)
                return new HeatmapDto { Id = reportElementId, IsCorrect = false };

            HeatmapDto heatmap = mapper.Map<Sensor, HeatmapDto>(reportElement.Sensor);

            heatmap.Id = reportElement.Id;
            heatmap.DashboardName = reportElement.Dashboard.Name;
            heatmap.DashboardId = reportElement.Dashboard.Id;
            heatmap.AvgSensorValuesPerDays = AvgSensorValuesPerDays;
            heatmap.Hours = reportElement.Hours;

            return heatmap;
        }

        public async Task<BoolHeatmapDto> GetBoolHeatmapById(int reportElementId)
        {
            ReportElement reportElement = await unitOfWork.ReportElementRepo.GetById(reportElementId);

            DateTime dateFrom = new DateTime();
            DateTime dateTo = DateTime.Now.AddMinutes(1);

            if (reportElement.Hours != 0)
                dateFrom = DateTime.Now.AddHours(-(int)reportElement.Hours);

            int[] hoursArray = new int[(int)(dateTo - dateFrom).TotalHours];
            string[] daysArray = new string[(int)(dateTo - dateFrom).TotalHours];

            for (int i = 0; i < hoursArray.Length; i++)
            {
                hoursArray[i] = dateFrom.AddHours(i).Hour;
                daysArray[i] = dateFrom.AddHours(i).Date.ToString("dd.MM.yyyy");
            }

            IEnumerable<BoolValuePercentagePerHour> boolValuePercentagesPerHours = await
                unitOfWork.HistoryRepo.GetBoolValuePercentagesPerHours(reportElement.SensorId.Value, dateFrom, dateTo);
            List<BoolValuePercentagePerHour> BoolValuePercentagesPerHours = new List<BoolValuePercentagePerHour>();

            int w = 0;
            for (int j = 0; j < hoursArray.Length; j++)
            {
                if (!boolValuePercentagesPerHours.Any(a => a.HourTime == hoursArray[j] && a.DayDate.ToString().Contains(daysArray[j])))
                {

                    BoolValuePercentagesPerHours.Add(
                    new BoolValuePercentagePerHour
                    {
                        DayDate = dateFrom,
                        HourTime = hoursArray[j],
                        TrueCount = null,
                        TrueFalseCount = null,
                        TruePercentage = null
                    });
                    dateFrom = dateFrom.AddHours(1);

                }
                else
                {
                    if (w != boolValuePercentagesPerHours.Count())
                    {
                        BoolValuePercentagesPerHours.Add(
                        new BoolValuePercentagePerHour
                        {
                            DayDate = dateFrom,
                            HourTime = hoursArray[j],
                            TrueCount = boolValuePercentagesPerHours.ElementAt(w).TrueCount,
                            TrueFalseCount = boolValuePercentagesPerHours.ElementAt(w).TrueFalseCount,
                            TruePercentage = boolValuePercentagesPerHours.ElementAt(w).TruePercentage
                        });
                        w++;
                        dateFrom = dateFrom.AddHours(1);

                    }
                    else
                    {
                        BoolValuePercentagesPerHours.Add(
                        new BoolValuePercentagePerHour
                        {
                            DayDate = dateFrom,
                            HourTime = hoursArray[j],
                            TrueCount = null,
                            TrueFalseCount = null,
                            TruePercentage = null
                        });
                        dateFrom = dateFrom.AddHours(1);
                    }
                }
            }

            if (boolValuePercentagesPerHours.Count() == 0)
                return new BoolHeatmapDto { Id = reportElementId, IsCorrect = false };

            BoolHeatmapDto heatmap = mapper.Map<Sensor, BoolHeatmapDto>(reportElement.Sensor);

            heatmap.Id = reportElement.Id;
            heatmap.DashboardName = reportElement.Dashboard.Name;
            heatmap.DashboardId = reportElement.Dashboard.Id;
            heatmap.BoolValuePercentagesPerHours = BoolValuePercentagesPerHours;
            heatmap.Hours = reportElement.Hours;

            return heatmap;
        }

        public async Task<ReportElementDto> GetOnOffById(int ReportElementId)
        {
            ReportElement reportElement = await unitOfWork.ReportElementRepo.GetById(ReportElementId);
            ReportElementDto onOff = mapper.Map<ReportElement, ReportElementDto>(reportElement);
            return onOff;
        }

        public async Task<ReportElementDto> GetWordCloudById(int ReportElementId)
        {
            ReportElement reportElement = await unitOfWork.ReportElementRepo.GetById(ReportElementId);
            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0);
            if (reportElement.Hours != 0)
                date = DateTime.Now.AddHours(-(int)reportElement.Hours);
            IEnumerable<History> histories = await unitOfWork.HistoryRepo.GetHistoriesBySensorIdAndDate(reportElement.SensorId.Value, date);

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

            gaugeDto.Min = historyManager.GetMinValueForPeriod(reportElement.SensorId.Value, (int)gaugeDto.Hours);
            gaugeDto.Max = historyManager.GetMaxValueForPeriod(reportElement.SensorId.Value, (int)gaugeDto.Hours);
            if (gaugeDto.Min.HasValue && gaugeDto.Max.HasValue)
            {
                var value = historyManager.GetLastHistoryBySensorId(reportElement.SensorId.Value);
                gaugeDto.Value = value.DoubleValue.HasValue ? value.DoubleValue : value.IntValue;
                gaugeDto.SensorName = reportElement.Sensor.Name;
                gaugeDto.MeasurementName = reportElement.Sensor.SensorType.MeasurementName;
                if (gaugeDto.Min == gaugeDto.Max)
                {
                    gaugeDto.Min--;
                }
                gaugeDto.IsValid = true;
            }
            else
            {
                gaugeDto.IsValid = false;
            }

            return gaugeDto;
        }

        public async Task UpdateReportElementHours(int gaugeId, int hours)
        {
            ReportElement reportElement = await unitOfWork.ReportElementRepo.GetById(gaugeId);
            reportElement.Hours = (ReportElementHours)hours;
            await unitOfWork.ReportElementRepo.Update(reportElement);
            unitOfWork.Save();
        }
        public async Task<ReportElementDto> GetColumnRangeById(int ReportElementId)
        {
            ReportElement reportElement = await unitOfWork.ReportElementRepo.GetById(ReportElementId);
            if (reportElement == null)
                return new ReportElementDto { IsCorrect = false, Message = "Invalid report element" };
            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0);
            if (reportElement.Hours != 0)
                date = DateTime.Now.AddHours(-(int)reportElement.Hours);
            IEnumerable<History> histories = await unitOfWork.HistoryRepo.GetHistoriesBySensorIdAndDate(reportElement.SensorId.Value, date);
            if (!histories.Any())
            {
                int hours = (int)reportElement.Hours;
                string strhours;
                if (hours == 0)
                    strhours = "all days";
                else if (hours == 1)
                    strhours = "1 hour";
                else if (hours <= 12)
                    strhours = $"{hours} hours";
                else if (hours / 24 == 1)
                    strhours = "1 day";
                else
                    strhours = $"{hours / 24} days";
                return new ReportElementDto { Id = ReportElementId, IsCorrect = false, Message = $"No histories in this report element per {strhours}" };
            }

            ReportElementDto columnRange = mapper.Map<Sensor, ReportElementDto>(reportElement.Sensor);

            columnRange.Id = ReportElementId;
            columnRange.DashboardName = reportElement.Dashboard.Name;
            columnRange.Hours = reportElement.Hours;
            columnRange.Dates = new List<string>();
            columnRange.MinValues = new List<dynamic>();
            columnRange.MaxValues = new List<dynamic>();

            IEnumerable<dynamic> values = null;

            switch (columnRange.MeasurementType)
            {
                case MeasurementType.Int:
                    if ((int)reportElement.Hours == 1)
                    {
                        values = histories.OrderBy(p => p.Date.LocalDateTime).GroupBy(p => p.Date.LocalDateTime.Minute).Select(p => new
                        {
                            Min = p.Min(g => g.IntValue.GetValueOrDefault()),
                            Max = p.Max(g => g.IntValue.GetValueOrDefault()),
                            Date = p.Key.ToString()
                        }).ToList();
                        break;
                    }
                    else if ((int)reportElement.Hours > 1 && (int)reportElement.Hours <= 24)
                    {
                        values = histories.OrderBy(p => p.Date.LocalDateTime).GroupBy(p => p.Date.LocalDateTime.Hour).Select(p => new
                        {
                            Min = p.Min(g => g.IntValue.GetValueOrDefault()),
                            Max = p.Max(g => g.IntValue.GetValueOrDefault()),
                            Date = p.Key.ToString()
                        }).ToList();
                        break;

                    }
                    else
                    {
                        values = histories.OrderBy(p => p.Date.LocalDateTime).GroupBy(p => p.Date.LocalDateTime.Date).Select(p => new
                        {
                            Min = p.Min(g => g.IntValue.GetValueOrDefault()),
                            Max = p.Max(g => g.IntValue.GetValueOrDefault()),
                            Date = p.Key.ToString("d")
                        }).ToList();
                        break;

                    }

                case MeasurementType.Double:

                    if ((int)reportElement.Hours == 1)
                    {
                        values = histories.OrderBy(p => p.Date.LocalDateTime).GroupBy(p => p.Date.LocalDateTime.Minute).Select(p => new
                        {
                            Min = p.Min(g => g.DoubleValue.GetValueOrDefault()),
                            Max = p.Max(g => g.DoubleValue.GetValueOrDefault()),
                            Date = p.Key.ToString()
                        }).ToList();
                        break;

                    }
                    else if ((int)reportElement.Hours > 1 && (int)reportElement.Hours <= 24)
                    {
                        values = histories.OrderBy(p => p.Date.LocalDateTime).GroupBy(p => p.Date.LocalDateTime.Hour).Select(p => new
                        {
                            Min = p.Min(g => g.DoubleValue.GetValueOrDefault()),
                            Max = p.Max(g => g.DoubleValue.GetValueOrDefault()),
                            Date = p.Key.ToString()
                        }).ToList();
                        break;

                    }
                    else
                    {
                        values = histories.OrderBy(p => p.Date.LocalDateTime).GroupBy(p => p.Date.LocalDateTime.Date).Select(p => new
                        {
                            Min = p.Min(g => g.DoubleValue.GetValueOrDefault()),
                            Max = p.Max(g => g.DoubleValue.GetValueOrDefault()),
                            Date = p.Key.ToString("d")
                        }).ToList();
                        break;

                    }
                default:
                    return new ReportElementDto { Id = ReportElementId, IsCorrect = false, Message = "Incorrect sensor type for this element" };
            }

            List<dynamic> items = values.ToList();
            for (int i = 0; i < items.Count(); i++)
            {
                if (values.Count() == 1 || i == 0)
                {
                    columnRange.Dates.Add(items[i].Date);
                    columnRange.MinValues.Add(items[i].Min);
                    columnRange.MaxValues.Add(items[i].Max);
                    continue;
                }
                if (i > 0 && !items[i].Equals(items[i - 1]))
                {
                    columnRange.Dates.Add(items[i].Date);
                    columnRange.MinValues.Add(items[i].Min);
                    columnRange.MaxValues.Add(items[i].Max);
                }
            }
            return columnRange;
        }

        public async Task<ReportElementDto> GetDataForTimeSeries(int reportElementId)
        {
            var reportElement = await unitOfWork.ReportElementRepo.GetById(reportElementId);
            if (reportElement == null) return null;

            DateTimeOffset date = new DateTimeOffset(1970, 1, 1, 0, 0, 0,
              new TimeSpan(0, 0, 0));
            if (reportElement.Hours != 0)
                date = DateTimeOffset.Now.AddHours(-(int)reportElement.Hours);

            var histories = await unitOfWork.HistoryRepo.GetHistoriesBySensorIdAndDate(reportElement.SensorId.Value, date);
            if (histories.Count() == 0 || histories == null)
                return new ReportElementDto
                {
                    Id = reportElementId,
                    SensorName = reportElement.Sensor.Name,
                    IsCorrect = false
                };

            var milliseconds = GetMilliseconds(histories).ToList();
            var values = GetValues(histories).ToList();

            ReportElementDto data = mapper.Map<ReportElement, ReportElementDto>(reportElement);
            data.Milliseconds = milliseconds;
            data.Values = values;

            return data;
        }

        private IEnumerable<dynamic> GetValues(IEnumerable<History> histories)
        {
            foreach (var items in histories)
            {
                if (items.Sensor.SensorType.MeasurementType == MeasurementType.Int)
                    yield return items.IntValue.Value;
                else
                if (items.Sensor.SensorType.MeasurementType == MeasurementType.Double)
                    yield return items.DoubleValue.Value;
                else
                 if (items.Sensor.SensorType.MeasurementType == MeasurementType.Bool)
                    yield return items.BoolValue.Value ? 1 : 0;
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
            await unitOfWork.ReportElementRepo.Update(result);
            unitOfWork.Save();
        }

        public async Task Delete(ReportElement reportElement)
        {
            await unitOfWork.ReportElementRepo.Delete(reportElement);
            unitOfWork.Save();
        }

        public async Task Lock(ReportElement reportElement)
        {
            ReportElement result = await unitOfWork.ReportElementRepo.GetById(reportElement.Id);
            result.IsLocked = !reportElement.IsLocked;
            await unitOfWork.ReportElementRepo.Update(result);
            unitOfWork.Save();
        }

        public async Task<ReportElementDto> GetStatusReport(int ReportElementId)
        {
            ReportElement reportElementt = await unitOfWork.ReportElementRepo.GetById(ReportElementId);
            if (reportElementt == null)
                return new ReportElementDto { IsCorrect = false, Message = "Invalid report element" };

            ReportElementDto reportElement = mapper.Map<Sensor, ReportElementDto>(reportElementt.Sensor);
            IEnumerable<Sensor> sensors = await unitOfWork.SensorRepo.GetAllSensorsByUserId(UserId);

            foreach (Sensor sensor in sensors)
            {
                reportElement.Dates.Add(sensor.Name);
                History history = unitOfWork.HistoryRepo.GetLastHistoryBySensorId(sensor.Id);
                dynamic value = null;
                switch (reportElement.MeasurementType)
                {
                    case MeasurementType.Int:
                        value = history.IntValue.GetValueOrDefault();
                        break;
                    case MeasurementType.Bool:
                        value = history.BoolValue.GetValueOrDefault();
                        if (value == true)
                            value = "Active";
                        else
                            value = "Inactive";

                        break;
                    case MeasurementType.Double:
                        value = Math.Round(history.DoubleValue.GetValueOrDefault(), 2);
                        break;
                    case MeasurementType.String:
                        value = history.StringValue;
                        break;
                    default:
                        return new ReportElementDto { Id = ReportElementId, IsCorrect = false, Message = "Incorrect sensor type for this element" };
                }
                reportElement.Values.Add(value);
            }

            return reportElement;
        }

        public Task<SensorDto> GetLastSensorByUserId(string userId)
        {
            return historyManager.GetLastSensorByUserId(userId);
        }
    }
}
