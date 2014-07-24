using System.Diagnostics;
using System.Net.Http.Headers;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBus.ReSTEndpoint.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", "http://localhost:9000");

                
                Console.ReadLine(); 
            }
        }
    }
}
