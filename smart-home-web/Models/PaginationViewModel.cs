using Infrastructure.Business.DTOs;

namespace smart_home_web.Models
{
	public class PaginationViewModel
    {
        public FilterDto filterDto { get; set; }
        public string action { get; set; }
        public string controller { get; set; }
    }
}
