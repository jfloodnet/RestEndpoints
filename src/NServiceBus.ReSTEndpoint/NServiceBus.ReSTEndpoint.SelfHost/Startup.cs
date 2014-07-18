using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace NServiceBus.ReSTEndpoint.SelfHost
{
    public class Startup 
    { 
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder) 
        {
            LoadEndpointControllerAssembly();

            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            config.ConfigureContractsEndpoint();
            
            config.Routes.MapHttpRoute( 
                name: "DefaultApi", 
                routeTemplate: "api/{controller}/{id}", 
                defaults: new { id = RouteParameter.Optional } 
            );

            config.Services.Replace(typeof(IHttpControllerActivator), new ControllerFactory(                
                Contracts.LookIn(typeof(Program).Assembly)
                .For(x => typeof(Command).IsAssignableFrom(x) && x != typeof(Command))
                )); 


            
            appBuilder.UseWebApi(config); 
        }

        private void LoadEndpointControllerAssembly()
        {
            Type endpoint = typeof(EndpointController);
        } 
    }

    public class ControllerFactory : IHttpControllerActivator
    {
        Contracts contracts;
        public ControllerFactory(Contracts contracts)
        {
            this.contracts = contracts;
        }
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            return new EndpointController(contracts);
        }
    }
}
