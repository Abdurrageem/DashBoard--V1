using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteDashBoard.Data.Entities
{
    /// <summary>
    /// Camera recording entity - CAMERA_RECORDINGS table
    /// </summary>
    [Table("CAMERA_RECORDINGS")]
    public class CameraRecording
    {
        [Key]
        [Column("recording_id")]
        public int RecordingId { get; set; }

        [Column("incident_id")]
        public int IncidentId { get; set; }

        [Column("detection_id")]
        public int? DetectionId { get; set; }

        [Required]
        [Column("trigger_type")]
        [StringLength(100)]
        public string TriggerType { get; set; } = string.Empty;

        [Required]
        [Column("file_path")]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;

        // Navigation properties
        [ForeignKey(nameof(IncidentId))]
        public virtual Incident Incident { get; set; } = null!;

        [ForeignKey(nameof(DetectionId))]
        public virtual ThreatDetection? ThreatDetection { get; set; }
    }
}
