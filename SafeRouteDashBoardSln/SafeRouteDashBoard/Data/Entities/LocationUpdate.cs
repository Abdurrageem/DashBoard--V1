using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteDashBoard.Data.Entities
{
    /// <summary>
    /// Location update entity - LOCATION_UPDATES table
    /// </summary>
    [Table("LOCATION_UPDATES")]
    public class LocationUpdate
    {
        [Key]
        [Column("location_id")]
        public int LocationId { get; set; }

        [Column("driver_id")]
        public int DriverId { get; set; }

        [Column("lat")]
        public decimal Lat { get; set; }

        [Column("lng")]
        public decimal Lng { get; set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        // Navigation properties
        [ForeignKey(nameof(DriverId))]
        public virtual Driver Driver { get; set; } = null!;
    }
}
