using Infrastructure.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models
{
    public class PaginationViewModel
    {
        public FilterDto filterDto { get; set; }
        public string action { get; set; }
        public string controller { get; set; }
    }
}
