using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteDashBoard.Data.Entities
{
    /// <summary>
    /// Delivery entity - DELIVERIES table
    /// </summary>
    [Table("DELIVERIES")]
    public class Delivery
    {
        [Key]
        [Column("delivery_id")]
        public int DeliveryId { get; set; }

        [Column("driver_id")]
        public int DriverId { get; set; }

        [Required]
        [Column("risk_level")]
        [StringLength(50)]
        public string RiskLevel { get; set; } = string.Empty;

        [Required]
        [Column("status")]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;

        // Navigation properties
        [ForeignKey(nameof(DriverId))]
        public virtual Driver Driver { get; set; } = null!;

        public virtual ICollection<Models.Route> Routes { get; set; } = new List<Models.Route>();
    }
}
