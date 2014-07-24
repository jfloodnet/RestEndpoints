using System;

namespace RestEndpoints.Core.Models
{
    public class ContractDescriptor
    {
        private readonly Action<object> receiveAction = _ => {};

        public string Endpoint { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }

        public string ResponseMessage { get; set; }

        public void Receive(object message)
        {
            receiveAction(message);
        }
    }
}
