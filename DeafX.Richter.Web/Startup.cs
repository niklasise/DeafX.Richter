using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Business.Models;
using DeafX.Richter.Business.Services;
using DeafX.Richter.Common.DataStorage;
using DeafX.Richter.Common.Logging;
using DeafX.Richter.Web.Hubs;
using DeafX.Richter.Web.Models;
using DeafX.Richter.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace DeafX.Richter.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IHostingEnvironment Environment { get; }

        public ILoggerFactory LoggerFactory { get; set; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            Environment = environment;
            LoggerFactory = loggerFactory;

            LoggerFactoryWrapper.LoggerFactory = loggerFactory;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var appConfiguration = Configuration.Get<AppConfiguration>();

            var httpClient = new HttpClient();
            var zWayService = new ZWayService(httpClient, LoggerFactory.CreateLogger<ZWayService>());
            var virtualService = new DeviceGroupService(zWayService);
            var aggregatedService = new AggregatedDeviceService(zWayService, virtualService);
            var statisticsService = new StatisticsService(LoggerFactory.CreateLogger<StatisticsService>(), aggregatedService, appConfiguration.Statistics);

            zWayService.InitAsync(appConfiguration.ZWay).Wait();
            virtualService.Init(appConfiguration.DeviceGroups);
            aggregatedService.Init(appConfiguration.ToggleAutomationRules);
            statisticsService.Init();

            services.AddMvc();
            services.AddSingleton<HttpClient>(new HttpClient());
            services.AddSingleton<IDeviceService>(aggregatedService);
            services.AddSingleton<VersionService>(new VersionService());
            services.AddSingleton<StatisticsService>(statisticsService);
            //services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                    HotModuleReplacement = true,
                    ReactHotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<DevicesHub>("devices");
            //});

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
