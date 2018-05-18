using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Business.Models.Sun
{
    public class SunResponse
    {
        public SunResponseResult results { get; set; }

        public string status { get; set; }
    }

    public class SunResponseResult
    {
        public DateTime sunrise { get; set; }

        public DateTime sunset { get; set; }

        public DateTime solar_noon { get; set; }

        public DateTime civil_twilight_begin { get; set; }

        public DateTime civil_twilight_end { get; set; }

        public DateTime nautical_twilight_begin { get; set; }

        public DateTime nautical_twilight_end { get; set; }

        public DateTime astronomical_twilight_begin { get; set; }

        public DateTime astronomical_twilight_end { get; set; }

        public int day_length { get; set; }
    }
}
