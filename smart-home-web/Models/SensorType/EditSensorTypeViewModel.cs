using Domain.Core.Model.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace smart_home_web.Models.SensorType
{
	public class EditSensorTypeViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Comment { get; set; }

        [Required]
        [DisplayName("Measurement Type")]
        public MeasurementType MeasurementType { get; set; }

        [Required]
        [DisplayName("Measurement Name")]
        public string MeasurementName { get; set; }

        public int IconId { get; set; }

        public string IconPath { get; set; }

        [DisplayName("Icon")]
        public IFormFile IconFile { get; set; }
    }
}
