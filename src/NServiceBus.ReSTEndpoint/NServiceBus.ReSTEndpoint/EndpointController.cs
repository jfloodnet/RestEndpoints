using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace NServiceBus.ReSTEndpoint
{
    public class EndpointController : ApiController
    {
        private readonly Endpoints endpoints;

        public EndpointController(Endpoints endpoints)
        {
            this.endpoints = endpoints;
        }

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(endpoints.AllEndpoints());
        }

        public HttpResponseMessage Get(string endpointName)
        {
            return Request.CreateResponse(endpoints.ContractsIn(endpointName));
        }

        public HttpResponseMessage Get(string endpointName, string contractName)
        {
            return Request.CreateResponse(endpoints.FindContract(endpointName, contractName));
        }

        public HttpResponseMessage Post(string endpointName, string contractName, FormDataCollection formData)
        {
            var descriptor = endpoints.FindContract(endpointName, contractName);
            var command = Activator.CreateInstance(descriptor.Type);

            foreach (var property in descriptor.Type.GetProperties())
            {
                property.SetValue(command, Convert.ChangeType(formData[property.Name], property.PropertyType));
            }

            //bus.Send(command); !woo

            return Request.CreateResponse(descriptor);
        }
    }
}
