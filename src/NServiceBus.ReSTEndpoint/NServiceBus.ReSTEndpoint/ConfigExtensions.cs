using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using NServiceBus.ReSTEndpoint.MediaTypeFormmatters;
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
               routeTemplate: "{commandName}",
               defaults: new { controller = "Endpoint", commandName = RouteParameter.Optional }
           );

            config.Formatters.Add(new FormUrlEncodedMediaTypeFormatter());
            config.Formatters.Add(
                new RazorMediaTypeFormatter<CommandDescriptor[]>(
                    Templates.AllCommands, new MediaTypeHeaderValue("text/html")));
            config.Formatters.Add(
                new RazorMediaTypeFormatter<CommandDescriptor>(
                    Templates.CommandSender, new MediaTypeHeaderValue("text/html")));
            return config;
        }
    }
}