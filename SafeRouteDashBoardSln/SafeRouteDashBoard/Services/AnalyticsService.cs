using SafeRouteDashBoard.Models;
using SafeRouteDashBoard.Data;
using Microsoft.EntityFrameworkCore;

namespace SafeRouteDashBoard.Services
{
    public interface IAnalyticsService
    {
        Task<Analytics> GetDailyMetricsAsync(DateTime date);
        Task<List<TrendDataPoint>> GetTrendDataAsync(DateTime startDate, DateTime endDate);
        Task<List<DriverPerformance>> GetDriverPerformanceComparisonAsync();
        Task<RevenueAnalysis> GetRevenueAnalysisAsync(string period);
        Task<byte[]> ExportReportAsync(string format, Dictionary<string, string> filters);
        Task<DashboardKpiMetrics> GetDashboardKpiMetricsAsync();
        Task<List<PanicAlertTrend>> GetPanicAlertTrendsAsync(int days = 7);
        Task<List<RiskLevelDistribution>> GetRiskDistributionAsync();
    }

    public class AnalyticsService : IAnalyticsService
    {
        private readonly IDbContextFactory<SafeRouteDbContext> _contextFactory;
        private readonly Random _random = new();

        public AnalyticsService(IDbContextFactory<SafeRouteDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Analytics> GetDailyMetricsAsync(DateTime date)
        {
            await Task.Delay(100);
            
            return new Analytics
            {
                Date = date,
                AverageDeliveryTime = 28 + _random.Next(5),
                FuelEfficiency = 8.5 + (_random.NextDouble() * 2), // L/100km (converted from MPG)
                CustomerRating = 4.5 + (_random.NextDouble() * 0.5),
                CostPerDelivery = 185.00m + (decimal)(_random.NextDouble() * 50), // ZAR (converted from USD)
                DeliveryTimeChange = -8 + (_random.NextDouble() * 4),
                FuelEfficiencyChange = 3 + (_random.NextDouble() * 4),
                CustomerRatingChange = 2 + (_random.NextDouble() * 2),
                CostPerDeliveryChange = -12 + (_random.NextDouble() * 5),
                OnTimePercentage = 92 + (_random.NextDouble() * 6),
                TotalDeliveries = 847,
                FailedDeliveries = 12,
                TotalRevenue = 156850.00m, // ZAR (converted from USD)
                OrderCount = 847,
                AverageOrderValue = 185.20m, // ZAR
                DeliveryTrends = GenerateDeliveryTrends(),
                EfficiencyTrends = GenerateEfficiencyTrends()
            };
        }

        public async Task<List<TrendDataPoint>> GetTrendDataAsync(DateTime startDate, DateTime endDate)
        {
            await Task.Delay(100);
            var trends = new List<TrendDataPoint>();
            var currentDate = startDate;

            while (currentDate <= endDate)
            {
                trends.Add(new TrendDataPoint
                {
                    Label = currentDate.ToString("MMM dd"),
                    Value = 800 + _random.Next(200),
                    Timestamp = currentDate
                });
                currentDate = currentDate.AddDays(1);
            }

            return trends;
        }

        public async Task<List<DriverPerformance>> GetDriverPerformanceComparisonAsync()
        {
            await Task.Delay(100);
            var names = new[] { "Emily Watson", "Michael Chen", "Sarah Johnson", "David Rodriguez", "Lisa Anderson" };
            
            return names.Select((name, index) => new DriverPerformance
            {
                DriverId = $"driver-{index + 1:D3}",
                DriverName = name,
                TotalDeliveries = 450 + _random.Next(200),
                OnTimePercentage = 88 + (_random.NextDouble() * 10),
                CustomerRating = 4.3 + (_random.NextDouble() * 0.7),
                AverageDeliveryTime = 25 + _random.Next(10),
                RiskScore = 45 + _random.Next(40)
            }).OrderByDescending(d => d.TotalDeliveries).ToList();
        }

        public async Task<RevenueAnalysis> GetRevenueAnalysisAsync(string period)
        {
            await Task.Delay(100);
            
            var dailyBreakdown = new List<DailyRevenue>();
            var days = period == "week" ? 7 : 30;

            for (int i = 0; i < days; i++)
            {
                dailyBreakdown.Add(new DailyRevenue
                {
                    Date = DateTime.Today.AddDays(-days + i),
                    Revenue = 120000m + (decimal)(_random.Next(75000)), // ZAR
                    OrderCount = 700 + _random.Next(300)
                });
            }

            var totalRevenue = dailyBreakdown.Sum(d => d.Revenue);
            var previousRevenue = totalRevenue * 0.85m;

            return new RevenueAnalysis
            {
                Period = DateTime.Today,
                TotalRevenue = totalRevenue,
                PreviousPeriodRevenue = previousRevenue,
                GrowthPercentage = 15.5,
                DailyBreakdown = dailyBreakdown
            };
        }

        public async Task<byte[]> ExportReportAsync(string format, Dictionary<string, string> filters)
        {
            await Task.Delay(500);
            // Simulate export - return empty byte array
            return Array.Empty<byte>();
        }

        private List<TrendDataPoint> GenerateDeliveryTrends()
        {
            var times = new[] { "6:00", "8:00", "10:00", "12:00", "14:00", "16:00", "18:00" };
            var baseValues = new[] { 45, 58, 72, 68, 75, 80, 65 };

            return times.Select((time, index) => new TrendDataPoint
            {
                Label = time,
                Value = baseValues[index] + _random.Next(-5, 6),
                Timestamp = DateTime.Today.AddHours(6 + index * 2)
            }).ToList();
        }

        private List<TrendDataPoint> GenerateEfficiencyTrends()
        {
            var times = new[] { "6:00", "8:00", "10:00", "12:00", "14:00", "16:00", "18:00" };
            var baseValues = new[] { 85, 88, 92, 90, 94, 96, 91 };

            return times.Select((time, index) => new TrendDataPoint
            {
                Label = time,
                Value = baseValues[index] + _random.Next(-3, 4),
                Timestamp = DateTime.Today.AddHours(6 + index * 2)
            }).ToList();
        }

        // =============================================
        // NEW: Real Database Analytics Methods
        // =============================================

        public async Task<DashboardKpiMetrics> GetDashboardKpiMetricsAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            
            var activeDriversCount = await context.Drivers
                .CountAsync(d => d.CurrentStatus == "Active");

            var openPanicAlerts = await context.PanicAlerts
                .CountAsync(pa => pa.Status == "Active");

            // Safe average calculation
            var avgSafetyScore = 0.0;
            if (await context.SafetyScores.AnyAsync())
            {
                avgSafetyScore = await context.SafetyScores
                    .AverageAsync(ss => (double)ss.OverallScore);
            }

            var highRiskZones = await context.RiskZones
                .CountAsync(rz => rz.RiskLevel == "High" || rz.RiskLevel == "Critical");

            return new DashboardKpiMetrics
            {
                ActiveDriversCount = activeDriversCount,
                OpenPanicAlertsCount = openPanicAlerts,
                AverageSafetyScore = Math.Round(avgSafetyScore, 1),
                HighRiskZonesCount = highRiskZones
            };
        }

        public async Task<List<PanicAlertTrend>> GetPanicAlertTrendsAsync(int days = 7)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var startDate = DateTime.Today.AddDays(-days);

            var alerts = await context.PanicAlerts
                .Where(pa => pa.CreatedAt >= startDate)
                .GroupBy(pa => pa.CreatedAt.Date)
                .Select(g => new PanicAlertTrend
                {
                    Date = g.Key,
                    AlertCount = g.Count(),
                    CriticalCount = g.Count(pa => pa.AlertType == "Hijack" || pa.AlertType == "Panic"),
                    MediumCount = g.Count(pa => pa.AlertType == "Accident"),
                    LowCount = g.Count(pa => pa.AlertType == "Medical")
                })
                .OrderBy(t => t.Date)
                .ToListAsync();

            // Fill missing days with zero counts
            var allDates = Enumerable.Range(0, days)
                .Select(i => DateTime.Today.AddDays(-days + i + 1))
                .ToList();

            var result = allDates.Select(date => 
            {
                var existing = alerts.FirstOrDefault(a => a.Date == date);
                return existing ?? new PanicAlertTrend 
                { 
                    Date = date, 
                    AlertCount = 0,
                    CriticalCount = 0,
                    MediumCount = 0,
                    LowCount = 0
                };
            }).ToList();

            return result;
        }

        public async Task<List<RiskLevelDistribution>> GetRiskDistributionAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var distribution = await context.RiskZones
                .GroupBy(rz => rz.RiskLevel)
                .Select(g => new RiskLevelDistribution
                {
                    RiskLevel = g.Key,
                    ZoneCount = g.Count(),
                    TotalIncidents = g.Sum(rz => rz.IncidentCount ?? 0)
                })
                .ToListAsync();

            return distribution;
        }
    }

    // =============================================
    // Analytics DTOs
    // =============================================

    public class DashboardKpiMetrics
    {
        public int ActiveDriversCount { get; set; }
        public int OpenPanicAlertsCount { get; set; }
        public double AverageSafetyScore { get; set; }
        public int HighRiskZonesCount { get; set; }
    }

    public class PanicAlertTrend
    {
        public DateTime Date { get; set; }
        public int AlertCount { get; set; }
        public int CriticalCount { get; set; }
        public int MediumCount { get; set; }
        public int LowCount { get; set; }
    }

    public class RiskLevelDistribution
    {
        public string RiskLevel { get; set; } = string.Empty;
        public int ZoneCount { get; set; }
        public int TotalIncidents { get; set; }
    }
}
