using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteDashBoard.Data.Entities
{
    /// <summary>
    /// Safety score entity - SAFETY_SCORES table
    /// </summary>
    [Table("SAFETY_SCORES")]
    public class SafetyScore
    {
        [Key]
        [Column("score_id")]
        public int ScoreId { get; set; }

        [Column("driver_id")]
        public int DriverId { get; set; }

        [Column("overall_score")]
        public decimal OverallScore { get; set; }

        [Column("recommendations")]
        public string? Recommendations { get; set; }

        // Navigation properties
        [ForeignKey(nameof(DriverId))]
        public virtual Driver Driver { get; set; } = null!;
    }
}
