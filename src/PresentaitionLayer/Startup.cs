using System;
using ApplicationCore.Interfaces.ServiceLayer;
using DomainLayer;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceLayer;

namespace PresentaitionLayer
{
    public class Startup
    {
        IServiceFacade _facade;
        ILogger<Startup> _logger;
        NotificationsCenter _notificationsCenter;
        PipelineManager _pipelineManager;
        SystemInitializer _systemInitializer;

        public Startup(IConfiguration configuration, IServiceFacade facade, ILogger<Startup> logger,
            NotificationsCenter notificationsCenter, PipelineManager pipelineManager
            , SystemInitializer systemInitializer)
        {
            Configuration = configuration;
            _facade = facade;
            _logger = logger;
            _notificationsCenter = notificationsCenter;
            _pipelineManager = pipelineManager;
            _systemInitializer = systemInitializer;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _logger.LogDebug("Configuring Services");
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAuthentication(options => {

                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(options =>
            {
                options.LoginPath = "/auth/login";
                options.LogoutPath = "/auth/logout";
            });
            services.AddSession();
            if (!_systemInitializer.InitSystem())
                throw new Exception();
            _systemInitializer.InitSystemWithFile();
            UpdateCenter.Subscribe(_notificationsCenter.HandleUpdate);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };

            app.UseWebSockets(webSocketOptions);

            app.Use(async (context, next) =>
            {
                await _pipelineManager.HandleHttpRequest(context, next);
            });
        }
    }
}
