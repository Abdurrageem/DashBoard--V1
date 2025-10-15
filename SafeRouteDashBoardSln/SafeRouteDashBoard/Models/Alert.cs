namespace SafeRouteDashBoard.Models
{
    /// <summary>
    /// Represents a safety or operational alert in the system
    /// </summary>
    public class Alert
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public AlertSeverity Severity { get; set; }
        public AlertCategory Category { get; set; }
        
        // Related entities
        public string DriverId { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string RouteId { get; set; } = string.Empty;
        public Location? Location { get; set; }
        
        // Timestamps and status
        public DateTime Timestamp { get; set; }
        public DateTime? ResolvedTimestamp { get; set; }
        public AlertStatus Status { get; set; }
        
        // Assignment and notes
        public string AssignedTo { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public List<string> RelatedAlerts { get; set; } = new();
        
        // Helper methods
        public string GetRelativeTime()
        {
            var timeSpan = DateTime.Now - Timestamp;
            
            if (timeSpan.TotalMinutes < 1)
                return "Just now";
            if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes}m ago";
            if (timeSpan.TotalHours < 24)
                return $"{(int)timeSpan.TotalHours}h ago";
            
            return $"{(int)timeSpan.TotalDays}d ago";
        }
    }

    public enum AlertSeverity
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum AlertCategory
    {
        Safety,
        Vehicle,
        Theft,
        Traffic,
        Weather,
        System,
        Other
    }

    public enum AlertStatus
    {
        Active,
        Resolved,
        Dismissed,
        InProgress
    }
}
