using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteDashBoard.Data.Entities
{
    /// <summary>
    /// Company entity - COMPANIES table
    /// </summary>
    [Table("COMPANIES")]
    public class Company
    {
        [Key]
        [Column("company_id")]
        public int CompanyId { get; set; }

        [Required]
        [Column("registration_number")]
        [StringLength(100)]
        public string RegistrationNumber { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
    }
}
