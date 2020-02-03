using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Components.Others
{
    public class ModalsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}
