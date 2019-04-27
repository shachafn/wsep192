using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using ServiceLayer;

using Microsoft.AspNetCore.Builder;
using Serilog;


namespace PressentaitionLayer
{
    public class Program
    {
        public static ServiceFacadeProxy Service;
        public static void Main(string[] args)
        {
            SetupLogging();
            try
            {
                CreateWebHostBuilder(args).Build().Run();
                var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .Build();

                host.Run();
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();

        private static void SetupLogging()
        {
            IConfigurationRoot configuration = GetConfigurationAccordingToEnvironmentVariable();
            Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(configuration)
                            .CreateLogger();
        }

        private static IConfigurationRoot GetConfigurationAccordingToEnvironmentVariable()
        {
            var h = new WebHostBuilder();
            var environment = h.GetSetting("environment");
            var builder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true)
                    .AddEnvironmentVariables();
            return builder.Build();
        }
    }
}
