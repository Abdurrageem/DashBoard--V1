using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteDashBoard.Data.Entities
{
    /// <summary>
    /// Risk zone entity - RISK_ZONES table
    /// </summary>
    [Table("RISK_ZONES")]
    public class RiskZone
    {
        [Key]
        [Column("zone_id")]
        public int ZoneId { get; set; }

        [Required]
        [Column("risk_level")]
        [StringLength(50)]
        public string RiskLevel { get; set; } = string.Empty;

        [Required]
        [Column("boundary_coordinates")]
        public string BoundaryCoordinates { get; set; } = string.Empty;

        [Column("incident_count")]
        public int? IncidentCount { get; set; }
    }
}
