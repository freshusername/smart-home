using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.Model
{
    public class Dashboard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public ICollection<ReportElement> ReportElements { get; set; }
    }
}
