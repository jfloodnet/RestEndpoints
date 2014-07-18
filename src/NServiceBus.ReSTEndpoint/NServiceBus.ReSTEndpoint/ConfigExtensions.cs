using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using NServiceBus.ReSTEndpoint.MediaTypeFormatters;
using NServiceBus.ReSTEndpoint.Models;
using NServiceBus.ReSTEndpoint.Templating;

namespace NServiceBus.ReSTEndpoint
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
            config.Formatters.Add(new FormUrlEncodedMediaTypeFormatter());
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