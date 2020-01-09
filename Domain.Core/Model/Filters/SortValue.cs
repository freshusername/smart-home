using Domain.Core.Model;
using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Domain.Core.Filters
{
    public class SortValue
    {
		//TODO: Remove spare SortValue 
        public static IQueryable<History> SortHistories(SortState sortState, IQueryable<History> histories)
        {

            switch (sortState)
            {
                case SortState.SensorAsc:
                    histories = histories.OrderBy(p => p.SensorId);
                    break;
                case SortState.SensorDesc:
                    histories = histories.OrderByDescending(p => p.SensorId);
                    break;
                case SortState.DateAsc:
                    histories = histories.OrderBy(p => p.Date);
                    break;
                case SortState.DateDesc:
                    histories = histories.OrderByDescending(p => p.Date);
                    break;
                case SortState.StringValueAsc:
                    histories = histories.OrderBy(p => p.StringValue);
                    break;
                case SortState.StringValueDesc:
                    histories = histories.OrderByDescending(p => p.StringValue);
                    break;
                case SortState.IntValueAsc:
                    histories = histories.OrderBy(p => p.IntValue);
                    break;
                case SortState.IntValueDesc:
                    histories = histories.OrderByDescending(p => p.IntValue);
                    break;
                case SortState.DoubleValueAsc:
                    histories = histories.OrderBy(p => p.DoubleValue);
                    break;
                case SortState.DoubleValueDesc:
                    histories = histories.OrderByDescending(p => p.DoubleValue);
                    break;
                case SortState.BoolValueAsc:
                    histories = histories.OrderBy(p => p.BoolValue);
                    break;
                case SortState.BoolValueDesc:
                    histories = histories.OrderByDescending(p => p.BoolValue);
                    break;
                case SortState.HistoryAsc:
                    histories = histories.OrderBy(p => p.Id);
                    break;
                case SortState.HistoryDesc:
                    histories = histories.OrderByDescending(p => p.Id);
                    break;
                default:
                    histories = histories.OrderBy(p => p.Id);
                    break;
            }
            return histories;
        }
    }
}
