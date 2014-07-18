using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NServiceBus.ReSTEndpoint.Models
{
    public class CommandDescriptor
    {
        public string CommandName { get; set; }
        public Type CommandType { get; set; }
    }
}
