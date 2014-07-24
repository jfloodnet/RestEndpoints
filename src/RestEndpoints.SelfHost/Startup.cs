using Newtonsoft.Json;
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
using RestEndpoints.Core.Configuration;
using RestEndpoints.Core;

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
                new Endpoints()
                    .ForEndpoint("Billing")
                        .LookIn(typeof(Program).Assembly)
                        .For(BillingContracts())                       

                    .ForEndpoint("Sales")
                        .LookIn(typeof(Program).Assembly)
                        .For(SalesContracts())

                    .ForEndpoint("Clients")
                        .LookIn(typeof(Program).Assembly)
                        .For(ClientContracts()                       
                     )

                     .WhenMessageReceived((endpointName, message) =>
                         Console.WriteLine(
                         "Dispatched to '{0}', message: {1}", 
                         endpointName, 
                         JsonConvert.SerializeObject(message)))
                ));

            appBuilder.UseWebApi(config); 
        }

        private Func<Type, bool> ClientContracts()
        {
            return x => typeof(Clients).IsAssignableFrom(x) && x != typeof(Clients);
        }

        private Func<Type, bool> BillingContracts()
        {
            return x => typeof(Billing).IsAssignableFrom(x) && x != typeof(Billing);
        }

        private Func<Type, bool> SalesContracts()
        {
            return x => typeof(Sales).IsAssignableFrom(x) && x != typeof(Sales);
        }

        private void LoadEndpointControllerAssembly()
        {
            Type endpoint = typeof(EndpointController);
        } 
    }
}
