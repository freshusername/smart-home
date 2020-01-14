using Infrastructure.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.History
{
	public class AllHistoriesViewModel
	{
		public IEnumerable<HistoryViewModel> Histories { get; set; }
        public FilterDto FilterDto { get; set; }
    }
}
