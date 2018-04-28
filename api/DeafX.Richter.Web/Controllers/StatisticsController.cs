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

        [HttpGet("{deviceId}")]
        public IActionResult GetStatistics(string deviceId, [FromQuery]long to = 0, [FromQuery]long from = 0, [FromQuery]int minimumDataInterval = 0)
        {
            var toDateTime = to > 0 ? to.ToDateTimeUnixTimeStamp() : DateTime.MaxValue;
            var fromDateTime = from > 0 ? from.ToDateTimeUnixTimeStamp() : DateTime.MinValue;

            var statistics = _service.GetStatistics(deviceId, fromDateTime, toDateTime, TimeSpan.FromSeconds(minimumDataInterval));

            if(statistics == null)
            {
                return NotFound();
            }

            return new JsonResult(
                statistics.Select(d => new StatisticsViewModel() { timeStamp = d.DateTime.ToUnixTimeStamp(), data = d.Data })
            );
        }

    }
}
