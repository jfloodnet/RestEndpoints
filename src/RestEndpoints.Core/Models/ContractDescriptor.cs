using System;
using System.Collections.Generic;
using System.Web;

namespace RestEndpoints.Core.Models
{
    public class ContractDescriptor
    {
        private readonly Action<object> dispatch = _ => {};

        public string Endpoint { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }

        public string[] ErrorMessages { get; set; }

        public ContractDescriptor()
        {
            ErrorMessages = new string []{};
        }

        public void Dispatch(object message)
        {
            this.dispatch(message);
        }
    }
}
