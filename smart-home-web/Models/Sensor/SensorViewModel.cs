﻿using System;

namespace smart_home_web.Models.SensorViewModel
{
    public class SensorViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public Guid Token { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public bool IsValid { get; set; }
        public bool IsActive { get; set; }

        public int SensorTypeId { get; set; }
        public int? IconId { get; set; }
        public string AppUserId { get; set; }
        public string IconPath { get; set; }
        public string SensorTypeName { get; set; }

    }
}
