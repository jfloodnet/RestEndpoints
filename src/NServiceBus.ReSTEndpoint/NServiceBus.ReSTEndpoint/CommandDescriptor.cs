using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NServiceBus.ReSTEndpoint
{
    public class CommandDescriptor
    {
        public string CommandName { get; set; }

        public string[] Properties { get; set; }
    }
}
