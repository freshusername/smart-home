﻿using System;
using Domain.Core.Model.Enums;

namespace Infrastructure.Business.DTOs
{
    public class FilterDTO:ICloneable
    {

        public SortState sortState { get; set; } = SortState.None;
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 20;
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