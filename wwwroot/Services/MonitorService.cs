using System;
using homeMonitor.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;

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

        private Metric GetLastMetric() =>
            _metrics.Find(metric => true)
                .Sort(Builders<Metric>.Sort.Descending("timestamp"))
                .Limit(10)
                .First();

        public double GetCurrentTemperature() => GetLastMetric().TempValue;

        public double GetCurrentHumidity() => GetLastMetric().HumiValue;

        public Metric GetCurrentMetric() => GetLastMetric();

        public Metric Get(string id) => _metrics.Find<Metric>(metric => metric.Id == id).FirstOrDefault();
    }
}