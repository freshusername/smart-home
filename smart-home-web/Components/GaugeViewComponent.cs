﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Components
{
    public class GaugeViewComponent : BaseViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}