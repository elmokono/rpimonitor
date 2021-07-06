using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace homeMonitor.Models
{

    public class Metric
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; }
        [BsonElement("timestamp")]
        public string TimeStamp { get; set; }
        [BsonElement("type")]
        public string MetricType { get; set; }
        [BsonElement("value")]
        public double MetricValue { get; set; }
        [BsonElement("zoneValue")]
        public double MetricZoneValue { get; set; }
        [BsonElement("tempValue")]
        public double TempValue { get; set; }
        [BsonElement("tempZoneValue")]
        public double TempZoneValue { get; set; }
        [BsonElement("humiValue")]
        public double HumiValue { get; set; }
        [BsonElement("humiZoneValue")]
        public double HumiZoneValue { get; set; }
    }
}

