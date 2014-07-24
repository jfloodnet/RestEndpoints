using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using RestEndpoints.Core.Formatters;
using RestEndpoints.Core.Models;
using RestEndpoints.Core.Templating;

namespace RestEndpoints.Core.Configuration
{
    public static class ConfigExtensions
    {
        public static HttpConfiguration ConfigureContractsEndpoint(this HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
               name: "Endpoint",
               routeTemplate: "{endpointName}/{contractName}",
               defaults: new 
               { 
                   controller = "Endpoint", 
                   endpointName = RouteParameter.Optional, 
                   contractName = RouteParameter.Optional 
               }
           );

            config.Formatters.Insert(0, new ContractInstanceFormDataFormatter());
            config.Formatters.Insert(1, new ContractInstanceJsonFormatter());

            config.Formatters.Add(
                new RazorMediaTypeFormatter<string[]>(
                Templates.AllEndpoints, new MediaTypeHeaderValue("text/html")));            
            config.Formatters.Add(
                new RazorMediaTypeFormatter<ContractDescriptor[]>(
                    Templates.AllCommands, new MediaTypeHeaderValue("text/html")));
            config.Formatters.Add(
                new RazorMediaTypeFormatter<ContractDescriptor>(
                    Templates.CommandSender, new MediaTypeHeaderValue("text/html")));
            return config;
        }
    }
}