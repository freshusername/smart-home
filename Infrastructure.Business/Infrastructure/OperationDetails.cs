using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.Infrastructure
{
    public class OperationDetails
    {
        public OperationDetails(bool succedeed, string message, string prop, Dictionary<string, object> data)
        {
            Succeeded = succedeed;
            Message = message;
            Property = prop;
            Data = data ?? new Dictionary<string, object>();
        }

        public OperationDetails(bool succedeed, string message, string prop) : this(succedeed, message, prop, null)
        {
        }

        public OperationDetails(bool succedeed) : this(succedeed, "", "", null)
        {
        }

        public bool Succeeded { get; private set; }
        public string Message { get; private set; }
        public string Property { get; private set; }
        public Dictionary<string, object> Data { get; set; }

    }
}
