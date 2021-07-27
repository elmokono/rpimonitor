using System;
using System.ComponentModel.DataAnnotations;

namespace homeMonitor.Models
{

    public class Metric
    {
        [Key]
        public int rowIndex { get; set; }
        public DateTime timeStamp { get; set; }
        public double tempValue { get; set; }
        public double tempZoneValue { get; set; }
        public double humiValue { get; set; }
        public double humiZoneValue { get; set; }
    }
}

