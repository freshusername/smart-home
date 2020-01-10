using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models;

namespace smart_home_web.Controllers
{
    public class ReportElementController : Controller
    {
        [HttpGet]
        public IActionResult Wordcloud()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Wordcloud(GraphViewModel model)
        {
            return View();
        }
    }
}