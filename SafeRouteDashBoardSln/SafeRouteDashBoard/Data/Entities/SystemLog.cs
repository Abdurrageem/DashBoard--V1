using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteDashBoard.Data.Entities
{
    /// <summary>
    /// System log entity - SYSTEM_LOGS table
    /// </summary>
    [Table("SYSTEM_LOGS")]
    public class SystemLog
    {
        [Key]
        [Column("log_id")]
        public int LogId { get; set; }

        [Required]
        [Column("log_type")]
        [StringLength(100)]
        public string LogType { get; set; } = string.Empty;

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
