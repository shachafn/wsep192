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
using PresentaitionLayer.Services;
using ServiceLayer;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Infrastructure;
using Microsoft.AspNetCore.SignalR;
using ApplicationCore.Interfaces.Infastracture;
using ApplicationCore.Interfaces.ExternalServices;
using Infrastructure.gRPC.services;
using DomainLayer.External_Services;
using Infrastructure.ExternalServices;

namespace PresentaitionLayer
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
            //services.AddSingleton<IPaymentSystem, GRPCPaymentService>();
            services.AddSingleton<IPaymentSystem, PaymentService>();
            services.AddSingleton<ISupplySystem, SupplyService>();
            services.AddSingleton<IExternalServicesManager, ExternalServicesManager>();
            services.AddSingleton<IDomainLayerFacade, DomainLayerFacade>();
            services.AddSingleton<ISessionManager, SessionManager>();

            services.AddTransient<PipelineManager>();
            services.AddTransient<WebSocketsManager>();
            services.AddTransient<NotificationsWebSocketsManager>();

            services.AddTransient<IUserNotifier, NotificationsWebSocketsManager>();

            services.AddSingleton<NotificationsCenter>();

            services.AddSingleton<ServiceFacade>();
            services.AddSingleton<IServiceFacade, ServiceFacadeProxy>();
            services.AddSingleton<UserServices>();
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
