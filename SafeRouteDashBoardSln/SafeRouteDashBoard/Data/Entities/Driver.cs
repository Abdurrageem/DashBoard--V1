using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteDashBoard.Data.Entities
{
    /// <summary>
    /// Driver entity - DRIVERS table
    /// </summary>
    [Table("DRIVERS")]
    public class Driver
    {
        [Key]
        [Column("driver_id")]
        public int DriverId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("current_status")]
        [StringLength(50)]
        public string CurrentStatus { get; set; } = string.Empty;

        // Navigation properties
        [ForeignKey(nameof(UserId))]
        public virtual UserEntity User { get; set; } = null!;

        public virtual ICollection<LocationUpdate> LocationUpdates { get; set; } = new List<LocationUpdate>();
        public virtual ICollection<PanicAlert> PanicAlerts { get; set; } = new List<PanicAlert>();
        public virtual ICollection<ZoneEntry> ZoneEntries { get; set; } = new List<ZoneEntry>();
        public virtual ICollection<Incident> Incidents { get; set; } = new List<Incident>();
        public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
        public virtual ICollection<DeviceStatus> DeviceStatuses { get; set; } = new List<DeviceStatus>();
        public virtual ICollection<SafetyScore> SafetyScores { get; set; } = new List<SafetyScore>();
        public virtual ICollection<EmergencyContact> EmergencyContacts { get; set; } = new List<EmergencyContact>();
    }
}
