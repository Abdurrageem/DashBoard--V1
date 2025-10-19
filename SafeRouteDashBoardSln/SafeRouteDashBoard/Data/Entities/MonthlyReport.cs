using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteDashBoard.Data.Entities
{
    /// <summary>
    /// Monthly report entity - MONTHLY_REPORTS table
    /// </summary>
    [Table("MONTHLY_REPORTS")]
    public class MonthlyReport
    {
        [Key]
        [Column("report_id")]
        public int ReportId { get; set; }

        [Column("company_id")]
        public int CompanyId { get; set; }

        [Required]
        [Column("safety_metrics")]
        public string SafetyMetrics { get; set; } = string.Empty;

        [Required]
        [Column("risk_analysis")]
        public string RiskAnalysis { get; set; } = string.Empty;
    }
}
