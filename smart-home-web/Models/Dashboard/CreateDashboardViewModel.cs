using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.Dashboard
{
    public class CreateDashboardViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public int? IconId { get; set; }
        public string IconPath { get; set; }
        public IFormFile IconFile { get; set; }
    }
}
