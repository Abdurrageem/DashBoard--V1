using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteDashBoard.Data.Entities
{
    /// <summary>
    /// Notification entity - NOTIFICATIONS table
    /// </summary>
    [Table("NOTIFICATIONS")]
    public class NotificationEntity
    {
        [Key]
        [Column("notification_id")]
        public int NotificationId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("message")]
        public string Message { get; set; } = string.Empty;

        [Required]
        [Column("priority")]
        [StringLength(50)]
        public string Priority { get; set; } = string.Empty;
    }
}
