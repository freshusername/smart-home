using Domain.Core.Model.Enums;
using System.ComponentModel;

namespace smart_home_web.Models.SensorType
{
	public class SensorTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        [DisplayName("Measurement Type")]
        public MeasurementType MeasurementType { get; set; }
        [DisplayName("Measurement Name")]
        public string MeasurementName { get; set; }
        [DisplayName("Icon")]
        public string IconPath { get; set; }
    }
}
