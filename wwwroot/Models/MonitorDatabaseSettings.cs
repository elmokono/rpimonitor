using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homeMonitor.Models
{
    public class MonitorDatabaseSettings : IMonitorDatabaseSettings
    {
        public string MetricsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IMonitorDatabaseSettings
    {
        string MetricsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
