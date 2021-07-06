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
        public ActionResult<List<Metric>> Get() => _monitorService.GetTemperatureList();

        [Route("currentTemp")]
        public ActionResult<double> GetCurrentTemperature() => _monitorService.GetCurrentTemperature();
        
        [HttpGet("{id:length(24)}", Name = "GetMetric")]
        public ActionResult<Metric> Get(string id)
        {
            var metric = _monitorService.Get(id);

            if (metric == null)
            {
                return NotFound();
            }

            return metric;
        }
    }
}
