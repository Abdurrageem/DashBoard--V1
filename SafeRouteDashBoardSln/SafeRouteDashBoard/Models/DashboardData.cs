namespace SafeRouteDashBoard.Models
{
    /// <summary>
    /// Dashboard data containing all metrics and information for the main page
    /// </summary>
    public class DashboardData
    {
        // KPI metrics
        public KpiMetrics KpiMetrics { get; set; } = new();
        
        // Driver information
        public List<Driver> ActiveDrivers { get; set; } = new();
        public DriverStatistics DriverStats { get; set; } = new();
        
        // Alert information
        public List<Alert> ActiveAlerts { get; set; } = new();
        
        // System status
        public SystemStatus SystemStatus { get; set; } = new();
        
        // Last update timestamp
        public DateTime LastUpdated { get; set; }
    }

    public class KpiMetrics
    {
        public int ActiveDriversCount { get; set; }
        public int TotalDrivers { get; set; }
        public int DeliveriesToday { get; set; }
        public double OnTimePercentage { get; set; }
        public int ActiveAlertsCount { get; set; }
        public int FleetSafetyScore { get; set; }
        
        // Trends for sparklines
        public List<double> ActiveDriversTrend { get; set; } = new();
        public List<double> DeliveriesTrend { get; set; } = new();
        public List<double> SafetyScoreTrend { get; set; } = new();
    }

    public class DriverStatistics
    {
        public int ActiveCount { get; set; }
        public int OnBreakCount { get; set; }
        public int OfflineCount { get; set; }
        public int EmergencyCount { get; set; }
        public int TotalCount { get; set; }
    }

    public class SystemStatus
    {
        public bool IsOnline { get; set; }
        public DateTime LastUpdate { get; set; }
        public string Version { get; set; } = string.Empty;
        public List<ServiceStatus> Services { get; set; } = new();
    }

    public class ServiceStatus
    {
        public string Name { get; set; } = string.Empty;
        public bool IsHealthy { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime LastCheck { get; set; }
    }
}
