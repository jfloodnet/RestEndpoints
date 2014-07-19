using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NServiceBus.ReSTEndpoint.Models
{
    public class ContractDescriptor
    {
        public string Endpoint { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }

        public string ResponseMessage { get; set; }
    }
}
