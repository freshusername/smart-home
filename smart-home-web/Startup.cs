﻿using System;
using System.Reflection;
using AutoMapper;
using Domain.Core.Model;
using Domain.Interfaces.Repositories;
using Infrastructure.Business.Infrastructure;
using Infrastructure.Business.Interfaces;
using Infrastructure.Business.Services;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using smart_home_web.AutoMapper;
using Infrastructure.Business.Hubs;
using Infrastructure.Business.Managers;

namespace smart_home_web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationsDbContext>(options =>
                 options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("Infrastructure.Data")));

            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ApplicationsDbContext>().AddDefaultTokenProviders();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = Configuration["GoogleAuth:ClientId"];
                googleOptions.ClientSecret = Configuration["GoogleAuth:ClientSecret"];
            });
            services.AddAuthentication();
            services.AddSignalR();
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(1440);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.SlidingExpiration = true;
            });
            services.Configure<EmailOptionsModel>(Configuration.GetSection("EmailOptions"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddAutoMapper(typeof(AutoMapperProfile).GetTypeInfo().Assembly);

            services.AddTransient<IAuthenticationManager, AuthenticationManager>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddSingleton<IUserIdProvider, SignalRUserIdProvider>();

            services.AddTransient<ISensorManager, SensorManager>();
            services.AddTransient<ISensorControlManager, SensorControlManager>();
            services.AddTransient<IHistoryRepo, HistoryRepo>();
            services.AddTransient<IIconManager, IconManager>();
            services.AddTransient<IHistoryManager, HistoryManager>();
            services.AddTransient<ISensorTypeManager, SensorTypeManager>();
            services.AddTransient<IGenericRepository<Message>, BaseRepository<Message>>();
            services.AddTransient<IInvSensorNotificationManager, InvSensorNotificationManager>();
            services.AddTransient<IDashboardManager, DashboardManager>();
            services.AddTransient<IReportElementManager, ReportElementManager>();
            services.AddTransient<IActionService, ActionService>();
            services.AddTransient<IToastManager, ToastManager>();
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
            }
            app.UseDeveloperExceptionPage();
            app.UseDefaultFiles();
            app.UseStaticFiles();
     
            app.UseAuthentication();
          
            app.UseStaticFiles();        
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<MessageHub>("/messages");
                routes.MapHub<GraphHub>("/graphs");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
