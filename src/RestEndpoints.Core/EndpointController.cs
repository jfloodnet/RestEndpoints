using System;
using System.Linq;
using System.Net;
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
            var contracts = endpoints.ContractsIn(endpointName);

            if (!contracts.Any())
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, endpoints.AllEndpoints());
            }

            return Request.CreateResponse(endpoints.ContractsIn(endpointName));
        }

        public HttpResponseMessage Get(string endpointName, string contractName)
        {
            var contract = endpoints.FindContract(endpointName, contractName);

            if (contract == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, endpoints.AllEndpoints());
            }

            return Request.CreateResponse(contract);
        }

        public HttpResponseMessage Post(string endpointName, string contractName, ContractInstance instance)
        {
            var descriptor = endpoints.FindContract(endpointName, contractName);
            
            try
            {
                var message = instance.CreateMessage(descriptor.Type);

                this.endpoints.Dispatch(endpointName, message);

                return Request.CreateResponse(descriptor);
            }
            catch (MessageInstantiationException ex)
            {
                descriptor.ErrorMessages = ex.ErrorMessages;

                return Request.CreateResponse(descriptor);
            }
        }
    }
}
