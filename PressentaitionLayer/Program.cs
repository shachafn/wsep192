using System;
using System.IO;
using ApplicationCore.Interfaces.DomainLayer;
using ApplicationCore.Interfaces.ServiceLayer;
using DomainLayer.Domains;
using DomainLayer.Facade;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PressentaitionLayer.Services;
using ServiceLayer;
using Microsoft.AspNetCore.Builder;
using Serilog;

namespace PressentaitionLayer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SetupLogging();
            try
            {
                var host = CreateWebHostBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .ConfigureServices(BuildApplicationServices);

                host.Build().Run();
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

        private static void BuildApplicationServices(IServiceCollection services)
        {
            //Notice, the order of adding is crucial
            services.AddSingleton<IUserDomain, UserDomain>();
            services.AddSingleton<DomainLayerFacadeVerifier>();
            services.AddSingleton<IDomainLayerFacade, DomainLayerFacade>();
            services.AddSingleton<SessionManager>();
            services.AddSingleton<ServiceFacade>();
            services.AddSingleton<IServiceFacade, ServiceFacadeProxy>();
            services.AddSingleton<UserServices>();
            services.AddTransient<MyHub>();
        }

        private static void SetupLogging()
        {
            IConfigurationRoot configuration = GetConfigurationAccordingToEnvironmentVariable();
            Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(configuration)
                            .CreateLogger();
        }

        private static IConfigurationRoot GetConfigurationAccordingToEnvironmentVariable()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder();
            if (env.Equals("Development"))
                builder.AddJsonFile("appsettings.Development.json");
            else
                builder.AddJsonFile($"appsettings.json");

            return builder.Build();
        }
    }
}
