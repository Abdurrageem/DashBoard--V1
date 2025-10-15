namespace SafeRouteDashBoard.Models
{
    /// <summary>
    /// Analytics data for dashboard metrics and reports
    /// </summary>
    public class Analytics
    {
        public DateTime Date { get; set; }
        public double AverageDeliveryTime { get; set; } // in minutes
        public double FuelEfficiency { get; set; } // L/100km (Litres per 100 kilometres)
        public double CustomerRating { get; set; } // out of 5
        public decimal CostPerDelivery { get; set; }
        
        // Trend data for charts
        public List<TrendDataPoint> DeliveryTrends { get; set; } = new();
        public List<TrendDataPoint> EfficiencyTrends { get; set; } = new();
        
        // Revenue analysis
        public decimal TotalRevenue { get; set; }
        public int OrderCount { get; set; }
        public decimal AverageOrderValue { get; set; }
        
        // Performance metrics
        public double OnTimePercentage { get; set; }
        public int TotalDeliveries { get; set; }
        public int FailedDeliveries { get; set; }
        
        // Comparative changes
        public double DeliveryTimeChange { get; set; } // percentage
        public double FuelEfficiencyChange { get; set; }
        public double CustomerRatingChange { get; set; }
        public double CostPerDeliveryChange { get; set; }
    }

    public class TrendDataPoint
    {
        public string Label { get; set; } = string.Empty; // time label like "6:00", "8:00"
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class DriverPerformance
    {
        public string DriverId { get; set; } = string.Empty;
        public string DriverName { get; set; } = string.Empty;
        public int TotalDeliveries { get; set; }
        public double OnTimePercentage { get; set; }
        public double CustomerRating { get; set; }
        public double AverageDeliveryTime { get; set; }
        public int RiskScore { get; set; }
    }

    public class RevenueAnalysis
    {
        public DateTime Period { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal PreviousPeriodRevenue { get; set; }
        public double GrowthPercentage { get; set; }
        public List<DailyRevenue> DailyBreakdown { get; set; } = new();
    }

    public class DailyRevenue
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
    }
}
