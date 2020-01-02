using Infrastructure.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.History
{
    public class InvalidSensorsViewModel
    {
        public IEnumerable<HistoryViewModel> Histories { get; set; }
        public PaginationDTO paginationDTO { get; set; }
    }
}
