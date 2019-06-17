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
using ApplicationCore.Interfaces.Infastracture;
using ApplicationCore.Interfaces.ExternalServices;
using DomainLayer.External_Services;
using Infrastructure.ExternalServices;
using DataAccessLayer;
using ApplicationCore.Interfaces.DataAccessLayer;
using MongoDB.Bson.Serialization;
using DomainLayer.Policies;
using DomainLayer.Operators;

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
            //Notice, the order is crucial
            SetupDbConfiguration(services);
            SetupDbInjections(services);
            SetupExternalServicesInjection(services);
            SetupDomainLayerInjections(services);
            SetupServiceLayerInjections(services);
            SetupPipeLiningInjections(services);
            SetupInfrastructureInjections(services);
            SetupPresentationLayerServicesInjections(services);
        }
        private static void SetupDbInjections(IServiceCollection services)
        {
            SetupClassRegistrations();
            services.AddScoped<IContext, MongoDbContext>();
            services.AddScoped<MongoDbContext>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
        }

        private static void SetupClassRegistrations()
        {
            BsonClassMap.RegisterClassMap<CartDiscountPolicy>();
            BsonClassMap.RegisterClassMap<ProductDiscountPolicy>();
            BsonClassMap.RegisterClassMap<UserDiscountPolicy>();
            BsonClassMap.RegisterClassMap<CompositeDiscountPolicy>();

            BsonClassMap.RegisterClassMap<CartPurchasePolicy>();
            BsonClassMap.RegisterClassMap<UserPurchasePolicy>();
            BsonClassMap.RegisterClassMap<ProductPurchasePolicy>();
            BsonClassMap.RegisterClassMap<CompositePurchasePolicy>();

            BsonClassMap.RegisterClassMap<BiggerThan>();
            BsonClassMap.RegisterClassMap<SmallerThan>();

            BsonClassMap.RegisterClassMap<Implies>();
            BsonClassMap.RegisterClassMap<Xor>();
            BsonClassMap.RegisterClassMap<Or>();
            BsonClassMap.RegisterClassMap<And>();
        }

        private static void SetupDbConfiguration(IServiceCollection services)
        {
            IConfigurationRoot configuration = GetConfigurationAccordingToEnvironmentVariable();
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddConfiguration(configuration);
            var config = configurationBuilder.Build();
            var dbSection = config.GetSection(nameof(DatabaseConfiguration));

            services.AddSingleton(new DatabaseConfiguration(dbSection.GetValue<string>("ConnectionString"), dbSection.GetValue<string>("DatabaseName")));
        }

        private static void SetupPresentationLayerServicesInjections(IServiceCollection services)
        {
            services.AddScoped<UserServices>();
        }

        private static void SetupInfrastructureInjections(IServiceCollection services)
        {
            services.AddSingleton<NotificationsCenter>();
        }

        private static void SetupPipeLiningInjections(IServiceCollection services)
        {
            services.AddTransient<PipelineManager>();
            services.AddTransient<WebSocketsManager>();
            services.AddTransient<NotificationsWebSocketsManager>();
            services.AddTransient<IUserNotifier, NotificationsWebSocketsManager>();

        }

        private static void SetupDomainLayerInjections(IServiceCollection services)
        {
            services.AddScoped<ShoppingBagDomain>();
            services.AddScoped<ShopDomain>();
            services.AddScoped<IUserDomain, UserDomain>();
            services.AddScoped<DomainLayerFacadeVerifier>();
            services.AddSingleton<IExternalServicesManager, ExternalServicesManager>();
            services.AddScoped<DomainLayerFacade>();
            services.AddScoped<IDomainLayerFacade, DomainFacadeTransactionProxy>();
        }

        private static void SetupServiceLayerInjections(IServiceCollection services)
        {
            services.AddSingleton<ISessionManager, SessionManager>();
            services.AddScoped<ServiceFacade>();
            services.AddScoped<IServiceFacade, ServiceFacadeProxy>();
            services.AddSingleton<SystemInitializer>();
        }

        private static void SetupExternalServicesInjection(IServiceCollection services)
        {
            services.AddSingleton<IPaymentSystem, PaymentService>();
            services.AddSingleton<ISupplySystem, SupplyService>();
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
