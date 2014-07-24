using System.Net.Http;
using System.Web.Http;
using RestEndpoints.Core.Models;

namespace RestEndpoints.Core
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

        public HttpResponseMessage Post(string endpointName, string contractName, ContractInstance instance)
        {
            var descriptor = endpoints.FindContract(endpointName, contractName);
            var message = instance.ToInstanceOf(descriptor.Type);
            descriptor.Receive(message);
            return Request.CreateResponse(descriptor);
        }
    }
}
