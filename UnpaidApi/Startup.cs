using DataManager;
using DataManager.Interfaces;
using DataManager.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            // ********************
            // Setup CORS
            // ********************

            services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy", builder => builder.WithOrigins("http://localhost:50565").AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed(s => true).Build());
            });

            services.AddAuthentication(IISDefaults.AuthenticationScheme);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<UnpaidsContext>(options => options.UseSqlServer(Configuration["UnpaidsDBConnectionString:SqlServer"]));

            services.AddHttpClient();

            services.AddScoped<ISettings, AppConfigSettings>();
            services.AddScoped<IHttpClientOperations, HttpClientManager>();
            services.AddSingleton<ILogger, ConsoleLogger>();

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

            //services.AddAuthorization(options => {
            //    options.AddPolicy("AllUsers", policy => {
            //        policy.AddAuthenticationSchemes(IISDefaults.AuthenticationScheme);
            //        policy.RequireRole("S - 1 - 1 - 0");
            //    });
            //});


            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowSpecificOrigin",
            //        //builder => builder.WithOrigins("https://localhost:44348").AllowAnyHeader());
            //        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            //});
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
