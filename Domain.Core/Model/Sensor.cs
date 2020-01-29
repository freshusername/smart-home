using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.Model
{
    public class Sensor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public Guid Token { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public bool IsValid { get; set; } = true;
        public bool IsActive { get; set; }

        public string AppUserId { get; set; }
        public int? SensorTypeId { get; set; }
        public int? IconId { get; set; }

        public AppUser User { get; set; }
        public SensorType SensorType { get; set; }
        public Icon Icon { get; set; }
        public virtual ICollection<History> Histories { get; set; }
        public virtual ICollection<ReportElement> ReportElements { get; set; }

        public virtual ICollection<SensorControl> SensorControls { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
