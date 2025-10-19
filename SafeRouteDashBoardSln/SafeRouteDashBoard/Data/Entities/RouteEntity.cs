using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteDashBoard.Data.Entities
{
    /// <summary>
    /// Route entity - ROUTES table
    /// </summary>
    [Table("ROUTES")]
    public class RouteEntity
    {
        [Key]
        [Column("route_id")]
        public int RouteId { get; set; }

        [Required]
        [Column("planned_waypoints")]
        public string PlannedWaypoints { get; set; } = string.Empty;

        [Column("actual_path")]
        public string? ActualPath { get; set; }

        // No navigation properties defined in schema
    }
}
