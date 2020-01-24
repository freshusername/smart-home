using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace smart_home_web.Models.SensorViewModel
{
	public class CreateSensorViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Comment field is required.")]
        [Display(Name = "Comment")]
        [StringLength(50)]
        public string Comment { get; set; }

        public int? IconId { get; set; }

        [Required(ErrorMessage = "The Sensor type is required.")]
        [Display(Name = "Sensor type")]
        public int SensorTypeId { get; set; }

        public Guid Token { get; set; }

        public DateTimeOffset? CreatedOn { get; set; }
        public bool IsActivated { get; set; }
        public IFormFile IconFile { get; set; }

    }
}
