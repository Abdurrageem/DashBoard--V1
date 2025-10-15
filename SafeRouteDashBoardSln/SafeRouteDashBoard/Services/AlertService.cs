using SafeRouteDashBoard.Models;

namespace SafeRouteDashBoard.Services
{
    public interface IAlertService
    {
        Task<List<Alert>> GetActiveAlertsAsync();
        Task<List<Alert>> GetAlertsBySeverityAsync(AlertSeverity severity);
        Task<List<Alert>> GetAlertsByDriverAsync(string driverId);
        Task<Alert> CreateAlertAsync(Alert alert);
        Task ResolveAlertAsync(string id);
        Task DismissAlertAsync(string id);
        Task AssignAlertAsync(string alertId, string userId);
        Task AddAlertNoteAsync(string alertId, string note);
    }

    public class AlertService : IAlertService
    {
        private readonly List<Alert> _alerts;
        private readonly Random _random = new();

        public AlertService()
        {
            _alerts = GenerateMockAlerts();
        }

        public async Task<List<Alert>> GetActiveAlertsAsync()
        {
            await Task.Delay(100);
            return _alerts.Where(a => a.Status == AlertStatus.Active).ToList();
        }

        public async Task<List<Alert>> GetAlertsBySeverityAsync(AlertSeverity severity)
        {
            await Task.Delay(100);
            return _alerts.Where(a => a.Severity == severity && a.Status == AlertStatus.Active).ToList();
        }

        public async Task<List<Alert>> GetAlertsByDriverAsync(string driverId)
        {
            await Task.Delay(100);
            return _alerts.Where(a => a.DriverId == driverId).ToList();
        }

        public async Task<Alert> CreateAlertAsync(Alert alert)
        {
            await Task.Delay(50);
            alert.Id = $"alert-{_alerts.Count + 1:D3}";
            alert.Timestamp = DateTime.Now;
            alert.Status = AlertStatus.Active;
            _alerts.Add(alert);
            return alert;
        }

        public async Task ResolveAlertAsync(string id)
        {
            await Task.Delay(50);
            var alert = _alerts.FirstOrDefault(a => a.Id == id);
            if (alert != null)
            {
                alert.Status = AlertStatus.Resolved;
                alert.ResolvedTimestamp = DateTime.Now;
            }
        }

        public async Task DismissAlertAsync(string id)
        {
            await Task.Delay(50);
            var alert = _alerts.FirstOrDefault(a => a.Id == id);
            if (alert != null)
            {
                alert.Status = AlertStatus.Dismissed;
            }
        }

        public async Task AssignAlertAsync(string alertId, string userId)
        {
            await Task.Delay(50);
            var alert = _alerts.FirstOrDefault(a => a.Id == alertId);
            if (alert != null)
            {
                alert.AssignedTo = userId;
            }
        }

        public async Task AddAlertNoteAsync(string alertId, string note)
        {
            await Task.Delay(50);
            var alert = _alerts.FirstOrDefault(a => a.Id == alertId);
            if (alert != null)
            {
                alert.Notes = string.IsNullOrEmpty(alert.Notes) ? note : $"{alert.Notes}\n{note}";
            }
        }

        private List<Alert> GenerateMockAlerts()
        {
            var alerts = new List<Alert>
            {
                new Alert
                {
                    Id = "alert-001",
                    Title = "Emergency button pressed by Emily Watson",
                    Message = "Driver has activated the emergency button. Immediate assistance required.",
                    Severity = AlertSeverity.Critical,
                    Category = AlertCategory.Safety,
                    DriverId = "driver-004",
                    VehicleId = "VEH-104",
                    RouteId = "SR-2024-004",
                    Timestamp = DateTime.Now.AddMinutes(-7),
                    Status = AlertStatus.Active,
                    Location = new Location
                    {
                        Address = "Greenwich Village, NY",
                        City = "New York",
                        State = "NY"
                    }
                },
                new Alert
                {
                    Id = "alert-002",
                    Title = "Vehicle maintenance required for SR-2024-003",
                    Message = "Vehicle VEH-103 requires immediate maintenance. Oil change overdue by 500 miles.",
                    Severity = AlertSeverity.High,
                    Category = AlertCategory.Vehicle,
                    DriverId = "driver-003",
                    VehicleId = "VEH-103",
                    RouteId = "SR-2024-003",
                    Timestamp = DateTime.Now.AddMinutes(-17),
                    Status = AlertStatus.Active,
                    Location = new Location
                    {
                        Address = "Queens, NY",
                        City = "New York",
                        State = "NY"
                    }
                },
                new Alert
                {
                    Id = "alert-003",
                    Title = "Package theft reported in Greenwich Village area",
                    Message = "Multiple reports of package theft in the delivery area. Exercise caution.",
                    Severity = AlertSeverity.Medium,
                    Category = AlertCategory.Theft,
                    DriverId = "",
                    VehicleId = "",
                    RouteId = "",
                    Timestamp = DateTime.Now.AddMinutes(-35),
                    Status = AlertStatus.Active,
                    Location = new Location
                    {
                        Address = "Greenwich Village, NY",
                        City = "New York",
                        State = "NY"
                    }
                }
            };

            return alerts;
        }
    }
}
