using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteDashBoard.Data.Entities
{
    /// <summary>
    /// Threat detection entity - THREAT_DETECTIONS table
    /// </summary>
    [Table("THREAT_DETECTIONS")]
    public class ThreatDetection
    {
        [Key]
        [Column("detection_id")]
        public int DetectionId { get; set; }

        [Required]
        [Column("threat_type")]
        [StringLength(100)]
        public string ThreatType { get; set; } = string.Empty;

        [Column("confidence_score")]
        public decimal ConfidenceScore { get; set; }

        // Navigation properties
        public virtual ICollection<CameraRecording> CameraRecordings { get; set; } = new List<CameraRecording>();
    }
}
