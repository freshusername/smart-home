using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.Model
{
    public class Message
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public bool IsRead { get; set; }

        public int HistoryId { get; set; }

        public History History { get; set; }
        public AppUser AppUser { get; set; }
    }
}
