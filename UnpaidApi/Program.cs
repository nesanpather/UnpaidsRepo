using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;

namespace UnpaidApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).
                ConfigureLogging((hostingContext, logging) =>
                {                    
                    logging.AddConsole();
                    logging.AddEventLog(new EventLogSettings
                    {
                        LogName = "UnpaidApi",
                        SourceName = "UnpaidApi"
                    });
                })
                .UseStartup<Startup>();
    }
}
