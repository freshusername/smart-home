using Domain.Core.Model;
using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Infrastructure.Business.Managers;
using Infrastructure.Business.DTOs.History;

namespace Infrastructure.Business.Filters
{
    public class SortValue
    {

        public static IEnumerable<HistoryDto> SortHistories(SortState sortState, IEnumerable<HistoryDto> histories)
        {

            switch (sortState)
            {
                case SortState.SensorAsc:
                    histories = histories.OrderBy(p => p.SensorId).ToList();
                    break;
                case SortState.SensorDesc:
                    histories = histories.OrderByDescending(p => p.SensorId).ToList();
                    break;
                case SortState.DateAsc:
                    histories = histories.OrderBy(p => p.Date).ToList();
                    break;
                case SortState.DateDesc:
                    histories = histories.OrderByDescending(p => p.Date).ToList();
                    break;
                case SortState.StringValueAsc:
                    histories = histories.OrderBy(p => p.StringValue).ToList();
                    break;
                case SortState.StringValueDesc:
                    histories = histories.OrderByDescending(p => p.StringValue).ToList();
                    break;
                case SortState.IntValueAsc:
                    histories = histories.OrderBy(p => p.IntValue).ToList();
                    break;
                case SortState.IntValueDesc:
                    histories = histories.OrderByDescending(p => p.IntValue).ToList();
                    break;
                case SortState.DoubleValueAsc:
                    histories = histories.OrderBy(p => p.DoubleValue).ToList();
                    break;
                case SortState.DoubleValueDesc:
                    histories = histories.OrderByDescending(p => p.DoubleValue).ToList();
                    break;
                case SortState.BoolValueAsc:
                    histories = histories.OrderBy(p => p.BoolValue).ToList();
                    break;
                case SortState.BoolValueDesc:
                    histories = histories.OrderByDescending(p => p.BoolValue).ToList();
                    break;
                case SortState.HistoryAsc:
                    histories = histories.OrderBy(p => p.Id).ToList();
                    break;
                case SortState.HistoryDesc:
                    histories = histories.OrderByDescending(p => p.Id).ToList();
                    break;
                default:
                    histories = histories.OrderBy(p => p.SensorId).ToList();
                    break;
            }
            return histories;
        }
    }
}
