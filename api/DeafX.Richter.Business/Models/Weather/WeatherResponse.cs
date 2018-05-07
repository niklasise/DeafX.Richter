using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Business.Models.Weather
{
    public class WeatherResponse
    {
        public Response Response { get; set; }
    }

    public class Response
    {
        public Result[] Result { get; set; }
    }

    public class Result
    {
        public WeatherStation[] WeatherStation { get; set; }
    }

    public class WeatherStation
    {
        public bool Active { get; set; }

        public string Name { get; set; }

        public string Id { get; set; }

        public Measurement Measurement { get; set; }
    }

    public class Measurement
    {
        public DateTime MeasureTime { get; set; }

        public AirMeasurement Air { get; set; }

        public RoadMeasurement Road { get; set; }

        public PrecipitationMeasurement Precipitation { get; set; }

        public WindMeasurement Wind { get; set; }
    }

    public class RoadMeasurement
    {
        public double Temp { get; set; }
    }

    public class AirMeasurement
    {
        public double RelativeHumidity { get; set; }

        public double Temp { get; set; }
    }

    public class PrecipitationMeasurement
    {
        public double Amount { get; set; }

        public string AmountName { get; set; }

        public string Type { get; set; }
    }

    public class WindMeasurement
    {
        public double Direction { get; set; }

        public string DirectionText { get; set; }

        public double Force { get; set; }

        public double ForceMax { get; set; }
    }
}
