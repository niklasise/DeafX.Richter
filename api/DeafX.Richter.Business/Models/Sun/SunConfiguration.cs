using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Business.Models.Sun
{
    public class SunConfiguration
    {
        public int SunSetOffset { get; set; }

        public int SunRiseOffset { get; set; }

        public string ApiUrl { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }
    }
}
