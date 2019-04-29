using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.DomainLayer;
using ApplicationCore.Interfaces.ServiceLayer;
using DomainLayer.Domains;
using DomainLayer.Facade;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PressentaitionLayer.Services;
using ServiceLayer;
using Microsoft.Extensions.Logging;

namespace PressentaitionLayer
{
    public class Startup
    {
        IServiceFacade _facade;
        ILogger<Startup> _logger;
        public Startup(IConfiguration configuration, IServiceFacade facade, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _facade = facade;
            _logger = logger;
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
            //services.AddAuthorization(options=> options.AddPolicy("BuyerOnly",policy=>policy.RequireRole("Buyer")));

            _facade.Initialize(new Guid(), "meni", "moni");
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
        }
    }
}
