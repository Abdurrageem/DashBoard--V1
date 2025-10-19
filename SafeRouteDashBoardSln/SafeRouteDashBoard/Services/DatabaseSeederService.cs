using SafeRouteDashBoard.Data;
using SafeRouteDashBoard.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace SafeRouteDashBoard.Services
{
    public interface IDatabaseSeederService
    {
        Task SeedDatabaseAsync();
        Task<bool> IsDatabaseSeededAsync();
    }

    public class DatabaseSeederService : IDatabaseSeederService
    {
        private readonly IDbContextFactory<SafeRouteDbContext> _contextFactory;
        private readonly ILogger<DatabaseSeederService> _logger;

        public DatabaseSeederService(IDbContextFactory<SafeRouteDbContext> contextFactory, ILogger<DatabaseSeederService> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<bool> IsDatabaseSeededAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Companies.AnyAsync();
        }

        public async Task SeedDatabaseAsync()
        {
            try
            {
                // Check if already seeded
                if (await IsDatabaseSeededAsync())
                {
                    _logger.LogInformation("Database already seeded. Skipping...");
                    return;
                }

                _logger.LogInformation("Starting database seeding...");

                // Use a single context instance for all seeding operations
                using var context = await _contextFactory.CreateDbContextAsync();

                // 1. Seed Companies
                await SeedCompaniesAsync(context);

                // 2. Seed Users
                await SeedUsersAsync(context);

                // 3. Seed Drivers
                await SeedDriversAsync(context);

                // 4. Seed Dispatchers
                await SeedDispatchersAsync(context);

                // 5. Seed Deliveries
                await SeedDeliveriesAsync(context);

                // 6. Seed Panic Alerts
                await SeedPanicAlertsAsync(context);

                // 7. Seed Risk Zones
                await SeedRiskZonesAsync(context);

                // 8. Seed Safety Scores
                await SeedSafetyScoresAsync(context);

                // 9. Seed Location Updates
                await SeedLocationUpdatesAsync(context);

                _logger.LogInformation("Database seeding completed successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding database");
                throw;
            }
        }

        private async Task SeedCompaniesAsync(SafeRouteDbContext context)
        {
            var companies = new List<Company>
            {
                new() { RegistrationNumber = "2023/123456/07" },
                new() { RegistrationNumber = "2022/654321/07" }
            };

            await context.Companies.AddRangeAsync(companies);
            await context.SaveChangesAsync();
            _logger.LogInformation($"Seeded {companies.Count} companies");
        }

        private async Task SeedUsersAsync(SafeRouteDbContext context)
        {
            var users = new List<UserEntity>
            {
                new() { Role = "Admin", CompanyId = 1 },
                new() { Role = "Dispatcher", CompanyId = 1 },
                new() { Role = "Driver", CompanyId = 1 },
                new() { Role = "Driver", CompanyId = 1 },
                new() { Role = "Driver", CompanyId = 1 },
                new() { Role = "Driver", CompanyId = 1 },
                new() { Role = "Driver", CompanyId = 1 }
            };

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
            _logger.LogInformation($"Seeded {users.Count} users");
        }

        private async Task SeedDriversAsync(SafeRouteDbContext context)
        {
            var drivers = new List<Driver>
            {
                new() { UserId = 3, CurrentStatus = "Active" },
                new() { UserId = 4, CurrentStatus = "Active" },
                new() { UserId = 5, CurrentStatus = "OnBreak" },
                new() { UserId = 6, CurrentStatus = "Offline" },
                new() { UserId = 7, CurrentStatus = "Active" }
            };

            await context.Drivers.AddRangeAsync(drivers);
            await context.SaveChangesAsync();
            _logger.LogInformation($"Seeded {drivers.Count} drivers");
        }

        private async Task SeedDispatchersAsync(SafeRouteDbContext context)
        {
            var dispatchers = new List<Dispatcher>
            {
                new() { UserId = 2, AssignedDrivers = "1,2,3,4,5" }
            };

            await context.Dispatchers.AddRangeAsync(dispatchers);
            await context.SaveChangesAsync();
            _logger.LogInformation($"Seeded {dispatchers.Count} dispatchers");
        }

        private async Task SeedDeliveriesAsync(SafeRouteDbContext context)
        {
            var today = DateTime.Today;
            var deliveries = new List<Delivery>
            {
                // Today's deliveries
                new() { DriverId = 1, RiskLevel = "Low", Status = "Completed", CreatedAt = today, CompletedAt = today.AddHours(2) },
                new() { DriverId = 2, RiskLevel = "Medium", Status = "InProgress", CreatedAt = today },
                new() { DriverId = 1, RiskLevel = "Low", Status = "Completed", CreatedAt = today.AddHours(-1), CompletedAt = today.AddHours(1) },
                new() { DriverId = 3, RiskLevel = "High", Status = "Completed", CreatedAt = today.AddHours(-3), CompletedAt = today.AddHours(-1) },
                new() { DriverId = 5, RiskLevel = "Low", Status = "Completed", CreatedAt = today.AddHours(-4), CompletedAt = today.AddHours(-2) },
                
                // Yesterday's deliveries
                new() { DriverId = 1, RiskLevel = "Medium", Status = "Completed", CreatedAt = today.AddDays(-1), CompletedAt = today.AddDays(-1).AddHours(3) },
                new() { DriverId = 2, RiskLevel = "Low", Status = "Completed", CreatedAt = today.AddDays(-1), CompletedAt = today.AddDays(-1).AddHours(2) },
                new() { DriverId = 3, RiskLevel = "Low", Status = "Completed", CreatedAt = today.AddDays(-1), CompletedAt = today.AddDays(-1).AddHours(4) },
                
                // Last 7 days
                new() { DriverId = 1, RiskLevel = "Low", Status = "Completed", CreatedAt = today.AddDays(-2), CompletedAt = today.AddDays(-2).AddHours(2) },
                new() { DriverId = 2, RiskLevel = "Medium", Status = "Completed", CreatedAt = today.AddDays(-3), CompletedAt = today.AddDays(-3).AddHours(3) },
                new() { DriverId = 3, RiskLevel = "Low", Status = "Completed", CreatedAt = today.AddDays(-4), CompletedAt = today.AddDays(-4).AddHours(2) },
                new() { DriverId = 5, RiskLevel = "High", Status = "Completed", CreatedAt = today.AddDays(-5), CompletedAt = today.AddDays(-5).AddHours(4) },
                new() { DriverId = 1, RiskLevel = "Low", Status = "Completed", CreatedAt = today.AddDays(-6), CompletedAt = today.AddDays(-6).AddHours(3) }
            };

            await context.Deliveries.AddRangeAsync(deliveries);
            await context.SaveChangesAsync();
            _logger.LogInformation($"Seeded {deliveries.Count} deliveries");
        }

        private async Task SeedPanicAlertsAsync(SafeRouteDbContext context)
        {
            var now = DateTime.Now;
            var alerts = new List<PanicAlert>
            {
                // Active alerts
                new() { DriverId = 1, AlertType = "Panic", Status = "Active", CreatedAt = now.AddMinutes(-10) },
                new() { DriverId = 2, AlertType = "Hijack", Status = "Active", CreatedAt = now.AddMinutes(-5) },
                
                // Resolved alerts (last 7 days for charts)
                new() { DriverId = 1, AlertType = "Panic", Status = "Resolved", CreatedAt = now.AddDays(-1), ResolvedAt = now.AddDays(-1).AddHours(1) },
                new() { DriverId = 2, AlertType = "Medical", Status = "Resolved", CreatedAt = now.AddDays(-2), ResolvedAt = now.AddDays(-2).AddMinutes(30) },
                new() { DriverId = 1, AlertType = "Accident", Status = "Resolved", CreatedAt = now.AddDays(-3), ResolvedAt = now.AddDays(-3).AddHours(2) },
                new() { DriverId = 3, AlertType = "Hijack", Status = "Resolved", CreatedAt = now.AddDays(-4), ResolvedAt = now.AddDays(-4).AddHours(1) },
                new() { DriverId = 2, AlertType = "Panic", Status = "Resolved", CreatedAt = now.AddDays(-5), ResolvedAt = now.AddDays(-5).AddMinutes(45) },
                new() { DriverId = 1, AlertType = "Medical", Status = "Resolved", CreatedAt = now.AddDays(-6), ResolvedAt = now.AddDays(-6).AddMinutes(20) },
                
                // Acknowledged alert
                new() { DriverId = 3, AlertType = "Accident", Status = "Acknowledged", AcknowledgedByDispatcher = 1, CreatedAt = now.AddHours(-2) }
            };

            await context.PanicAlerts.AddRangeAsync(alerts);
            await context.SaveChangesAsync();
            _logger.LogInformation($"Seeded {alerts.Count} panic alerts");
        }

        private async Task SeedRiskZonesAsync(SafeRouteDbContext context)
        {
            var zones = new List<RiskZone>
            {
                new() { RiskLevel = "High", BoundaryCoordinates = "[{\"lat\":-26.2041,\"lng\":28.0473}]", IncidentCount = 15 },
                new() { RiskLevel = "Medium", BoundaryCoordinates = "[{\"lat\":-26.1500,\"lng\":28.0300}]", IncidentCount = 5 },
                new() { RiskLevel = "Critical", BoundaryCoordinates = "[{\"lat\":-26.3500,\"lng\":28.1500}]", IncidentCount = 42 },
                new() { RiskLevel = "Low", BoundaryCoordinates = "[{\"lat\":-26.0500,\"lng\":28.0200}]", IncidentCount = 1 }
            };

            await context.RiskZones.AddRangeAsync(zones);
            await context.SaveChangesAsync();
            _logger.LogInformation($"Seeded {zones.Count} risk zones");
        }

        private async Task SeedSafetyScoresAsync(SafeRouteDbContext context)
        {
            var now = DateTime.Now;
            var scores = new List<SafetyScore>
            {
                // Current scores
                new() { DriverId = 1, OverallScore = 92.5m, Recommendations = "Excellent safety record. Continue current practices.", CalculatedAt = now },
                new() { DriverId = 2, OverallScore = 78.3m, Recommendations = "Good performance. Consider defensive driving training.", CalculatedAt = now },
                new() { DriverId = 3, OverallScore = 85.7m, Recommendations = "Very good. Maintain awareness in high-risk zones.", CalculatedAt = now },
                new() { DriverId = 5, OverallScore = 88.2m, Recommendations = "Strong performance. Keep up the good work.", CalculatedAt = now },
                
                // Historical scores (last 7 days for trends)
                new() { DriverId = 1, OverallScore = 90.0m, Recommendations = "Good", CalculatedAt = now.AddDays(-1) },
                new() { DriverId = 2, OverallScore = 75.5m, Recommendations = "Fair", CalculatedAt = now.AddDays(-1) },
                new() { DriverId = 1, OverallScore = 91.2m, Recommendations = "Excellent", CalculatedAt = now.AddDays(-2) },
                new() { DriverId = 2, OverallScore = 76.8m, Recommendations = "Good", CalculatedAt = now.AddDays(-2) },
                new() { DriverId = 1, OverallScore = 89.5m, Recommendations = "Very Good", CalculatedAt = now.AddDays(-3) },
                new() { DriverId = 2, OverallScore = 77.2m, Recommendations = "Good", CalculatedAt = now.AddDays(-3) }
            };

            await context.SafetyScores.AddRangeAsync(scores);
            await context.SaveChangesAsync();
            _logger.LogInformation($"Seeded {scores.Count} safety scores");
        }

        private async Task SeedLocationUpdatesAsync(SafeRouteDbContext context)
        {
            // Johannesburg area coordinates
            var baseLatitude = -26.2041;
            var baseLongitude = 28.0473;
            var random = new Random();

            var locationUpdates = new List<LocationUpdate>();

            // Add location updates for active drivers (last 7 days)
            for (int day = 0; day < 7; day++)
            {
                var timestamp = DateTime.Now.AddDays(-day);
                
                for (int driverId = 1; driverId <= 3; driverId++)
                {
                    locationUpdates.Add(new LocationUpdate
                    {
                        DriverId = driverId,
                        Lat = (decimal)(baseLatitude + (random.NextDouble() * 0.1 - 0.05)),
                        Lng = (decimal)(baseLongitude + (random.NextDouble() * 0.1 - 0.05)),
                        Timestamp = timestamp.AddHours(random.Next(0, 24))
                    });
                }
            }

            await context.LocationUpdates.AddRangeAsync(locationUpdates);
            await context.SaveChangesAsync();
            _logger.LogInformation($"Seeded {locationUpdates.Count} location updates");
        }
    }
}

