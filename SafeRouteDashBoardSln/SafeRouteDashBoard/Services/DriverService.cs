using SafeRouteDashBoard.Models;

namespace SafeRouteDashBoard.Services
{
    public interface IDriverService
    {
        Task<List<Driver>> GetActiveDriversAsync();
        Task<Driver?> GetDriverByIdAsync(string id);
        Task<List<Driver>> GetDriversByStatusAsync(DriverStatus status);
        Task UpdateDriverLocationAsync(string id, Location location);
        Task UpdateDriverStatusAsync(string id, DriverStatus status);
        Task<DriverPerformance> GetDriverPerformanceAsync(string id);
        Task<List<Models.Route>> GetDriverRouteHistoryAsync(string id);
        Task AssignRouteAsync(string driverId, string routeId);
        Task<List<Driver>> SearchDriversAsync(string query);
        Task<DriverStatistics> GetDriverStatisticsAsync();
    }

    public class DriverService : IDriverService
    {
        private readonly List<Driver> _drivers;
        private readonly Random _random = new();

        public DriverService()
        {
            // Initialize with mock data
            _drivers = GenerateMockDrivers();
        }

        public async Task<List<Driver>> GetActiveDriversAsync()
        {
            await Task.Delay(100); // Simulate API call
            return _drivers.Where(d => d.Status != DriverStatus.Offline).ToList();
        }

        public async Task<Driver?> GetDriverByIdAsync(string id)
        {
            await Task.Delay(50);
            return _drivers.FirstOrDefault(d => d.Id == id);
        }

        public async Task<List<Driver>> GetDriversByStatusAsync(DriverStatus status)
        {
            await Task.Delay(100);
            return _drivers.Where(d => d.Status == status).ToList();
        }

        public async Task UpdateDriverLocationAsync(string id, Location location)
        {
            await Task.Delay(50);
            var driver = _drivers.FirstOrDefault(d => d.Id == id);
            if (driver != null)
            {
                driver.CurrentLocation = location;
            }
        }

        public async Task UpdateDriverStatusAsync(string id, DriverStatus status)
        {
            await Task.Delay(50);
            var driver = _drivers.FirstOrDefault(d => d.Id == id);
            if (driver != null)
            {
                driver.Status = status;
            }
        }

        public async Task<DriverPerformance> GetDriverPerformanceAsync(string id)
        {
            await Task.Delay(100);
            var driver = _drivers.FirstOrDefault(d => d.Id == id);
            if (driver == null) return new DriverPerformance();

            return new DriverPerformance
            {
                DriverId = driver.Id,
                DriverName = driver.Name,
                TotalDeliveries = driver.TotalDeliveries,
                OnTimePercentage = driver.OnTimePercentage,
                CustomerRating = driver.Rating,
                AverageDeliveryTime = 25 + _random.Next(10),
                RiskScore = driver.RiskScore
            };
        }

        public async Task<List<Models.Route>> GetDriverRouteHistoryAsync(string id)
        {
            await Task.Delay(100);
            // Return mock route history
            return new List<Models.Route>();
        }

        public async Task AssignRouteAsync(string driverId, string routeId)
        {
            await Task.Delay(50);
            var driver = _drivers.FirstOrDefault(d => d.Id == driverId);
            if (driver != null)
            {
                driver.RouteId = routeId;
            }
        }

        public async Task<List<Driver>> SearchDriversAsync(string query)
        {
            await Task.Delay(100);
            var lowerQuery = query.ToLower();
            return _drivers
                .Where(d => d.Name.ToLower().Contains(lowerQuery) || 
                           d.Id.ToLower().Contains(lowerQuery) ||
                           d.Email.ToLower().Contains(lowerQuery))
                .ToList();
        }

        public async Task<DriverStatistics> GetDriverStatisticsAsync()
        {
            await Task.Delay(50);
            return new DriverStatistics
            {
                ActiveCount = _drivers.Count(d => d.Status == DriverStatus.Active),
                OnBreakCount = _drivers.Count(d => d.Status == DriverStatus.OnBreak),
                OfflineCount = _drivers.Count(d => d.Status == DriverStatus.Offline),
                EmergencyCount = _drivers.Count(d => d.Status == DriverStatus.Emergency),
                TotalCount = _drivers.Count
            };
        }

        private List<Driver> GenerateMockDrivers()
        {
            var names = new[] { "Emily Watson", "Michael Chen", "Sarah Johnson", "David Rodriguez", "Lisa Anderson" };
            var locations = new[] { "Sandton, Johannesburg", "Rosebank, Johannesburg", "Pretoria CBD, Pretoria", "Cape Town CBD, Cape Town", "Durban North, Durban" };
            var destinations = new[] { "Midrand", "Randburg", "Centurion", "Waterfront", "Umhlanga" };
            var statuses = new[] { DriverStatus.Active, DriverStatus.Active, DriverStatus.OnBreak, DriverStatus.Offline, DriverStatus.Emergency };

            var drivers = new List<Driver>();

            for (int i = 0; i < 5; i++)
            {
                var riskScore = statuses[i] == DriverStatus.Emergency ? 92 : (45 + _random.Next(30));
                
                drivers.Add(new Driver
                {
                    Id = $"driver-{i + 1:D3}",
                    Name = names[i],
                    Email = $"{names[i].Replace(" ", ".").ToLower()}@saferoute.co.za",
                    Phone = $"+27 {80 + i} {100 + i:D3} {1000 + i:D4}",
                    Address = $"{100 + i * 10} Main Road, Johannesburg, Gauteng",
                    Avatar = $"/api/placeholder/100/100?text={names[i].Split(' ')[0][0]}{names[i].Split(' ')[1][0]}",
                    Status = statuses[i],
                    OnlineStatus = statuses[i] == DriverStatus.Offline ? OnlineStatus.Offline : OnlineStatus.Online,
                    DeliveryCount = 8 + _random.Next(7),
                    Distance = 41.0 + _random.Next(32), // kilometres (converted from 25.5 + 20 miles)
                    OnDutyHours = 3.5 + (_random.NextDouble() * 2),
                    BreakMinutes = 15 + _random.Next(30),
                    RouteId = $"SR-2024-{i + 1:D3}",
                    RiskScore = riskScore,
                    Progress = statuses[i] == DriverStatus.Active ? 45 + _random.Next(40) : 0,
                    ETA = statuses[i] == DriverStatus.Active ? $"{14 + _random.Next(2)}:30" : "N/A",
                    CurrentLocation = new Location
                    {
                        Latitude = -26.2041 + (_random.NextDouble() * 0.1), // Johannesburg coordinates
                        Longitude = 28.0473 + (_random.NextDouble() * 0.1),
                        Address = locations[i],
                        City = "Johannesburg",
                        State = "Gauteng"
                    },
                    Destination = destinations[i],
                    Rating = 4.5 + (_random.NextDouble() * 0.5),
                    TotalDeliveries = 450 + _random.Next(200),
                    OnTimePercentage = 88.5 + (_random.NextDouble() * 10),
                    VehicleId = $"VEH-{100 + i}",
                    LicenseNumber = $"DL{1000000 + i}",
                    HireDate = DateTime.Now.AddYears(-2).AddDays(_random.Next(730)),
                    EmergencyContact = $"+27 {81 + i} {200 + i:D3} {2000 + i:D4}",
                    Notes = "Experienced driver with excellent safety record."
                });
            }

            return drivers;
        }
    }
}
