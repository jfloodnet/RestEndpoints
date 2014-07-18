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
                new Endpoints()
                    .Map("some.endpoint.1s")
                        .LookIn(typeof(Program).Assembly)
                        .For(Endpoint1Contracts())

                    .Map("some.endpoint.2")                
                        .LookIn(typeof(Program).Assembly)
                        .For(Endpoint2Contracts())
                ));
            
            appBuilder.UseWebApi(config); 
        }

        private Func<Type, bool> Endpoint1Contracts()
        {
            return x => typeof(Endpoint1).IsAssignableFrom(x) && x != typeof(Endpoint1);
        }

        private Func<Type, bool> Endpoint2Contracts()
        {
            return x => typeof(Endpoint2).IsAssignableFrom(x) && x != typeof(Endpoint2);
        }

        private void LoadEndpointControllerAssembly()
        {
            Type endpoint = typeof(EndpointController);
        } 
    }
}
