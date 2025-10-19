using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteDashBoard.Data.Entities
{
    /// <summary>
    /// Dispatcher entity - DISPATCHERS table
    /// </summary>
    [Table("DISPATCHERS")]
    public class Dispatcher
    {
        [Key]
        [Column("dispatcher_id")]
        public int DispatcherId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("assigned_drivers")]
        [StringLength(500)]
        public string? AssignedDrivers { get; set; }

        // Navigation properties
        [ForeignKey(nameof(UserId))]
        public virtual UserEntity User { get; set; } = null!;

        public virtual ICollection<IncidentResponse> IncidentResponses { get; set; } = new List<IncidentResponse>();
        public virtual ICollection<PanicAlert> AcknowledgedAlerts { get; set; } = new List<PanicAlert>();
    }
}
