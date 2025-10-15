using SafeRouteDashBoard.Models;

namespace SafeRouteDashBoard.Services
{
    public interface ISafetyService
    {
        Task<SafetyMetrics> GetSafetyMetricsAsync(string period);
        Task<RiskDistribution> GetRiskDistributionAsync();
        Task<RiskFactors> GetRiskFactorsAsync();
        Task<List<PredictiveInsight>> GetPredictiveInsightsAsync();
        Task<List<Incident>> GetIncidentHistoryAsync(Dictionary<string, string> filters);
        Task<Dictionary<string, bool>> GetSafetyComplianceAsync();
    }

    public class SafetyService : ISafetyService
    {
        private readonly Random _random = new();

        public async Task<SafetyMetrics> GetSafetyMetricsAsync(string period)
        {
            await Task.Delay(100);
            
            return new SafetyMetrics
            {
                FleetScore = 82,
                ActiveAlerts = 3,
                CriticalAlertCount = 1,
                OnTimePercentage = 94.2,
                IncidentCount = 3,
                FleetScoreChange = 2.3,
                OnTimeChange = 1.2,
                IncidentTrend = -1,
                IncidentTypes = new List<IncidentType>
                {
                    new() { Type = "Traffic Violation", Count = 2, Percentage = 40 },
                    new() { Type = "Vehicle Damage", Count = 1, Percentage = 20 },
                    new() { Type = "Delivery Delay", Count = 2, Percentage = 40 }
                },
                RiskDistribution = await GetRiskDistributionAsync(),
                RiskFactors = await GetRiskFactorsAsync(),
                Insights = await GetPredictiveInsightsAsync()
            };
        }

        public async Task<RiskDistribution> GetRiskDistributionAsync()
        {
            await Task.Delay(50);
            
            return new RiskDistribution
            {
                LowRiskPercentage = 68,
                MediumRiskPercentage = 24,
                HighRiskPercentage = 8,
                TotalDrivers = 48
            };
        }

        public async Task<RiskFactors> GetRiskFactorsAsync()
        {
            await Task.Delay(50);
            
            return new RiskFactors
            {
                TimeRisk = RiskLevel.Low,
                TimeRiskDescription = "Peak hours: 5-7 PM",
                TimeRiskPercentage = 30,
                
                LocationRisk = RiskLevel.Medium,
                LocationRiskDescription = "3 high-risk zones active",
                LocationRiskPercentage = 60,
                
                TrafficRisk = RiskLevel.High,
                TrafficRiskDescription = "Heavy traffic conditions",
                TrafficRiskPercentage = 85,
                
                WeatherRisk = RiskLevel.Low,
                WeatherRiskDescription = "Clear conditions",
                WeatherRiskPercentage = 20
            };
        }

        public async Task<List<PredictiveInsight>> GetPredictiveInsightsAsync()
        {
            await Task.Delay(50);
            
            return new List<PredictiveInsight>
            {
                new()
                {
                    Id = "insight-001",
                    Title = "Theft Risk Elevated",
                    Message = "Package theft incidents increased 15% in Zone 3. Consider rerouting.",
                    Type = InsightType.Warning,
                    Priority = InsightPriority.High,
                    CreatedAt = DateTime.Now.AddMinutes(-30),
                    IsDismissed = false
                },
                new()
                {
                    Id = "insight-002",
                    Title = "Rush Hour Alert",
                    Message = "Traffic congestion expected to peak at 5:30 PM. Adjust delivery schedules.",
                    Type = InsightType.Information,
                    Priority = InsightPriority.Medium,
                    CreatedAt = DateTime.Now.AddHours(-1),
                    IsDismissed = false
                }
            };
        }

        public async Task<List<Incident>> GetIncidentHistoryAsync(Dictionary<string, string> filters)
        {
            await Task.Delay(100);
            
            return new List<Incident>
            {
                new()
                {
                    Id = "INC-001",
                    Date = DateTime.Now.AddDays(-2),
                    DriverId = "driver-002",
                    DriverName = "Michael Chen",
                    Type = "Traffic Violation",
                    Severity = AlertSeverity.Medium,
                    Status = IncidentStatus.Resolved,
                    Description = "Minor traffic violation - failure to signal",
                    Location = new Location { Address = "Manhattan, NY", City = "New York", State = "NY" }
                },
                new()
                {
                    Id = "INC-002",
                    Date = DateTime.Now.AddDays(-5),
                    DriverId = "driver-003",
                    DriverName = "Sarah Johnson",
                    Type = "Vehicle Damage",
                    Severity = AlertSeverity.High,
                    Status = IncidentStatus.UnderInvestigation,
                    Description = "Minor fender bender in parking lot",
                    Location = new Location { Address = "Queens, NY", City = "New York", State = "NY" }
                },
                new()
                {
                    Id = "INC-003",
                    Date = DateTime.Now.AddDays(-7),
                    DriverId = "driver-001",
                    DriverName = "Emily Watson",
                    Type = "Delivery Delay",
                    Severity = AlertSeverity.Low,
                    Status = IncidentStatus.Closed,
                    Description = "30-minute delay due to traffic",
                    Location = new Location { Address = "Brooklyn, NY", City = "New York", State = "NY" }
                }
            };
        }

        public async Task<Dictionary<string, bool>> GetSafetyComplianceAsync()
        {
            await Task.Delay(50);
            
            return new Dictionary<string, bool>
            {
                { "VehicleInspections", true },
                { "DriverTraining", true },
                { "InsuranceCoverage", true },
                { "DOTCompliance", false },
                { "SafetyEquipment", true }
            };
        }
    }
}
