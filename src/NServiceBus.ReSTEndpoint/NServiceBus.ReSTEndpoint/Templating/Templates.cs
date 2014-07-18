using System.IO;
using System.Reflection;

namespace NServiceBus.ReSTEndpoint.Templating
{
    public class Templates
    {
        public static string AllEndpoints
        {
            get
            {
                return
                    GetResource(
                        "NServiceBus.ReSTEndpoint.Templating.endpoints-all.cshtml");
            }
        }

        public static string AllCommands
        {
            get
            {
                return
                    GetResource(
                        "NServiceBus.ReSTEndpoint.Templating.commands-all.cshtml");
            }
        }

        public static string CommandSender
        {
            get
            {
                return
                    GetResource(
                        "NServiceBus.ReSTEndpoint.Templating.command-sender.cshtml");
            }
        }

        private static string GetResource(string resourceName)
        {
            using (var stream = Assembly.GetAssembly(typeof(Templates)).GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }        
    }
}