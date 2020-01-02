using Domain.Core.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.SensorViewModel
{
    public class CreateSensorViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Comment field is required.")]
        [Display(Name = "Additional info")]
        [StringLength(50)]
        public string Comment { get; set; }

        public int IconId { get; set; }
        public int SensorTypeId { get; set; }
        public Guid Token { get; set; }

        public DateTimeOffset? CreatedOn { get; set; }
        public bool IsActivated { get; set; }
        public IFormFile IconFile { get; set; }

    }
}
