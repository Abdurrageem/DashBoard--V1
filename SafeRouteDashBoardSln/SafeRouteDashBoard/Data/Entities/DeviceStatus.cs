using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteDashBoard.Data.Entities
{
    /// <summary>
    /// Device status entity - DEVICE_STATUS table
    /// </summary>
    [Table("DEVICE_STATUS")]
    public class DeviceStatus
    {
        [Key]
        [Column("status_id")]
        public int StatusId { get; set; }

        [Column("driver_id")]
        public int DriverId { get; set; }

        [Column("battery_level")]
        public int? BatteryLevel { get; set; }

        [Column("gps_enabled")]
        public bool GpsEnabled { get; set; }

        // Navigation properties
        [ForeignKey(nameof(DriverId))]
        public virtual Driver Driver { get; set; } = null!;
    }
}
