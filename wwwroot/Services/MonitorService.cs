using homeMonitor.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace homeMonitor.Services
{
    public class MonitorService
    {
        private readonly IMongoCollection<Metric> _metrics;

        public MonitorService(IMonitorDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _metrics = database.GetCollection<Metric>(settings.MetricsCollectionName);
        }

        public List<Metric> GetHumidityList() => _metrics.Find(metric => metric.Type == Metric.HumidityType).ToList();

        public List<Metric> GetTemperatureList() => _metrics.Find(metric => metric.Type == Metric.TemperatureType).ToList();

        public double GetCurrentTemperature() => GetTemperatureList().OrderByDescending(x => x.TimeStamp).First().Value;

        public Metric Get(string id) => _metrics.Find<Metric>(metric => metric.Id == id).FirstOrDefault();
    }
}