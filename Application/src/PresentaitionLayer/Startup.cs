using System;
using System.Net.WebSockets;
using System.Threading;
using ApplicationCore.Entities.Users;
using ApplicationCore.Interfaces.Infastracture;
using ApplicationCore.Interfaces.ServiceLayer;
using DataAccessLayer;
using DataAccessLayer.DAOs;
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
        UnitOfWork _unitOfWork;

        public Startup(IConfiguration configuration, IServiceFacade facade, ILogger<Startup> logger,
            NotificationsCenter notificationsCenter, PipelineManager pipelineManager, UnitOfWork unitOfWork)
        {
            Configuration = configuration;
            _facade = facade;
            _logger = logger;
            _notificationsCenter = notificationsCenter;
            _pipelineManager = pipelineManager;
            _unitOfWork = unitOfWork;
            //_unitOfWork.UserRepository.Create(new BaseUserDAO(new BaseUser("user", "213", false)));
            //_unitOfWork.Save();
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

            var g = Guid.NewGuid();
            _facade.Initialize(g, "meni", "moni");
            _facade.Register(Guid.NewGuid(), "ooo", "1111");
            _logger.LogDebug(_facade.Register(Guid.NewGuid(), "myUser", "1111").ToString());
            _facade.Logout(g);
            init_data();
            UpdateCenter.Subscribe(_notificationsCenter.HandleUpdate);
        }

        private void init_data()
        {
            var dummySession = new Guid();
            _facade.Register(dummySession, "ben", "ben");
            _facade.Login(dummySession, "ben", "ben");
            _facade.ChangeUserState(dummySession, "SellerUserState");
            var shop_guid = _facade.OpenShop(dummySession, "Ben's groceries");
            _facade.AddProductToShop(dummySession, shop_guid, "Banana", "good things", 3.0, 120);
            _facade.AddProductToShop(dummySession, shop_guid, "Mango", "nice things", 12.0, 30);
            _facade.Logout(dummySession);
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
