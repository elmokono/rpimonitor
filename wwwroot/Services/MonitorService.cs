using System;
using homeMonitor.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace homeMonitor.Services
{
    public class MonitorService
    {
        private readonly MonitorDbContext _context;
        private readonly IOsCommands _osCommands;

        public MonitorService(MonitorDbContext context, IOsCommands osCommands)
        {
            _context = context;
            _osCommands = osCommands;
        }

        private Metric GetLastMetric() => _context.metrics.OrderByDescending(x => x.timeStamp).First();

        public double GetCurrentTemperature() => GetLastMetric().tempValue;

        public double GetCurrentHumidity() => GetLastMetric().humiValue;

        public Metric GetCurrentMetric() => GetLastMetric();

        public Metric Get(int id) => _context.metrics.FirstOrDefault(metric => metric.rowIndex == id);

        public IEnumerable<BluetoothRgbLight> GetBluetoothRgbLights()
        {
            return new[]
            {
                new BluetoothRgbLight {
                    Address = "FF:FF:BA:00:EA:30",
                    Handle = "0x0007",
                    Name = "Luz Living",
                    RgbColorCommand = "gatttool -b {{ADDRESS}} --char-write-req -a {{HANDLE}} -n 56{{RR}}{{GG}}{{BB}}{{WW}}f0aa",
                    RowIndex = 0
                }
            };
        }

        public ActionResult<string> SetRgbColor(int deviceId, string color)
        {
            var device = GetBluetoothRgbLights().First(x => x.RowIndex == deviceId);

            var cmd = device.RgbColorCommand
                .Replace("{{ADDRESS}}", device.Address)
                .Replace("{{HANDLE}}", device.Handle)
                .Replace("{{RR}}{{GG}}{{BB}}{{WW}}", color);

            return _osCommands.Execute(cmd);
        }
    }
}