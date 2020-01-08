using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Components
{
    public abstract class BaseViewComponent : ViewComponent
    {
        public int DashboardId { get; set; }
        public int SensorId { get; set; }
    }
}
