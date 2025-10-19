using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteDashBoard.Data.Entities
{
    /// <summary>
    /// User entity - USERS table
    /// </summary>
    [Table("USERS")]
    public class UserEntity
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("role")]
        [StringLength(50)]
        public string Role { get; set; } = string.Empty;

        [Column("company_id")]
        public int? CompanyId { get; set; }

        // Navigation properties
        [ForeignKey(nameof(CompanyId))]
        public virtual Company? Company { get; set; }

        public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();
        public virtual ICollection<Dispatcher> Dispatchers { get; set; } = new List<Dispatcher>();
    }
}
