using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteDashBoard.Data.Entities
{
    /// <summary>
    /// Incident entity - INCIDENTS table
    /// </summary>
    [Table("INCIDENTS")]
    public class Incident
    {
        [Key]
        [Column("incident_id")]
        public int IncidentId { get; set; }

        [Column("driver_id")]
        public int DriverId { get; set; }

        [Required]
        [Column("alert_id")]
        public string AlertId { get; set; } = string.Empty;

        [Required]
        [Column("severity")]
        [StringLength(50)]
        public string Severity { get; set; } = string.Empty;

        [Column("evidence_files")]
        public string? EvidenceFiles { get; set; }

        // Navigation properties
        [ForeignKey(nameof(DriverId))]
        public virtual Driver Driver { get; set; } = null!;

        public virtual ICollection<IncidentResponse> Responses { get; set; } = new List<IncidentResponse>();
        public virtual ICollection<CameraRecording> CameraRecordings { get; set; } = new List<CameraRecording>();
    }
}
