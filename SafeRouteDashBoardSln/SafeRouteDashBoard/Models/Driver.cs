namespace SafeRouteDashBoard.Models
{
    /// <summary>
    /// Represents a driver in the fleet management system
    /// </summary>
    public class Driver
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public DriverStatus Status { get; set; }
        public OnlineStatus OnlineStatus { get; set; }
        
        // Delivery metrics
        public int DeliveryCount { get; set; }
        public double Distance { get; set; } // in kilometres
        public double OnDutyHours { get; set; }
        public int BreakMinutes { get; set; }
        
        // Route information
        public string RouteId { get; set; } = string.Empty;
        public int RiskScore { get; set; }
        public int Progress { get; set; } // percentage 0-100
        public string ETA { get; set; } = string.Empty;
        public Location? CurrentLocation { get; set; }
        public string Destination { get; set; } = string.Empty;
        
        // Performance metrics
        public double Rating { get; set; }
        public int TotalDeliveries { get; set; }
        public double OnTimePercentage { get; set; }
        
        // Vehicle and employment info
        public string VehicleId { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public string EmergencyContact { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }

    public enum DriverStatus
    {
        Active,
        OnBreak,
        Offline,
        Emergency
    }

    public enum OnlineStatus
    {
        Online,
        Offline,
        Away
    }

    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
    }
}
