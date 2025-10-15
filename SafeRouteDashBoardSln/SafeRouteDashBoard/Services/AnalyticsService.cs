using SafeRouteDashBoard.Models;

namespace SafeRouteDashBoard.Services
{
    public interface IAnalyticsService
    {
        Task<Analytics> GetDailyMetricsAsync(DateTime date);
        Task<List<TrendDataPoint>> GetTrendDataAsync(DateTime startDate, DateTime endDate);
        Task<List<DriverPerformance>> GetDriverPerformanceComparisonAsync();
        Task<RevenueAnalysis> GetRevenueAnalysisAsync(string period);
        Task<byte[]> ExportReportAsync(string format, Dictionary<string, string> filters);
    }

    public class AnalyticsService : IAnalyticsService
    {
        private readonly Random _random = new();

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
    }
}
