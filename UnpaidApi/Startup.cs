﻿using DataManager;
using DataManager.Interfaces;
using DataManager.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UnpaidApi.Controllers;
using UnpaidManager;
using UnpaidManager.Interfaces;
using Utilities;
using Utilities.Interfaces;

namespace UnpaidApi
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
            // ********************
            // Setup CORS
            // ********************

            var origins = Configuration["Cors:AllowedOrigins"].Split(";");

            if (origins.Length >= 0)
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("SiteCorsPolicy", builder => builder.WithOrigins(origins).AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed(s => true).Build());
                });
            }
            else
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("SiteCorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed(s => true).SetIsOriginAllowedToAllowWildcardSubdomains().Build());
                });
            }

            services.AddAuthentication(IISDefaults.AuthenticationScheme);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<UnpaidsContext>(options => options.UseSqlServer(Configuration["UnpaidsDBConnectionString:SqlServer"]));

            services.AddHttpClient();

            services.AddScoped<ISettings, AppConfigSettings>();
            services.AddScoped<IHttpClientOperations, HttpClientManager>();

            services.AddScoped<IUnpaidStorageOperations, UnpaidDataManager>();
            services.AddScoped<IUnpaidRequestStorageOperations, UnpaidRequestDataManager>();
            services.AddScoped<IUnpaidResponseStorageOperations, UnpaidResponseDataManager>();
            services.AddScoped<IAccessTokenStorageOperations, AccessTokenDataManager>();
            services.AddScoped<IUnpaidBatchStorageOperations, UnpaidBatchDataManager>();

            services.AddScoped<IPushNotificationClient, PushNotificationService>();
            services.AddScoped<IUnpaidNotificationApiClient, UnpaidNotificationApiService>();
            services.AddScoped<INotification, PushNotificationManager>();
            services.AddScoped<IUnpaidClient, UnpaidManager.UnpaidManager>();
            services.AddScoped<IUnpaidRequestClient, UnpaidRequestManager>();
            services.AddScoped<IUnpaidResponseClient, UnpaidResponseManager>();
            services.AddScoped<IAccessTokenClient, AccessTokenManager>();
            services.AddScoped<IUnpaidBatchClient, UnpaidBatchManager>();
            services.AddScoped<IUnpaidEngineHandler, UnpaidEngine>();
            services.AddScoped<IUnpaidNotificationsEngineHandler, UnpaidNotificationsEngine>();
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();                                    
            app.UseCors("SiteCorsPolicy");
            //app.UseAuthentication();
            app.UseMvc(); 
        }
    }
}
