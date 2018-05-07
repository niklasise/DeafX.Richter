using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Business.Models.Weather
{
    public class WeatherConfiguration
    {
        public string ApiUrl { get; set; }

        public string ApiKey { get; set; }

        public string StationId { get; set; }

        public int UpdateInterval { get; set; }
    }
}
