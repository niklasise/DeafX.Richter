using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DeafX.Richter.Common.DataStorage;
using DeafX.Richter.Web.Models;

namespace DeafX.Richter.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var webHost = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseUrls("http://*:80")
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config.SetBasePath(env.ContentRootPath);
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    var loggingConfig = hostingContext.Configuration.Get<AppConfiguration>().Logging;
                    //logging.AddDatabase(new LiteDbDataStorage(@"C:\Temp\Richter\storage.db"), LogLevel.Debug);
                    logging.AddFile($"{loggingConfig.Directory.TrimEnd('\\')}\\log-{{Date}}.txt", 
                        minimumLevel: loggingConfig.LogLevel,
                        levelOverrides: new Dictionary<string, LogLevel> {
                            { "System", LogLevel.Error },
                            { "Microsoft", LogLevel.Error },
                        }
                    );

                    logging.AddFilter("System", LogLevel.Error);
                    logging.AddFilter("Microsoft", LogLevel.Error);
                })
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            webHost.Run();
        }
    }
}
