using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace homeMonitor.Models
{

    public class Metric
    {
        public const string HumidityType = "humi";
        public const string TemperatureType = "temp";

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; }
        [BsonElement("timestamp")]
        public string TimeStamp { get; set; }
        [BsonElement("type")]
        public string Type { get; set; }
        [BsonElement("value")]
        public double Value { get; set; }
        [BsonElement("zoneValue")]
        public double ZoneValue { get; set; }
    }
}

