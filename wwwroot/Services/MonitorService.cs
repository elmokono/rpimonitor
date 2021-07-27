using System;
using homeMonitor.Models;
using System.Collections.Generic;
using System.Linq;

namespace homeMonitor.Services
{
    public class MonitorService
    {
        private readonly MonitorDbContext _context;

        public MonitorService(MonitorDbContext context)
        {
            _context = context;
        }

        private Metric GetLastMetric() => _context.metrics.OrderByDescending(x => x.timeStamp).First();

        public double GetCurrentTemperature() => GetLastMetric().tempValue;

        public double GetCurrentHumidity() => GetLastMetric().humiValue;

        public Metric GetCurrentMetric() => GetLastMetric();

        public Metric Get(int id) => _context.metrics.FirstOrDefault(metric => metric.rowIndex == id);
    }
}