using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.IconViewModel
{
    public class CreateIconViewModel
    {
        public int Id { get; set; }
        public IFormFile ImageFile { get; set; }

    }
}
