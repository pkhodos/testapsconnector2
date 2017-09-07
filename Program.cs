using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace FallballConnectorDotNet
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseAzureAppServices()
                .UseStartup<Startup>()
                .UseSetting("detailedErrors", "true")
                .CaptureStartupErrors(false)
                .Build();
            
            host.Run();
        }
    }
}