using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteDashBoard.Data.Entities
{
    /// <summary>
    /// Emergency contact entity - EMERGENCY_CONTACTS table
    /// </summary>
    [Table("EMERGENCY_CONTACTS")]
    public class EmergencyContact
    {
        [Key]
        [Column("contact_id")]
        public int ContactId { get; set; }

        [Column("driver_id")]
        public int DriverId { get; set; }

        [Required]
        [Column("relationship")]
        [StringLength(100)]
        public string Relationship { get; set; } = string.Empty;

        // Navigation properties
        [ForeignKey(nameof(DriverId))]
        public virtual Driver Driver { get; set; } = null!;
    }
}
