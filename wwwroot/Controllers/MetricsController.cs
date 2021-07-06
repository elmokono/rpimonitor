using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using homeMonitor.Models;
using homeMonitor.Services;

namespace homeMonitor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetricsController : Controller
    {
        private readonly MonitorService _monitorService;

        public MetricsController(MonitorService monitorService)
        {
            _monitorService = monitorService;
        }

        [HttpGet]
        public ActionResult<Metric> Get() => _monitorService.GetCurrentMetric();

        [Route("currentTemp")]
        public ActionResult<double> GetCurrentTemperature() => _monitorService.GetCurrentTemperature();

        [Route("currentHumi")]
        public ActionResult<double> GetCurrentHumidity() => _monitorService.GetCurrentHumidity();
    }
}
