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
    public class DevicesController : Controller
    {
        private readonly MonitorService _monitorService;

        public DevicesController(MonitorService monitorService)
        {
            _monitorService = monitorService;
        }
        
        [Route("setColor")]
        [HttpPost]
        public ActionResult<string> SetColor(BluetoothRgbLightCommand command)
        {
            return _monitorService.SetRgbColor(command.DeviceId, command.Color);
        }
    }
}
