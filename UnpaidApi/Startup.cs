using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataManager;
using DataManager.Interfaces;
using DataManager.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UnpaidManager;
using UnpaidManager.Interfaces;
using Utilities;
using Utilities.Interfaces;
using ILogger = Utilities.Interfaces.ILogger;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<UnpaidsContext>(options => options.UseSqlServer(Configuration["ConnectionString:Local"]));

            services.AddHttpClient();

            services.AddScoped<ISettings, AppConfigSettings>();
            services.AddScoped<IHttpClientOperations, HttpClientManager>();
            services.AddSingleton<ILogger, ConsoleLogger>();

            services.AddScoped<IUnpaidStorageOperations, UnpaidDataManager>();
            services.AddScoped<IUnpaidRequestStorageOperations, UnpaidRequestDataManager>();
            services.AddScoped<IUnpaidResponseStorageOperations, UnpaidResponseDataManager>();

            services.AddScoped<IPushNotificationClient, PushNotificationService>();
            services.AddScoped<INotification, PushNotificationManager>();
            services.AddScoped<IUnpaidClient, UnpaidManager.UnpaidManager>();
            services.AddScoped<IUnpaidRequestClient, UnpaidRequestManager>();
            services.AddScoped<IUnpaidEngineHandler, UnpaidEngine>();
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
            app.UseMvc();
        }
    }
}
