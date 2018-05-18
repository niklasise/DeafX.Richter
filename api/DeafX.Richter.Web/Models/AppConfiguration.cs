using DeafX.Richter.Business.Models;
using DeafX.Richter.Business.Models.Sun;
using DeafX.Richter.Business.Models.Weather;
using DeafX.Richter.Business.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeafX.Richter.Web.Models
{
    public class AppConfiguration
    {
        public ZWayConfiguration ZWay { get; set; }

        public ToggleAutomationRuleConfiguration[] ToggleAutomationRules { get; set; }

        public DeviceGroupConfiguration[] DeviceGroups { get; set; }

        public LoggingConfiguration Logging { get; set; }

        public StatisticsServiceConfiguration Statistics { get; set; }

        public WeatherConfiguration Weather { get; set; }

        public SunConfiguration Sun { get; set; }

        public string[] DevicesToShow { get; set; }
    }

    public class LoggingConfiguration
    {
        public string Directory { get; set; }

        public LogLevel LogLevel { get; set; }
    }
}
