using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.Model
{
    public class ReportElement
    {
        public int Id { get; set; }
        public ReportElementType Type { get; set; }
        public int Hours { get; set; }

        public int DashboardId { get; set; }
        public Dashboard Dashboard { get; set; }

        public int SensorId { get; set; }
        public Sensor Sensor { get; set; }

		public int X { get; set; }
		public int Y { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
	}
}
