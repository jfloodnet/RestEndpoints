using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace NServiceBus.ReSTEndpoint
{
    public class EndpointController : ApiController
    {
        private readonly Contracts contracts;

        public EndpointController(Contracts  contracts)
        {
            this.contracts = contracts;
        }

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(contracts.AllCommands());
        }

        public HttpResponseMessage Get(string commandName)
        {
            return Request.CreateResponse(contracts.FindCommand(commandName));
        }

        public HttpResponseMessage Post(FormDataCollection formData)
        {
            var descriptor = contracts.FindCommand(formData["CommandName"]);
            var command = Activator.CreateInstance(descriptor.CommandType);

            foreach (var property in descriptor.CommandType.GetProperties())
            {
                property.SetValue(command, Convert.ChangeType(formData[property.Name], property.PropertyType));
            }

            //bus.Send(command); !woo

            return Request.CreateResponse(descriptor);
        }
    }
}
