using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.Dashboard
{
    public class CreateDashboardViewModel
    {
        [Required]
        public string Name { get; set; }

        [DisplayName("Is Public?")]
        public bool IsPublic { get; set; }

        public string IconPath { get; set; }
        public IFormFile IconFile { get; set; }
    }
}
