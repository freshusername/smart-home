using Domain.Core.Model.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.SensorViewModel
{
    public class EditSensorViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Comment { get; set; }

        public int IconId { get; set; }
        public int SensorTypeId { get; set; }

        public string IconPath { get; set; }

        [DisplayName("IconFile")]
        public IFormFile IconFile { get; set; }
    }
}
