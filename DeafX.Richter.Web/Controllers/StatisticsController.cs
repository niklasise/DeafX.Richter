using DeafX.Richter.Business.Services;
using DeafX.Richter.Common.Extensions;
using DeafX.Richter.Web.Models.Statistics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeafX.Richter.Web.Controllers
{
    [Route("api/[controller]")]
    public class StatisticsController : Controller
    {
        private ILogger<StatisticsController> _logger;
        private StatisticsService _service;

        public StatisticsController(ILogger<StatisticsController> logger, StatisticsService service)
        {
            _logger = logger;
            _service = service;
        }


        //[HttpGet("{deviceId}")]
        //public StatisticsViewModel GetAllStatistics(string deviceId)
        //{
        //    return new StatisticsViewModel(_service.GetStatistics(deviceId).ToDictionary(kvp => kvp.DateTime.ToUnixTimeStamp(), kvp => kvp.Data));
        //}

        [HttpGet("{deviceId}")]
        public StatisticsViewModel GetStatistics(string deviceId, [FromQuery]long to = 0, [FromQuery]long from = 0, [FromQuery]int minimumDataInterval = 0)
        {
            var toDateTime = to > 0 ? to.ToDateTimeUnixTimeStamp() : DateTime.MaxValue;
            var fromDateTime = from > 0 ? from.ToDateTimeUnixTimeStamp() : DateTime.MinValue;

            return new StatisticsViewModel(
                _service.GetStatistics(deviceId, fromDateTime, toDateTime, TimeSpan.FromSeconds(minimumDataInterval))
                .ToDictionary(kvp => kvp.DateTime.ToUnixTimeStamp(), kvp => kvp.Data));
        }

    }
}
