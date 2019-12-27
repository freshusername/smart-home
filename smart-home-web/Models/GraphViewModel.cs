using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models
{
    public class GraphViewModel<T>
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<T> Values { get; set; }
        public List<string> Dates { get; set; }
    }
}