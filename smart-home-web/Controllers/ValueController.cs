using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValueController : ControllerBase
    {
        private readonly IHistoryManager
        public ValueController()
        {
                
        }
        [HttpGet("getdata")]
        public IActionResult AddHistory(Guid token , string value)
        {
     
            return Ok(token + value);

        }
    }
}
