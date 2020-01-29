
using Domain.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Business.DTOs.ReportElements;
using Microsoft.AspNetCore.Http;

namespace smart_home_web.Models.Dashboard
{
    public class DashboardViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public string DashCreatorUserName { get; set; }
        public IFormFile IconFile { get; set; }

        public ICollection<ReportElementDto> ReportElements { get; set; }
    }
}
