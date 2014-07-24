using System;

namespace RestEndpoints.Core.Models
{
    public class ContractDescriptor
    {
        private readonly Action<object> dispatch = _ => {};

        public string Endpoint { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }

        public string ResponseMessage { get; set; }

        public void Dispatch(object message)
        {
            this.dispatch(message);
        }
    }
}
