using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteDashBoard.Data.Entities
{
    /// <summary>
    /// Geofence entity - GEOFENCES table  
    /// </summary>
    [Table("GEOFENCES")]
    public class Geofence
    {
        [Key]
        [Column("geofence_id")]
        public int GeofenceId { get; set; }

        [Required]
        [Column("zone_type")]
        [StringLength(50)]
        public string ZoneType { get; set; } = string.Empty;

        [Required]
        [Column("polygon_coordinates")]
        public string PolygonCoordinates { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<ZoneEntry> ZoneEntries { get; set; } = new List<ZoneEntry>();
    }
}
