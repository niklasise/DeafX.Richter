using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Business.Models;
using DeafX.Richter.Business.Services;
using DeafX.Richter.Common.DataStorage;
using DeafX.Richter.Web.Hubs;
using DeafX.Richter.Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace DeafX.Richter.Web
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }
        public ILoggerFactory LoggerFactory { get; set; }

        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);

            Configuration = builder.Build();
            LoggerFactory = loggerFactory;
        }

        

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var httpClient = new HttpClient();
            var zWayService = new ZWayService(httpClient, LoggerFactory.CreateLogger<ZWayService>());
            //var virtualService = new VirtualDeviceService();
            var aggregatedService = new AggregatedDeviceService(zWayService);

            zWayService.InitAsync(Configuration.Get<AppConfiguration>().ZWay).Wait();
            aggregatedService.Init(Configuration.Get<AppConfiguration>().ToggleAutomationRules);

            services.AddMvc();
            services.AddSingleton<HttpClient>(new HttpClient());
            services.AddSingleton<IDeviceService>(aggregatedService);
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddDatabase(new LiteDbDataStorage(@"C:\Temp\Richter\storage.db"), LogLevel.Warning);
            loggerFactory.AddFile(@"C:\Temp\Richter\Logs\log-{Date}.txt", minimumLevel: LogLevel.Warning);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                    HotModuleReplacement = true,
                    ReactHotModuleReplacement = true,
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSignalR(routes =>
            {
                routes.MapHub<DevicesHub>("devices");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });


        }
    }
}
