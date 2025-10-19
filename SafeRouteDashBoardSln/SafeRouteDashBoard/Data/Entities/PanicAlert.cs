using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteDashBoard.Data.Entities
{
    /// <summary>
    /// Panic alert entity - PANIC_ALERTS table
    /// </summary>
    [Table("PANIC_ALERTS")]
    public class PanicAlert
    {
        [Key]
        [Column("alert_id")]
        public int AlertId { get; set; }

        [Column("driver_id")]
        public int DriverId { get; set; }

        [Required]
        [Column("alert_type")]
        [StringLength(50)]
        public string AlertType { get; set; } = string.Empty;

        [Required]
        [Column("status")]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;

        [Column("acknowledged_by_dispatcher")]
        public int? AcknowledgedByDispatcher { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("resolved_at")]
        public DateTime? ResolvedAt { get; set; }

        // Navigation properties
        [ForeignKey(nameof(DriverId))]
        public virtual Driver Driver { get; set; } = null!;

        [ForeignKey(nameof(AcknowledgedByDispatcher))]
        public virtual Dispatcher? Dispatcher { get; set; }
    }
}
