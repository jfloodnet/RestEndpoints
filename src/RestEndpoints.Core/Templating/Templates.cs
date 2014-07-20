using System.IO;
using System.Reflection;

namespace RestEndpoints.Core.Templating
{
    public class Templates
    {
        private const string NAMESPACE = "RestEndpoints.Core.Templating";
        public static string AllEndpoints
        {
            get
            {
                return
                    GetResource(
                        NAMESPACE +".endpoints-all.cshtml");
            }
        }

        public static string AllCommands
        {
            get
            {
                return
                    GetResource(
                        NAMESPACE + ".commands-all.cshtml");
            }
        }

        public static string CommandSender
        {
            get
            {
                return
                    GetResource(
                        NAMESPACE + ".command-sender.cshtml");
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