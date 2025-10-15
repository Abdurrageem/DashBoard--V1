namespace SafeRouteDashBoard.Models
{
    /// <summary>
    /// Safety metrics and risk analysis data
    /// </summary>
    public class SafetyMetrics
    {
        public int FleetScore { get; set; } // 0-100
        public int ActiveAlerts { get; set; }
        public int CriticalAlertCount { get; set; }
        public double OnTimePercentage { get; set; }
        public int IncidentCount { get; set; }
        public List<IncidentType> IncidentTypes { get; set; } = new();
        
        // Risk distribution
        public RiskDistribution RiskDistribution { get; set; } = new();
        
        // Risk factors
        public RiskFactors RiskFactors { get; set; } = new();
        
        // Trends
        public double FleetScoreChange { get; set; } // daily change
        public double OnTimeChange { get; set; }
        public int IncidentTrend { get; set; } // week over week change
        
        // Predictive insights
        public List<PredictiveInsight> Insights { get; set; } = new();
    }

    public class RiskDistribution
    {
        public int LowRiskPercentage { get; set; }
        public int MediumRiskPercentage { get; set; }
        public int HighRiskPercentage { get; set; }
        public int TotalDrivers { get; set; }
    }

    public class RiskFactors
    {
        public RiskLevel TimeRisk { get; set; }
        public string TimeRiskDescription { get; set; } = string.Empty;
        public int TimeRiskPercentage { get; set; }
        
        public RiskLevel LocationRisk { get; set; }
        public string LocationRiskDescription { get; set; } = string.Empty;
        public int LocationRiskPercentage { get; set; }
        
        public RiskLevel TrafficRisk { get; set; }
        public string TrafficRiskDescription { get; set; } = string.Empty;
        public int TrafficRiskPercentage { get; set; }
        
        public RiskLevel WeatherRisk { get; set; }
        public string WeatherRiskDescription { get; set; } = string.Empty;
        public int WeatherRiskPercentage { get; set; }
    }

    public enum RiskLevel
    {
        Low,
        Medium,
        High,
        Critical
    }

    public class IncidentType
    {
        public string Type { get; set; } = string.Empty;
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

    public class PredictiveInsight
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public InsightType Type { get; set; }
        public InsightPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDismissed { get; set; }
        public DateTime? DismissedAt { get; set; }
    }

    public enum InsightType
    {
        Warning,
        Information,
        Recommendation,
        Alert
    }

    public enum InsightPriority
    {
        Low,
        Medium,
        High
    }

    public class Incident
    {
        public string Id { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string DriverId { get; set; } = string.Empty;
        public string DriverName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public AlertSeverity Severity { get; set; }
        public IncidentStatus Status { get; set; }
        public string Description { get; set; } = string.Empty;
        public Location? Location { get; set; }
        public List<string> Actions { get; set; } = new();
    }

    public enum IncidentStatus
    {
        Reported,
        UnderInvestigation,
        Resolved,
        Closed
    }
}
