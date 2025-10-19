using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteDashBoard.Data.Entities
{
    /// <summary>
    /// Incident response entity - INCIDENT_RESPONSES table
    /// </summary>
    [Table("INCIDENT_RESPONSES")]
    public class IncidentResponse
    {
        [Key]
        [Column("incident_id")]
        public int IncidentId { get; set; }

        [Column("dispatcher_id")]
        public int DispatcherId { get; set; }

        [Required]
        [Column("response_type")]
        [StringLength(100)]
        public string ResponseType { get; set; } = string.Empty;

        // Navigation properties
        [ForeignKey(nameof(IncidentId))]
        public virtual Incident Incident { get; set; } = null!;

        [ForeignKey(nameof(DispatcherId))]
        public virtual Dispatcher Dispatcher { get; set; } = null!;
    }
}
