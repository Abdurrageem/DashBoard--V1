using SafeRouteDashBoard.Models;

namespace SafeRouteDashBoard.Services
{
    public interface IDashboardService
    {
        Task<DashboardData> GetDashboardDataAsync();
        Task<KpiMetrics> GetKpiMetricsAsync();
        Task RefreshDataAsync();
    }

    public class DashboardService : IDashboardService
    {
        private readonly IDriverService _driverService;
        private readonly IAlertService _alertService;
        private readonly Random _random = new();

        public DashboardService(IDriverService driverService, IAlertService alertService)
        {
            _driverService = driverService;
            _alertService = alertService;
        }

        public async Task<DashboardData> GetDashboardDataAsync()
        {
            var activeDrivers = await _driverService.GetActiveDriversAsync();
            var activeAlerts = await _alertService.GetActiveAlertsAsync();
            var driverStats = await _driverService.GetDriverStatisticsAsync();

            return new DashboardData
            {
                KpiMetrics = await GetKpiMetricsAsync(),
                ActiveDrivers = activeDrivers,
                DriverStats = driverStats,
                ActiveAlerts = activeAlerts,
                SystemStatus = new SystemStatus
                {
                    IsOnline = true,
                    LastUpdate = DateTime.Now,
                    Version = "1.0.0",
                    Services = new List<ServiceStatus>
                    {
                        new() { Name = "API Gateway", IsHealthy = true, Status = "Online", LastCheck = DateTime.Now },
                        new() { Name = "Database", IsHealthy = true, Status = "Online", LastCheck = DateTime.Now },
                        new() { Name = "SignalR Hub", IsHealthy = true, Status = "Online", LastCheck = DateTime.Now }
                    }
                },
                LastUpdated = DateTime.Now
            };
        }

        public async Task<KpiMetrics> GetKpiMetricsAsync()
        {
            await Task.Delay(50);
            var driverStats = await _driverService.GetDriverStatisticsAsync();

            return new KpiMetrics
            {
                ActiveDriversCount = driverStats.ActiveCount,
                TotalDrivers = driverStats.TotalCount,
                DeliveriesToday = 847,
                OnTimePercentage = 94.2,
                ActiveAlertsCount = 3,
                FleetSafetyScore = 82,
                ActiveDriversTrend = GenerateTrendData(7),
                DeliveriesTrend = GenerateTrendData(7),
                SafetyScoreTrend = GenerateTrendData(7)
            };
        }

        public async Task RefreshDataAsync()
        {
            await Task.Delay(100);
            // Simulate data refresh
        }

        private List<double> GenerateTrendData(int count)
        {
            var trend = new List<double>();
            var baseValue = 80 + _random.Next(20);

            for (int i = 0; i < count; i++)
            {
                baseValue += _random.Next(-5, 6);
                trend.Add(Math.Max(50, Math.Min(100, baseValue)));
            }

            return trend;
        }
    }
}
