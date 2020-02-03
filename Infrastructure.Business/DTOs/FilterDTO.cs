using System;
using Domain.Core.Model.Enums;

namespace Infrastructure.Business.DTOs
{
    public class FilterDto : ICloneable
    {

        public SortState sortState { get; set; } = SortState.HistoryAsc;
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 999999; // In case we use custom pagination and sorting, this should be 15
        public int Amount { get; set; } 
        public int PagesCount => (int)Math.Ceiling(decimal.Divide(Amount, PageSize));

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage <= PagesCount - 1;

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public int sensorId { get; set;}
    }
}
