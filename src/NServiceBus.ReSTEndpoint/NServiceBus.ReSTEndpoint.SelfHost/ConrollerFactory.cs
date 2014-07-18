using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace NServiceBus.ReSTEndpoint.SelfHost
{
    public class ControllerFactory : IHttpControllerActivator
    {
        Endpoints endpoints;
        public ControllerFactory(Endpoints endpoints)
        {
            this.endpoints = endpoints;
        }
        public IHttpController Create(
            HttpRequestMessage request,
            HttpControllerDescriptor controllerDescriptor,
            Type controllerType)
        {
            return new EndpointController(endpoints);
        }
    }
}
