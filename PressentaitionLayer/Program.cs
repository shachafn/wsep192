using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using ServiceLayer;

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
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.File(path: "C://Wsep192//Logs//info-logs.txt", restrictedToMinimumLevel: LogEventLevel.Debug)
                .WriteTo.File(path: "C://Wsep192//Logs//error-logs.txt", restrictedToMinimumLevel: LogEventLevel.Error)
                .WriteTo.Console(
                    LogEventLevel.Verbose,
                    "{NewLine}{Timestamp:HH:mm:ss} [{Level}] ({CorrelationToken}) {Message}{NewLine}{Exception}")
                    .CreateLogger();
        }
    }
}
