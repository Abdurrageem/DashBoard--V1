namespace SafeRouteDashBoard.Models
{
    /// <summary>
    /// Represents a delivery route with stops and metrics
    /// </summary>
    public class Route
    {
        public string Id { get; set; } = string.Empty;
        public string DriverId { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public RouteStatus Status { get; set; }
        
        // Route details
        public Location? StartLocation { get; set; }
        public Location? EndLocation { get; set; }
        public List<RouteStop> Stops { get; set; } = new();
        public int CurrentStopIndex { get; set; }
        
        // Distance and time metrics
        public double EstimatedDistance { get; set; } // in kilometres
        public double ActualDistance { get; set; }
        public TimeSpan EstimatedTime { get; set; }
        public TimeSpan ActualTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        
        // Optimization and conditions
        public int OptimizationScore { get; set; }
        public TrafficCondition TrafficConditions { get; set; }
        
        // Additional details
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }

    public class RouteStop
    {
        public string Id { get; set; } = string.Empty;
        public int Sequence { get; set; }
        public Location? Location { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public StopStatus Status { get; set; }
        public DateTime? EstimatedArrival { get; set; }
        public DateTime? ActualArrival { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string PackageId { get; set; } = string.Empty;
        public string DeliveryNotes { get; set; } = string.Empty;
        public string SignatureUrl { get; set; } = string.Empty;
    }

    public enum RouteStatus
    {
        Pending,
        InProgress,
        Completed,
        Cancelled,
        OnHold
    }

    public enum StopStatus
    {
        Pending,
        InProgress,
        Completed,
        Failed,
        Skipped
    }

    public enum TrafficCondition
    {
        Clear,
        Light,
        Moderate,
        Heavy,
        Severe
    }
}
