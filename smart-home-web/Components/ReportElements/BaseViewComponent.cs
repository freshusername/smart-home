using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Components.ReportElements
{
    public abstract class BaseViewComponent : ViewComponent
    {
        public int DashboardId { get; set; }
    }
}
