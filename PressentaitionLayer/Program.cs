using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.DomainLayer;
using ApplicationCore.Interfaces.ServiceLayer;
using DomainLayer.Domains;
using DomainLayer.Facade;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PressentaitionLayer.Services;
using ServiceLayer;

namespace PressentaitionLayer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args)
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .ConfigureServices(BuildApplicationServices);

            host.Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

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
        }
    }
}
