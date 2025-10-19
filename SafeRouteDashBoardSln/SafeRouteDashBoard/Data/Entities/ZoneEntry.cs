using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteDashBoard.Data.Entities
{
    /// <summary>
    /// Zone entry entity - ZONE_ENTRIES table
    /// </summary>
    [Table("ZONE_ENTRIES")]
    public class ZoneEntry
    {
        [Key]
        [Column("driver_id")]
        public int DriverId { get; set; }

        [Key]
        [Column("geofence_id")]
        public int GeofenceId { get; set; }

        [Column("alert_triggered")]
        public bool AlertTriggered { get; set; }

        // Navigation properties
        [ForeignKey(nameof(DriverId))]
        public virtual Driver Driver { get; set; } = null!;

        [ForeignKey(nameof(GeofenceId))]
        public virtual Geofence Geofence { get; set; } = null!;
    }
}
