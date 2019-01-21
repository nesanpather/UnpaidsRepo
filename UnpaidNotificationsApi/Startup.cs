using DataManager;
using DataManager.Interfaces;
using DataManager.Models;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UnpaidManager;
using UnpaidManager.Interfaces;
using Utilities;
using Utilities.Interfaces;

namespace UnpaidNotificationsApi
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
            services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(s => true).SetIsOriginAllowedToAllowWildcardSubdomains().Build());
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration["UnpaidNotificationsDBConnectionString:SqlServer"]));

            services.AddDbContext<UnpaidsContext>(options => options.UseSqlServer(Configuration["UnpaidsDBConnectionString:SqlServer"]));

            services.AddHttpClient();

            services.AddScoped<ISettings, AppConfigSettings>();
            services.AddScoped<IHttpClientOperations, HttpClientManager>();
            services.AddSingleton<ICustomLogger, ConsoleLogger>();

            services.AddScoped<IUnpaidStorageOperations, UnpaidDataManager>();
            services.AddScoped<IUnpaidRequestStorageOperations, UnpaidRequestDataManager>();
            services.AddScoped<IUnpaidResponseStorageOperations, UnpaidResponseDataManager>();
            services.AddScoped<IAccessTokenStorageOperations, AccessTokenDataManager>();

            services.AddScoped<IPushNotificationClient, PushNotificationService>();
            services.AddScoped<IUnpaidNotificationApiClient, UnpaidNotificationApiService>();
            services.AddScoped<INotification, PushNotificationManager>();
            services.AddScoped<IUnpaidClient, UnpaidManager.UnpaidManager>();
            services.AddScoped<IUnpaidRequestClient, UnpaidRequestManager>();
            services.AddScoped<IUnpaidResponseClient, UnpaidResponseManager>();
            services.AddScoped<IAccessTokenClient, AccessTokenManager>();
            services.AddScoped<IUnpaidEngineHandler, UnpaidEngine>();
            services.AddScoped<IUnpaidNotificationsEngineHandler, UnpaidNotificationsEngine>();
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
            app.UseMvc();
            //app.UseAuthentication();
            app.UseHangfireServer();
            app.UseHangfireDashboard();
            //app.UseHangfireDashboard("/hangfire", new DashboardOptions
            //{
            //    Authorization = new[] { new HangfireAuthorizationFilter() },
            //});
        }
    }
}
