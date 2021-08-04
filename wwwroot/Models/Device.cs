using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace homeMonitor.Models
{
    public class Device
    {
        [Key]
        [Column("rowIndex")]
        public int RowIndex { get; set; }
        [Column("name")]
        public string Name { get; set; }
    }

    public class BluetoothRgbLight : Device
    {
        [Column("address")]
        public string Address { get; set; }
        [Column("handle")]
        public string Handle { get; set; }
        [Column("command")] 
        public string Command { get; set; }
    }
}
