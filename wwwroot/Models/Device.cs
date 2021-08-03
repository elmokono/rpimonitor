using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace homeMonitor.Models
{
    public class Device
    {
        [Key]
        public int RowIndex { get; set; }
        public string Name { get; set; }
    }

    public class BluetoothRgbLight : Device
    {
        public string Address { get; set; }
        public string Handle { get; set; }
        public string RgbColorCommand { get; set; }
    }

    public class BluetoothRgbLightCommand
    {
        public int DeviceId { get; set; }
        public string Color { get; set; }
    }
    
}
